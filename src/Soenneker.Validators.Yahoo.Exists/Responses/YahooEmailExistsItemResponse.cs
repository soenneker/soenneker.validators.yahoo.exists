using System.Text.Json.Serialization;

namespace Soenneker.Validators.Yahoo.Exists.Responses;

/// <summary>
/// Represents the yahoo email exists item response.
/// </summary>
public class YahooEmailExistsItemResponse
{
    /// <summary>
    /// Gets or sets error.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}