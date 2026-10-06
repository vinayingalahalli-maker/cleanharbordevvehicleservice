# Vehicle Service API

A .NET 10 C# Web API that wraps the **Vehicle Service Collection SDK** and exposes a RESTful interface for managing vehicles. It delegates all data operations to the SDK client, which communicates with the underlying Vehicle Service backend.

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

---

## How to Run

From the workspace root, run:

```bash
dotnet run --project src/VehicleServiceApi
```

The API will start on `http://localhost:5000` (HTTP) by default.

---

## Swagger UI

Once the application is running, open your browser and navigate to:

```
http://localhost:5000/swagger
```

This provides an interactive UI to explore and test all available endpoints.

---

## Available Endpoints

| Method   | Path                  | Description                        |
|----------|-----------------------|------------------------------------|
| `GET`    | `/vehicles`           | Get all vehicles                   |
| `POST`   | `/vehicles`           | Create a new vehicle               |
| `GET`    | `/vehicles/{id}`      | Retrieve a vehicle by ID           |
| `PATCH`  | `/vehicles/{id}`      | Partially update a vehicle         |
| `PUT`    | `/vehicles/{id}`      | Fully replace a vehicle            |
| `DELETE` | `/vehicles/{id}`      | Delete a vehicle                   |

---

## Configuration

The backend URL used by the SDK client can be configured in two ways:

### 1. `appsettings.json` (or `appsettings.Development.json`)

```json
{
  "VehicleService": {
    "BaseUrl": "http://localhost:3000"
  }
}
```

### 2. Environment Variable

Set the environment variable using the ASP.NET Core configuration key format:

```bash
export VehicleService__BaseUrl=http://your-backend-host:3000
```

> The double underscore `__` is the standard separator for nested configuration keys in environment variables on Linux/macOS.

---

## Project Structure

```
src/VehicleServiceApi/
├── Controllers/
│   └── VehiclesController.cs   # API controller with all 6 endpoints
├── Models/
│   └── VehicleModels.cs        # Request/response record types
├── Program.cs                  # App entry point, DI registration, middleware
├── appsettings.json            # Default configuration
├── appsettings.Development.json# Development overrides
├── VehicleServiceApi.csproj    # Project file referencing the SDK
└── README.md                   # This file
```

---

## SDK Reference

This project references the generated C# SDK at:

```
postman/sdks/Collections SDKs/Vehicle Service Collection/csharp/VehicleServiceCollectionSdk/
```

The `VehicleServiceCollectionSdkClient` is registered as a singleton and injected into `VehiclesController`.
