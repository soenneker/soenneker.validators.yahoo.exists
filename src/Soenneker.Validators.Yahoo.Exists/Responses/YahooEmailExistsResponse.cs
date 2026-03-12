using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Validators.Yahoo.Exists.Responses;

public class YahooEmailExistsResponse
{
    [JsonPropertyName("errors")]
    public List<YahooEmailExistsItemResponse>? Errors { get; set; }
}