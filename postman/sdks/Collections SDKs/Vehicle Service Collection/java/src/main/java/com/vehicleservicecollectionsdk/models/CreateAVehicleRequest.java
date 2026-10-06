package com.vehicleservicecollectionsdk.models;

import com.fasterxml.jackson.annotation.JsonAnyGetter;
import com.fasterxml.jackson.annotation.JsonAnySetter;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.HashMap;
import java.util.Map;
import lombok.Builder;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.ToString;
import lombok.With;
import lombok.extern.jackson.Jacksonized;
import org.openapitools.jackson.nullable.JsonNullable;

@Data
@Builder
@With
@ToString
@EqualsAndHashCode
@Jacksonized
public class CreateAVehicleRequest {

  @JsonProperty("nickName")
  private JsonNullable<String> nickName;

  @JsonProperty("vin")
  private JsonNullable<String> vin;

  @JsonProperty("make")
  private JsonNullable<String> make;

  @JsonProperty("model")
  private JsonNullable<String> model;

  @JsonProperty("year")
  private JsonNullable<String> year;

  @JsonProperty("miles")
  private JsonNullable<Long> miles;

  // FSM-59: capture unknown JSON fields so they round-trip on re-serialize.
  // @Builder.Default keeps the empty-map default in the Lombok-generated builder; without it the
  // builder would leave the map null and the any-setter would NPE on the first unknown field.
  // Deserialization is wired via the builder's @JsonAnySetter (see the Builder below), NOT here:
  // Lombok @Jacksonized deserializes through the builder and does not copy a field-level
  // @JsonAnySetter across, so unknown fields would be silently dropped if it lived on this field.
  @Builder.Default
  private Map<String, Object> additionalProperties = new HashMap<>();

  // @JsonAnyGetter must sit on the getter (not the field) so Jackson inlines the unknown entries on
  // serialize. On the field it double-registers with the Lombok getter and leaks a literal
  // "additionalProperties" property into every request body and object parameter.
  // Declaring the getter here also stops Lombok @Data from generating its own.
  @JsonAnyGetter
  public Map<String, Object> getAdditionalProperties() {
    return additionalProperties;
  }

  @JsonIgnore
  public String getNickName() {
    return nickName.orElse(null);
  }

  @JsonIgnore
  public String getVin() {
    return vin.orElse(null);
  }

  @JsonIgnore
  public String getMake() {
    return make.orElse(null);
  }

  @JsonIgnore
  public String getModel() {
    return model.orElse(null);
  }

  @JsonIgnore
  public String getYear() {
    return year.orElse(null);
  }

  @JsonIgnore
  public Long getMiles() {
    return miles.orElse(null);
  }

  // Overwrite lombok builder methods
  public static class CreateAVehicleRequestBuilder {

    private JsonNullable<String> nickName = JsonNullable.undefined();

    @JsonProperty("nickName")
    public CreateAVehicleRequestBuilder nickName(String value) {
      this.nickName = JsonNullable.of(value);
      return this;
    }

    private JsonNullable<String> vin = JsonNullable.undefined();

    @JsonProperty("vin")
    public CreateAVehicleRequestBuilder vin(String value) {
      this.vin = JsonNullable.of(value);
      return this;
    }

    private JsonNullable<String> make = JsonNullable.undefined();

    @JsonProperty("make")
    public CreateAVehicleRequestBuilder make(String value) {
      this.make = JsonNullable.of(value);
      return this;
    }

    private JsonNullable<String> model = JsonNullable.undefined();

    @JsonProperty("model")
    public CreateAVehicleRequestBuilder model(String value) {
      this.model = JsonNullable.of(value);
      return this;
    }

    private JsonNullable<String> year = JsonNullable.undefined();

    @JsonProperty("year")
    public CreateAVehicleRequestBuilder year(String value) {
      this.year = JsonNullable.of(value);
      return this;
    }

    private JsonNullable<Long> miles = JsonNullable.undefined();

    @JsonProperty("miles")
    public CreateAVehicleRequestBuilder miles(Long value) {
      this.miles = JsonNullable.of(value);
      return this;
    }

    @JsonAnySetter
    public CreateAVehicleRequestBuilder additionalProperties(String key, Object value) {
      if (this.additionalProperties$value == null) {
        this.additionalProperties$value = new HashMap<>();
      }
      this.additionalProperties$value.put(key, value);
      this.additionalProperties$set = true;
      return this;
    }
  }
}
