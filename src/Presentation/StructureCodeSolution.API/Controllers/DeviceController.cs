using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.API.Abstractions;
using StructureCodeSolution.Contract.Abstractions.Shared;
using StructureCodeSolution.Contract.UseCases.V1.Device;

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
        public async Task<Result> CreateDevices([FromBody] Command.CreateDeviceCommand createDevice)
        {
            await Sender.Send(createDevice);
            return Result.Success();
        }
    }
}
