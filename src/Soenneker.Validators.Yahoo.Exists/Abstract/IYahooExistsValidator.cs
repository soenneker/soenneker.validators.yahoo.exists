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
    /// Executes the email exists operation.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<bool?> EmailExists(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the email exists without limit operation.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<bool?> EmailExistsWithoutLimit(string email, CancellationToken cancellationToken = default);
}
