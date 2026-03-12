using System.Text.Json.Serialization;

namespace Soenneker.Validators.Yahoo.Exists.Responses;

public class YahooEmailExistsItemResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}