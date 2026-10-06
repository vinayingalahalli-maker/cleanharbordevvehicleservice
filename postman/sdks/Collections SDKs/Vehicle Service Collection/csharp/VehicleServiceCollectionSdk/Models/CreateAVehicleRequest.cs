using System.Text.Json.Serialization;

namespace VehicleServiceCollectionSdk.Models;

public record CreateAVehicleRequest(
    [property:
        JsonPropertyName("nickName"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<string?> NickName = default,
    [property:
        JsonPropertyName("vin"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<string?> Vin = default,
    [property:
        JsonPropertyName("make"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<string?> Make = default,
    [property:
        JsonPropertyName("model"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<string?> Model = default,
    [property:
        JsonPropertyName("year"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<string?> Year = default,
    [property:
        JsonPropertyName("miles"),
        JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)
    ]
        Optional<long?> Miles = default
)
{
    [JsonExtensionData]
    public Dictionary<string, object?> AdditionalProperties { get; set; } = new();
}
