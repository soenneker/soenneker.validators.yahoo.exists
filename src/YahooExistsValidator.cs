using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.HttpClient;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.RateLimiting.Executor;
using Soenneker.Utils.RateLimiting.Factory.Abstract;
using Soenneker.Validators.Yahoo.Exists.Abstract;
using Soenneker.Validators.Yahoo.Exists.Responses;
using Soenneker.Validators.Yahoo.Exists.Utils;

namespace Soenneker.Validators.Yahoo.Exists;

///<inheritdoc cref="IYahooExistsValidator"/>
public class YahooExistsValidator : Validator.Validator, IYahooExistsValidator
{
    private const string _signUpPage = "https://login.yahoo.com/account/create?specId=yidReg&lang=en-US&src=&done=https%3A%2F%2Fwww.yahoo.com&display=login";
    private const string _signUpApi = "https://login.yahoo.com/account/module/create?validateField=yid";
    private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36";

    private readonly IHttpClientCache _httpClientCache;
    private readonly IRateLimitingFactory _rateLimitingFactory;
    private readonly TimeSpan _interval;

    public YahooExistsValidator(IHttpClientCache httpClientCache, ILogger<YahooExistsValidator> logger, IRateLimitingFactory rateLimitingFactory, IConfiguration configuration) : base(logger)
    {
        _httpClientCache = httpClientCache;
        _rateLimitingFactory = rateLimitingFactory;

        const string key = $"{nameof(YahooExistsValidator)}:IntervalMs";

        var intervalMs = configuration.GetValue<int?>(key);

        if (intervalMs != null)
            _interval = TimeSpan.FromMilliseconds(intervalMs.Value);
        else
        {
            Logger.LogDebug("{key} config was not set, defaulting to 4000ms rate limiting interval", key);
            _interval = TimeSpan.FromMilliseconds(4000);
        }
    }

    public async ValueTask<bool?> EmailExists(string email, CancellationToken cancellationToken = default)
    {
        RateLimitingExecutor rateLimiter = await _rateLimitingFactory.Get(nameof(YahooExistsValidator), _interval, cancellationToken).NoSync();
        return await rateLimiter.Execute(ct => EmailExistsWithoutLimit(email, ct), cancellationToken).NoSync();
    }

    public async ValueTask<bool?> EmailExistsWithoutLimit(string email, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Checking if Yahoo account ({Email}) exists...", email);

        HttpClient client = await _httpClientCache.Get(nameof(YahooExistsValidator), cancellationToken: cancellationToken).NoSync();
        (string CookieString, string SessionIndex)? cookiesAndSessionIndex = await GetCookiesAndSessionIndex(client, cancellationToken).NoSync();

        if (cookiesAndSessionIndex == null)
        {
            Logger.LogError("Failed to retrieve necessary data while checking if Yahoo account ({Email}) exists, exiting early", email);
            return null;
        }

        (string cookieString, string sessionIndex) = cookiesAndSessionIndex.Value;
        return await CheckEmailExists(client, email, cookieString, sessionIndex, cancellationToken).NoSync();
    }

    private static async Task<(string CookieString, string SessionIndex)?> GetCookiesAndSessionIndex(HttpClient client, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, _signUpPage);
        request.Headers.Add("User-Agent", _userAgent);
        HttpResponseMessage response = await client.SendAsync(request, cancellationToken).NoSync();

        response.EnsureSuccessStatusCode();

        if (!response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? cookies))
        {
            return null;
        }

        string body = await response.Content.ReadAsStringAsync(cancellationToken).NoSync();
        string cookieString = string.Join(";", cookies);

        Match acrumbMatch = Regexes.Acrumb().Match(cookieString);

        if (!acrumbMatch.Success)
        {
            return null;
        }

        Match sessionIndexMatch = Regexes.SessionIndex().Match(body);

        if (!sessionIndexMatch.Success)
        {
            return null;
        }

        return (cookieString, sessionIndexMatch.Groups["sessionIndex"].Value);
    }

    private async ValueTask<bool?> CheckEmailExists(HttpClient client, string email, string cookieString, string sessionIndex, CancellationToken cancellationToken)
    {
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"acrumb", Regexes.Acrumb().Match(cookieString).Groups["acrumb"].Value},
            {"sessionIndex", sessionIndex},
            {"specId", "yidReg"},
            {"userId", email.Split('@')[0]}
        });

        var postRequest = new HttpRequestMessage(HttpMethod.Post, _signUpApi)
        {
            Content = httpContent,
            Headers =
            {
                {"Origin", "https://login.yahoo.com"},
                {"X-Requested-With", "XMLHttpRequest"},
                {"User-Agent", _userAgent},
                {"Referer", _signUpPage},
                {"Accept", "*/*"},
                {"Cookie", cookieString}
            }
        };

        YahooEmailExistsResponse? emailExistsResponse = await client.SendToType<YahooEmailExistsResponse>(postRequest, Logger, cancellationToken).NoSync();

        if (emailExistsResponse?.Errors?.Exists(item => item.Name == "userId" && (item.Error == "IDENTIFIER_NOT_AVAILABLE" || item.Error == "IDENTIFIER_EXISTS")) == true)
        {
            Logger.LogDebug("Yahoo account ({Email}) exists", email);
            return true;
        }

        Logger.LogDebug("Yahoo account ({Email}) does NOT exist", email);
        return false;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClientCache.RemoveSync(nameof(YahooExistsValidator));
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _httpClientCache.Remove(nameof(YahooExistsValidator));
    }
}