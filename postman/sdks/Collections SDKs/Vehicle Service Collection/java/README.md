# VehicleServiceCollectionSdk Java SDK 1.0.0

Welcome to the VehicleServiceCollectionSdk SDK documentation. This guide will help you get started with integrating and using the VehicleServiceCollectionSdk SDK in your project.

## Versions

- SDK version: `1.0.0`

## About the API

This collection covers the core CRUD operations for the **Vehicle Service API**, providing a complete set of endpoints to create, retrieve, update, and delete vehicle resources.

### Endpoints

| Method   | Endpoint        | Description                          |
| -------- | --------------- | ------------------------------------ |
| `POST`   | `/vehicles`     | Create a new vehicle                 |
| `GET`    | `/vehicles/:id` | Retrieve a vehicle by its ID         |
| `PATCH`  | `/vehicles/:id` | Partially update an existing vehicle |
| `PUT`    | `/vehicles/:id` | Fully replace an existing vehicle    |
| `DELETE` | `/vehicles/:id` | Delete a vehicle by its ID           |

### Response Coverage

Each request includes saved examples that cover both success and error scenarios:

- **2xx** — `200 Success`, `201 Created`, `204 No Content`
- **4xx** — `400 Missing/Bad Request`, `404 Not Found`, `409 Conflict (Vehicle already exists)`
- **5xx** — `500 Unexpected Error`

This makes the collection well-suited for both active development and mock server usage.

### Configuration

All requests use the `{{baseUrl}}` variable. Make sure your active environment has `baseUrl` set to the correct server URL before sending requests.

## Table of Contents

