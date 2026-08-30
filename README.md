[![](https://img.shields.io/nuget/v/soenneker.validators.yahoo.exists.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.validators.yahoo.exists/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.yahoo.exists/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.validators.yahoo.exists/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.validators.yahoo.exists.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.validators.yahoo.exists/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.yahoo.exists/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.validators.yahoo.exists/actions/workflows/codeql.yml)

# Soenneker.Validators.Yahoo.Exists

Applies an undocumented Yahoo signup-response heuristic to a supplied address's local part.

## Install

```bash
dotnet add package Soenneker.Validators.Yahoo.Exists
```

## Registration

```csharp
using Soenneker.Validators.Yahoo.Exists.Registrars;
using Microsoft.Extensions.DependencyInjection;

services.AddYahooExistsValidatorAsSingleton();
```

Scoped registration is also available. Both registrations reuse singleton HTTP-client-cache and rate-limiter-factory services. Disposing a scoped validator leaves the shared named HTTP client alive.

## Configure request spacing

```json
{
  "YahooExistsValidator": {
    "IntervalMs": 4000
  }
}
```

The default interval is 4,000 milliseconds. `EmailExists` executes through a shared named rate limiter using this spacing.

## Check an identifier

```csharp
using Soenneker.Validators.Yahoo.Exists.Abstract;

bool? result = await validator.EmailExists(
    "person@yahoo.com",
    cancellationToken);
```

The validator loads Yahoo's signup page, extracts session cookies and hidden state, then submits the text before the first `@` as a proposed Yahoo user ID. It returns:

- `true` when Yahoo reports `IDENTIFIER_NOT_AVAILABLE` or `IDENTIFIER_EXISTS` for `userId`;
- `false` when the parsed response does not contain either marker;
- `null` when the required signup cookies or session index cannot be extracted.

HTTP failures, non-success status codes, response-deserialization failures, and cancellation propagate. `EmailExistsWithoutLimit` skips only the local rate limiter; it does not bypass Yahoo's limits.

The method does not validate email syntax or require a Yahoo domain. `person@gmail.com` and `person@yahoo.com` both query the Yahoo identifier `person`, while input without `@` is submitted in full. Validate and normalize input before calling if that distinction matters.

## Reliability and privacy

This uses an undocumented signup endpoint and page structure. Yahoo can change the cookies, HTML, API response, or blocking behavior without notice, producing errors or incorrect results. It is not proof that a mailbox is reachable or owned by a user; send a verification message for ownership.

The queried local part is disclosed to Yahoo. Full addresses are no longer included in this validator's logs, but request content may still appear in upstream HTTP infrastructure or Yahoo's logs. Ensure the check is compatible with privacy requirements and provider terms.
