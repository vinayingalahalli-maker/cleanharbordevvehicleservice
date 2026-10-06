# VehicleServiceCollectionSdk TypeScript SDK 1.0.0

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
  - [Runtime Compatibility](#runtime-compatibility)
- [Authentication](#authentication)
  - [API Key Authentication](#api-key-authentication)
- [Setting a Custom Timeout](#setting-a-custom-timeout)
- [Sample Usage](#sample-usage)
- [Services](#services)
- [Models](#models)

# Setup & Configuration

## Supported Language Versions

This SDK is compatible with the following versions: `TypeScript >= 4.8.4`

## Installation

To get started with the SDK, we recommend installing using `npm` or `yarn`:

```bash
npm install vehicle-service-collection-sdk
```

or

```bash
yarn add vehicle-service-collection-sdk
```

## Runtime Compatibility

The SDK works in the following runtimes:

- Node.js 18+
- Vercel
- Cloudflare Workers
- Deno v1.25+
- Bun 1.0+
- React Native

## Authentication

### API Key Authentication

The VehicleServiceCollectionSdk API uses API keys as a form of authentication. An API key is a unique identifier used to authenticate a user, developer, or a program that is calling the API.

#### Setting the API key

When you initialize the SDK, you can set the API key as follows:

```ts
const sdk = new VehicleServiceCollectionSdk({ apiKey: 'YOUR_API_KEY' });
```

If you need to set or update the API key after initializing the SDK, you can use:

```ts
const sdk = new VehicleServiceCollectionSdk();
sdk.apiKey = 'YOUR_API_KEY';
```

## Setting a Custom Timeout

You can set a custom timeout for the SDK's HTTP requests as follows:

```ts
const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({ timeoutMs: 10000 });
```

# Sample Usage

Below is a comprehensive example demonstrating how to authenticate and call a simple endpoint:

```ts
import { VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.getAllVehicles();

  console.log(data);
})();
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
