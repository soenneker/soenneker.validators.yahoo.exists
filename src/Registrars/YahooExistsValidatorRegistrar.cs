using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.HttpClientCache.Registrar;
using Soenneker.Utils.RateLimiting.Factory.Registrars;
using Soenneker.Validators.Yahoo.Exists.Abstract;

namespace Soenneker.Validators.Yahoo.Exists.Registrars;

/// <summary>
/// A validation module checking for Yahoo account existence
/// </summary>
public static class YahooExistsValidatorRegistrar
{
    /// <summary>
    /// Adds <see cref="IYahooExistsValidator"/> as a singleton service. <para/>
    /// </summary>
    public static void AddYahooExistsValidatorAsSingleton(this IServiceCollection services)
    {
        services.AddRateLimitingFactoryAsSingleton();
        services.AddHttpClientCache();
        services.TryAddSingleton<IYahooExistsValidator, YahooExistsValidator>();
    }

    /// <summary>
    /// Adds <see cref="IYahooExistsValidator"/> as a scoped service. <para/>
    /// </summary>
    public static void AddYahooExistsValidatorAsScoped(this IServiceCollection services)
    {
        services.AddRateLimitingFactoryAsSingleton();
        services.AddHttpClientCache();
        services.TryAddScoped<IYahooExistsValidator, YahooExistsValidator>();
    }
}
