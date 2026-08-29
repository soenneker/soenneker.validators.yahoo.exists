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
    /// Checks whether the mailbox exists with the target email provider.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if the mailbox exists; false if it does not; null when the provider cannot determine the result.</returns>
    ValueTask<bool?> EmailExists(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether the mailbox exists without applying the validator rate limit.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if the mailbox exists; false if it does not; null when the provider cannot determine the result.</returns>
    ValueTask<bool?> EmailExistsWithoutLimit(string email, CancellationToken cancellationToken = default);
}
