[![](https://img.shields.io/nuget/v/soenneker.validators.yahoo.exists.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.validators.yahoo.exists/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.yahoo.exists/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.validators.yahoo.exists/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.validators.yahoo.exists.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.validators.yahoo.exists/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.yahoo.exists/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.validators.yahoo.exists/actions/workflows/codeql.yml)

# Soenneker.Validators.Yahoo.Exists

A validation module checking for Yahoo account existence.

## Install

```bash
dotnet add package Soenneker.Validators.Yahoo.Exists
```

## Quick start

```csharp
using Soenneker.Validators.Yahoo.Exists.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddYahooExistsValidatorAsSingleton();
```

Adds `IYahooExistsValidator` as a singleton service.

## What you get

- `IYahooExistsValidator` — A validation module checking for Yahoo account existence.
- `YahooExistsValidatorRegistrar` — A validation module checking for Yahoo account existence.
- `YahooEmailExistsItemResponse` — Represents the yahoo email exists item response.
- `YahooEmailExistsResponse` — Represents the yahoo email exists response.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IYahooExistsValidator.EmailExists(email, cancellationToken)` | Checks whether the mailbox exists with the target email provider. | true if the mailbox exists; false if it does not; null when the provider cannot determine the result. |
| `IYahooExistsValidator.EmailExistsWithoutLimit(email, cancellationToken)` | Checks whether the mailbox exists without applying the validator rate limit. | true if the mailbox exists; false if it does not; null when the provider cannot determine the result. |
| `YahooExistsValidatorRegistrar.AddYahooExistsValidatorAsSingleton(services)` | Adds `IYahooExistsValidator` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `YahooExistsValidatorRegistrar.AddYahooExistsValidatorAsScoped(services)` | Adds `IYahooExistsValidator` as a scoped service. | The same service collection, so additional registrations can be chained. |
| `YahooEmailExistsItemResponse.Error` | Gets or sets error. | Gets or sets error. |
| `YahooEmailExistsItemResponse.Name` | Gets or sets name. | Gets or sets name. |
| `YahooEmailExistsResponse.Errors` | Gets or sets errors. | Gets or sets errors. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Dispose instances you own when their scope ends so held resources can be released.
