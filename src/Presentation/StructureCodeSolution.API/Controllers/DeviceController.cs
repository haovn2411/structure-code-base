using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.Contract.Abstractions.Shared;
using StructureCodeSolution.Contract.UseCases.V1.Device;

namespace StructureCodeSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ISender _sender;

        public DeviceController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost]
        public async Task<Result> CreateDevices([FromBody] Command.CreateDeviceCommand createDevice)
        {
            await _sender.Send(createDevice);
            return Result.Success();
        }
    }
}
