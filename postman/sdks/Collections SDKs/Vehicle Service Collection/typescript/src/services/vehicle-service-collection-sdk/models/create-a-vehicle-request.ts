import { z } from 'zod';

/**
 * Zod schema for the CreateAVehicleRequest model.
 * Defines the structure and validation rules for this data type.
 * This is the shape used in application code - what developers interact with.
 */
export const createAVehicleRequest = z.lazy(() => {
  return z.object({
    nickName: z.string().optional().nullable(),
    vin: z.string().optional().nullable(),
    make: z.string().optional().nullable(),
    model: z.string().optional().nullable(),
    year: z.string().optional().nullable(),
    miles: z.number().optional().nullable(),
    additionalProperties: z.record(z.string(), z.unknown()).optional(),
  });
});

/**
 * @typedef {CreateAVehicleRequest} createAVehicleRequest
 * @property {string} nickName
 * @property {string} vin
 * @property {string} make
 * @property {string} model
 * @property {string} year
 * @property {number} miles
 */
export type CreateAVehicleRequest = z.infer<typeof createAVehicleRequest>;

/**
 * Zod schema for mapping API responses to the CreateAVehicleRequest application shape.
 * Handles any property name transformations from the API schema.
 * If property names match the API schema exactly, this is identical to the application shape.
 */
export const createAVehicleRequestResponse = z.lazy(() => {
  return z
    .object({
      nickName: z.string().optional().nullable(),
      vin: z.string().optional().nullable(),
      make: z.string().optional().nullable(),
      model: z.string().optional().nullable(),
      year: z.string().optional().nullable(),
      miles: z.number().optional().nullable(),
    })
    .passthrough()
    .transform((data) => {
      const additionalProperties: { [key: string]: unknown } = {};
      const declaredKeys = new Set<string>(['nickName', 'vin', 'make', 'model', 'year', 'miles']);
      for (const key of globalThis.Object.keys(data)) {
        if (!declaredKeys.has(key)) {
          additionalProperties[key] = (data as { [key: string]: unknown })[key];
        }
      }
      return {
        nickName: data['nickName'],
        vin: data['vin'],
        make: data['make'],
        model: data['model'],
        year: data['year'],
        miles: data['miles'],
        additionalProperties,
      };
    });
});

/**
 * Zod schema for mapping the CreateAVehicleRequest application shape to API requests.
 * Handles any property name transformations required by the API schema.
 * If property names match the API schema exactly, this is identical to the application shape.
 */
export const createAVehicleRequestRequest = z.lazy(() => {
  return z
    .object({
      nickName: z.string().optional().nullable(),
      vin: z.string().optional().nullable(),
      make: z.string().optional().nullable(),
      model: z.string().optional().nullable(),
      year: z.string().optional().nullable(),
      miles: z.number().optional().nullable(),
      additionalProperties: z.record(z.string(), z.unknown()).optional(),
    })
    .transform((data) => ({
      ...(data['additionalProperties'] ?? {}),
      nickName: data['nickName'],
      vin: data['vin'],
      make: data['make'],
      model: data['model'],
      year: data['year'],
      miles: data['miles'],
    }));
});
