using Soenneker.Validators.Validator.Abstract;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Validators.Yahoo.Exists.Abstract;

/// <summary>
/// A validation module checking for Yahoo account existence
/// </summary>
public interface IYahooExistsValidator : IValidator, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Applies a Yahoo signup-response heuristic through the shared rate limiter.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when Yahoo reports the identifier unavailable or existing, <see langword="false"/> otherwise, or <see langword="null"/> when signup session data cannot be extracted.</returns>
    ValueTask<bool?> EmailExists(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Applies the Yahoo signup-response heuristic without using the validator rate limiter.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when Yahoo reports the identifier unavailable or existing, <see langword="false"/> otherwise, or <see langword="null"/> when signup session data cannot be extracted.</returns>
    ValueTask<bool?> EmailExistsWithoutLimit(string email, CancellationToken cancellationToken = default);
}
