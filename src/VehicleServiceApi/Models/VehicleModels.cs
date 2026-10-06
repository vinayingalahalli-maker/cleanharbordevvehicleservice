using System.Text.Json.Serialization;

namespace VehicleServiceApi.Models;

public record VehicleResponse(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("nickName")] string? NickName,
    [property: JsonPropertyName("vin")] string? Vin,
    [property: JsonPropertyName("make")] string? Make,
    [property: JsonPropertyName("model")] string? Model,
    [property: JsonPropertyName("year")] string? Year,
    [property: JsonPropertyName("miles")] string? Miles
);

public record CreateVehicleRequest(
    [property: JsonPropertyName("nickName")] string? NickName,
    [property: JsonPropertyName("vin")] string? Vin,
    [property: JsonPropertyName("make")] string? Make,
    [property: JsonPropertyName("model")] string? Model,
    [property: JsonPropertyName("year")] string? Year,
    [property: JsonPropertyName("miles")] long? Miles
);

public record UpdateVehicleRequest(
    [property: JsonPropertyName("nickName")] string? NickName,
    [property: JsonPropertyName("vin")] string? Vin,
    [property: JsonPropertyName("make")] string? Make,
    [property: JsonPropertyName("model")] string? Model,
    [property: JsonPropertyName("year")] string? Year,
    [property: JsonPropertyName("miles")] long? Miles
);

public record ErrorResponse(
    [property: JsonPropertyName("message")] string Message
);
