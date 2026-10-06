using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Models;
using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Models;

namespace VehicleServiceApi.Controllers;

[ApiController]
[Route("vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleServiceCollectionSdkClient _client;

    public VehiclesController(VehicleServiceCollectionSdkClient client)
    {
        _client = client;
    }

    /// <summary>Get all vehicles</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllVehicles(CancellationToken ct)
    {
        try
        {
            var result = await _client.VehicleServiceCollectionSdk.GetAllVehiclesAsync(requestConfig: null, cancellationToken: ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse(ex.Message));
        }
    }

    /// <summary>Create a vehicle</summary>
    [HttpPost]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest body, CancellationToken ct)
    {
        try
        {
            var input = new CreateAVehicleRequest(
                NickName: body.NickName,
                Vin: body.Vin,
                Make: body.Make,
                Model: body.Model,
                Year: body.Year,
                Miles: body.Miles
            );
            var result = await _client.VehicleServiceCollectionSdk.CreateAVehicleAsync(input, cancellationToken: ct);
            return StatusCode(201, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

    /// <summary>Retrieve a vehicle by ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicle(string id, CancellationToken ct)
    {
        try
        {
            var result = await _client.VehicleServiceCollectionSdk.RetrieveAVehicleAsync(id, cancellationToken: ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(new ErrorResponse(ex.Message));
        }
    }

    /// <summary>Partially update a vehicle (PATCH)</summary>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchVehicle(string id, [FromBody] UpdateVehicleRequest body, CancellationToken ct)
    {
        try
        {
            var input = new CreateAVehicleRequest(
                NickName: body.NickName,
                Vin: body.Vin,
                Make: body.Make,
                Model: body.Model,
                Year: body.Year,
                Miles: body.Miles
            );
            var result = await _client.VehicleServiceCollectionSdk.UpdateACollectionAsync(input, id, cancellationToken: ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse(ex.Message));
        }
    }

    /// <summary>Fully replace a vehicle (PUT)</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutVehicle(string id, [FromBody] UpdateVehicleRequest body, CancellationToken ct)
    {
        try
        {
            var input = new CreateAVehicleRequest(
                NickName: body.NickName,
                Vin: body.Vin,
                Make: body.Make,
                Model: body.Model,
                Year: body.Year,
                Miles: body.Miles
            );
            var result = await _client.VehicleServiceCollectionSdk.UpdateVehicleAsync(input, id, cancellationToken: ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse(ex.Message));
        }
    }

    /// <summary>Delete a vehicle</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVehicle(string id, CancellationToken ct)
    {
        try
        {
            await _client.VehicleServiceCollectionSdk.DeleteAVehicleAsync(id, cancellationToken: ct);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new ErrorResponse(ex.Message));
        }
    }
}