- [Setup & Configuration](#setup--configuration)
  - [Supported Language Versions](#supported-language-versions)
  - [Installation](#installation)
- [Authentication](#authentication)
  - [API Key Authentication](#api-key-authentication)
- [Setting a Custom Timeout](#setting-a-custom-timeout)
- [Injecting a Custom HTTP Client](#injecting-a-custom-http-client)
- [Accessing the Raw HTTP Response](#accessing-the-raw-http-response)
- [Sample Usage](#sample-usage)
- [Services](#services)
- [Models](#models)

# Setup & Configuration

## Supported Language Versions

This SDK is compatible with the following versions: `Java >= 1.8`

## Installation

If you use Maven, place the following within the _dependency_ tag in your `pom.xml` file:

```XML
<dependency>
    <groupId>com</groupId>
    <artifactId>vehicleservicecollectionsdk</artifactId>
    <version>1.0.0</version>
</dependency>
```

If you use Gradle, paste the next line inside the _dependencies_ block of your `build.gradle` file:

```Gradle
implementation("com:vehicleservicecollectionsdk:1.0.0")
```

If you use JAR files, package the SDK by running the following command:

```shell
mvn compile assembly:single
```

Then, add the JAR file to your project's classpath.

## Authentication

### API Key Authentication

The VehicleServiceCollectionSdk API uses API keys as a form of authentication. An API key is a unique identifier used to authenticate a user, developer, or a program that is calling the API.

#### Setting the API key

When you initialize the SDK, you can set the API key as follows:

```java
import com.vehicleservicecollectionsdk.VehicleServiceCollectionSdk;
import com.vehicleservicecollectionsdk.config.ApiKeyAuthConfig;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;

public class Main {

  public static void main(String[] args) {
    ApiKeyAuthConfig apiKeyAuthConfig = ApiKeyAuthConfig.builder()
      .apiKey("YOUR_API_KEY")
      .apiKeyHeader("YOUR_API_KEY_HEADER")
      .build();

    VehicleServiceCollectionSdkConfig config = VehicleServiceCollectionSdkConfig.builder()
      .apiKeyAuthConfig(apiKeyAuthConfig)
      .build();

    VehicleServiceCollectionSdk vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk(
      config
    );
  }
}

```

If you need to set or update the API key after initializing the SDK, you can use:

```java
vehicleServiceCollectionSdk.setApiKey('YOUR_API_KEY');
vehicleServiceCollectionSdk.setApiKeyHeader('YOUR_API_KEY_HEADER');
```

## Setting a Custom Timeout

You can set a custom timeout for the SDK's HTTP requests as follows:

```java
import com.vehicleservicecollectionsdk.VehicleServiceCollectionSdk;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;

public class Main {

  public static void main(String[] args) {
    VehicleServiceCollectionSdkConfig config = VehicleServiceCollectionSdkConfig.builder()
      .timeout(10000)
      .build();
    VehicleServiceCollectionSdk vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk(
      config
    );
  }
}

```

## Injecting a Custom HTTP Client

You can supply your own `OkHttpClient` — for example to configure a proxy, a shared connection pool, custom TLS, timeouts, or your own interceptors. The SDK derives its client from the one you provide (preserving your transport settings and interceptors) and layers its own interceptors (such as authentication and retry) on top, so the SDK keeps working as usual.

```java
OkHttpClient customClient = new OkHttpClient.Builder().addInterceptor(new MyInterceptor()).build();

VehicleServiceCollectionSdk vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk(
  VehicleServiceCollectionSdkConfig.builder().httpClient(customClient).build()
);

```

`MyInterceptor` above is a placeholder for your own `okhttp3.Interceptor`.

> Your client's interceptors are added ahead of the SDK's, so on the outbound request they run before the SDK adds its own headers. A logging interceptor placed this way will **not** see SDK-injected headers such as authentication.

> **Timeout precedence:** when you inject a client, the config-level `timeout` is not applied — your client's own timeout settings are preserved. Per-request, method, and service-level timeout overrides still apply, layered on top of your client.

## Accessing the Raw HTTP Response

Every service method returns the parsed response body by default. When you also need the status code, response headers, or the raw HTTP response, call the same method through the per-call `withRawResponse()` accessor. The default methods are unchanged, so this is fully opt-in.

```java
VehicleServiceCollectionSdkResponse<Object> response =
    vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.withRawResponse().getAllVehicles();

response.getData();
response.getMetadata().getStatusCode();
response.getMetadata().getHeaders();
response.getRaw();
```

`getData()` returns the same value the default method would; `getMetadata()` exposes the status code and headers, and `getRaw()` exposes the underlying HTTP response.

# Sample Usage

Below is a comprehensive example demonstrating how to authenticate and call a simple endpoint:

```java
import com.vehicleservicecollectionsdk.VehicleServiceCollectionSdk;
import com.vehicleservicecollectionsdk.config.ApiKeyAuthConfig;
import com.vehicleservicecollectionsdk.config.VehicleServiceCollectionSdkConfig;
import com.vehicleservicecollectionsdk.exceptions.ApiError;

public class Main {

  public static void main(String[] args) {
    VehicleServiceCollectionSdkConfig config = VehicleServiceCollectionSdkConfig.builder()
      .apiKeyAuthConfig(ApiKeyAuthConfig.builder().apiKey("YOUR_API_KEY").build())
      .build();

    VehicleServiceCollectionSdk vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk(
      config
    );

    try {
      Object response = vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.getAllVehicles();

      System.out.println(response);
    } catch (ApiError e) {
      e.printStackTrace();
    }

    System.exit(0);
  }
}

```

## Services

The SDK provides various services to interact with the API.

<details>
<summary>Below is a list of all available services with links to their detailed documentation:</summary>

| Name                                                                                               |
| :------------------------------------------------------------------------------------------------- |
| [VehicleServiceCollectionSdkService](documentation/services/VehicleServiceCollectionSdkService.md) |

</details>

## Models

The SDK includes several models that represent the data structures used in API requests and responses. These models help in organizing and managing the data efficiently.

<details>
<summary>Below is a list of all available models with links to their detailed documentation:</summary>

| Name                                                                   | Description |
| :--------------------------------------------------------------------- | :---------- |
| [CreateAVehicleRequest](documentation/models/CreateAVehicleRequest.md) |             |

</details>
