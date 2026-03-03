using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.API.Abstractions;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Commands.Devices.Abstracts;

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
    }
}
