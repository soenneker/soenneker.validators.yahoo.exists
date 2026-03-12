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
    ValueTask<bool?> EmailExists(string email, CancellationToken cancellationToken = default);

    ValueTask<bool?> EmailExistsWithoutLimit(string email, CancellationToken cancellationToken = default);
}
