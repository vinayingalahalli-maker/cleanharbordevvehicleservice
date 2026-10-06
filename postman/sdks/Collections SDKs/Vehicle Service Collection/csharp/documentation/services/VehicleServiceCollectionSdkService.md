# VehicleServiceCollectionSdkService

A list of all methods in the `VehicleServiceCollectionSdkService` service. Click on the method name to view detailed information about that method.

| Methods                                           | Description |
| :------------------------------------------------ | :---------- |
| [CreateAVehicleAsync](#createavehicleasync)       |             |
| [RetrieveAVehicleAsync](#retrieveavehicleasync)   |             |
| [UpdateACollectionAsync](#updateacollectionasync) |             |
| [DeleteAVehicleAsync](#deleteavehicleasync)       |             |
| [UpdateVehicleAsync](#updatevehicleasync)         |             |
| [GetAllVehiclesAsync](#getallvehiclesasync)       |             |

## CreateAVehicleAsync

- HTTP Method: `POST`
- Endpoint: `/vehicles`

**Parameters**

| Name  | Type                  | Required | Description       |
| :---- | :-------------------- | :------- | :---------------- |
| input | CreateAVehicleRequest | ✅       | The request body. |

**Return Type**

`object`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Models;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var input = new CreateAVehicleRequest(Optional<string?>.Of("The Lisa Marie"), Optional<string?>.Of("4M2DV11W4RDJ53329"), Optional<string?>.Of("Mercury"), Optional<string?>.Of("Villager"), Optional<string?>.Of("1994"), Optional<long?>.Of(159864));

var response = await client.VehicleServiceCollectionSdk.CreateAVehicleAsync(input);

Console.WriteLine(response);
```

## RetrieveAVehicleAsync

- HTTP Method: `GET`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type   | Required | Description |
| :--- | :----- | :------- | :---------- |
| id   | string | ✅       |             |

**Return Type**

`object`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var response = await client.VehicleServiceCollectionSdk.RetrieveAVehicleAsync("1");

Console.WriteLine(response);
```

## UpdateACollectionAsync

- HTTP Method: `PATCH`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name  | Type                  | Required | Description       |
| :---- | :-------------------- | :------- | :---------------- |
| input | CreateAVehicleRequest | ✅       | The request body. |
| id    | string                | ✅       |                   |

**Return Type**

`object`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Models;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var input = new CreateAVehicleRequest(Optional<string?>.Of("The Lisa Marie"), Optional<string?>.Of("4M2DV11W4RDJ53329"), Optional<string?>.Of("Mercury"), Optional<string?>.Of("Villager"), Optional<string?>.Of("1994"), Optional<long?>.Of(159864));

var response = await client.VehicleServiceCollectionSdk.UpdateACollectionAsync(input, "1");

Console.WriteLine(response);
```

## DeleteAVehicleAsync

- HTTP Method: `DELETE`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type   | Required | Description |
| :--- | :----- | :------- | :---------- |
| id   | string | ✅       |             |

**Return Type**

`string`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var response = await client.VehicleServiceCollectionSdk.DeleteAVehicleAsync("1");

Console.WriteLine(response);
```

## UpdateVehicleAsync

- HTTP Method: `PUT`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name  | Type                  | Required | Description       |
| :---- | :-------------------- | :------- | :---------------- |
| input | CreateAVehicleRequest | ✅       | The request body. |
| id    | string                | ✅       |                   |

**Return Type**

`object`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;
using VehicleServiceCollectionSdk.Models;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var input = new CreateAVehicleRequest(Optional<string?>.Of("The Lisa Marie"), Optional<string?>.Of("4M2DV11W4RDJ53329"), Optional<string?>.Of("Mercury"), Optional<string?>.Of("Villager"), Optional<string?>.Of("1994"), Optional<long?>.Of(159864));

var response = await client.VehicleServiceCollectionSdk.UpdateVehicleAsync(input, "2");

Console.WriteLine(response);
```

## GetAllVehiclesAsync

- HTTP Method: `GET`
- Endpoint: `/vehicles`

**Return Type**

`object`

**Example Usage Code Snippet**

```csharp
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;

var config = new VehicleServiceCollectionSdkConfig{
    ApiKeyAuth = new ApiKeyAuthConfig("YOUR_API_KEY")
};

var client = new VehicleServiceCollectionSdkClient(config);

var response = await client.VehicleServiceCollectionSdk.GetAllVehiclesAsync();

Console.WriteLine(response);
```
