# VehicleServiceCollectionSdkService

A list of all methods in the `VehicleServiceCollectionSdkService` service. Click on the method name to view detailed information about that method.

| Methods                                 | Description |
| :-------------------------------------- | :---------- |
| [createAVehicle](#createavehicle)       |             |
| [retrieveAVehicle](#retrieveavehicle)   |             |
| [updateACollection](#updateacollection) |             |
| [deleteAVehicle](#deleteavehicle)       |             |
| [updateVehicle](#updatevehicle)         |             |
| [getAllVehicles](#getallvehicles)       |             |

## createAVehicle

- HTTP Method: `POST`
- Endpoint: `/vehicles`

**Parameters**

| Name | Type                                                        | Required | Description       |
| :--- | :---------------------------------------------------------- | :------- | :---------------- |
| body | [CreateAVehicleRequest](../models/CreateAVehicleRequest.md) | ✅       | The request body. |

**Return Type**

`any`

**Example Usage Code Snippet**

```typescript
import { CreateAVehicleRequest, VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const createAVehicleRequest: CreateAVehicleRequest = {
    nickName: 'The Lisa Marie',
    vin: '4M2DV11W4RDJ53329',
    make: 'Mercury',
    model: 'Villager',
    year: '1994',
    miles: 159864,
  };

  const data =
    await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.createAVehicle(
      createAVehicleRequest,
    );

  console.log(data);
})();
```

## retrieveAVehicle

- HTTP Method: `GET`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type   | Required | Description |
| :--- | :----- | :------- | :---------- |
| id   | string | ✅       |             |

**Return Type**

`any`

**Example Usage Code Snippet**

```typescript
import { VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.retrieveAVehicle('1');

  console.log(data);
})();
```

## updateACollection

- HTTP Method: `PATCH`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type                                                        | Required | Description       |
| :--- | :---------------------------------------------------------- | :------- | :---------------- |
| body | [CreateAVehicleRequest](../models/CreateAVehicleRequest.md) | ✅       | The request body. |
| id   | string                                                      | ✅       |                   |

**Return Type**

`any`

**Example Usage Code Snippet**

```typescript
import { CreateAVehicleRequest, VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const createAVehicleRequest: CreateAVehicleRequest = {
    nickName: 'The Lisa Marie',
    vin: '4M2DV11W4RDJ53329',
    make: 'Mercury',
    model: 'Villager',
    year: '1994',
    miles: 159864,
  };

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.updateACollection(
    '1',
    createAVehicleRequest,
  );

  console.log(data);
})();
```

## deleteAVehicle

- HTTP Method: `DELETE`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type   | Required | Description |
| :--- | :----- | :------- | :---------- |
| id   | string | ✅       |             |

**Return Type**

`string`

**Example Usage Code Snippet**

```typescript
import { VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.deleteAVehicle('1');

  console.log(data);
})();
```

## updateVehicle

- HTTP Method: `PUT`
- Endpoint: `/vehicles/{id}`

**Parameters**

| Name | Type                                                        | Required | Description       |
| :--- | :---------------------------------------------------------- | :------- | :---------------- |
| body | [CreateAVehicleRequest](../models/CreateAVehicleRequest.md) | ✅       | The request body. |
| id   | string                                                      | ✅       |                   |

**Return Type**

`any`

**Example Usage Code Snippet**

```typescript
import { CreateAVehicleRequest, VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const createAVehicleRequest: CreateAVehicleRequest = {
    nickName: 'The Lisa Marie',
    vin: '4M2DV11W4RDJ53329',
    make: 'Mercury',
    model: 'Villager',
    year: '1994',
    miles: 159864,
  };

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.updateVehicle(
    '2',
    createAVehicleRequest,
  );

  console.log(data);
})();
```

## getAllVehicles

- HTTP Method: `GET`
- Endpoint: `/vehicles`

**Return Type**

`any`

**Example Usage Code Snippet**

```typescript
import { VehicleServiceCollectionSdk } from 'vehicle-service-collection-sdk';

(async () => {
  const vehicleServiceCollectionSdk = new VehicleServiceCollectionSdk({
    apiKey: 'YOUR_API_KEY',
  });

  const data = await vehicleServiceCollectionSdk.vehicleServiceCollectionSdk.getAllVehicles();

  console.log(data);
})();
```
