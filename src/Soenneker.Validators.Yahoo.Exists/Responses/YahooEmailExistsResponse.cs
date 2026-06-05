using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Validators.Yahoo.Exists.Responses;

/// <summary>
/// Represents the yahoo email exists response.
/// </summary>
public class YahooEmailExistsResponse
{
    /// <summary>
    /// Gets or sets errors.
    /// </summary>
    [JsonPropertyName("errors")]
    public List<YahooEmailExistsItemResponse>? Errors { get; set; }
}