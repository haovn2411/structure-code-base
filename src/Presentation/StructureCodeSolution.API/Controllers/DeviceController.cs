using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.API.Abstractions;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Devices.Abstracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StructureCodeSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ApiController
    {
        public DeviceController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<IActionResult> CreateDevices([FromBody] Command.CreateDeviceCommand createDevice)
        {
            var result = await Sender.Send(createDevice);
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }
            return Ok(result);
        }
        //[HttpGet("{DeviceId}")]
        //[ProducesResponseType(typeof(Result<Response.DeviceResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Devices(Guid DeviceId)
        //{
        //    var result = await Sender.Send(new Query.GetDeviceByIdQuery(DeviceId));
        //    return Ok(result);
        //}

        //[HttpDelete("{DeviceId}")]
        //[ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteDevices(Guid DeviceId)
        //{
        //    var result = await Sender.Send(new Command.DeleteDeviceCommand(DeviceId));
        //    return Ok(result);
        //}

        //[HttpPut("{DeviceId}")]
        //[ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Devices(Guid DeviceId, [FromBody] Command.UpdateDeviceCommand updateDevice)
        //{
        //    var updateDeviceCommand = new Command.UpdateDeviceCommand(DeviceId, updateDevice.Name, updateDevice.Price, updateDevice.Description);
        //    var result = await Sender.Send(updateDeviceCommand);
        //    return Ok(result);
        //}
    }
}
