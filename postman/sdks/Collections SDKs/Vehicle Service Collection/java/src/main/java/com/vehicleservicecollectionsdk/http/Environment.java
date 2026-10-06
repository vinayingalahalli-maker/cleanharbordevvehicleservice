package com.vehicleservicecollectionsdk.http;

import lombok.Getter;

/**
 * Predefined environment configurations for the SDK.
 * Each environment represents a different base URL (e.g., production, staging, development).
 */
@Getter
public enum Environment {
  DEFAULT("https://{{baseUrl}}"),
  BASEURL("https://{{baseUrl}}");

  private final String url;

  Environment(String url) {
    this.url = url;
  }
}
