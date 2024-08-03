using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Commands.Tenant;
using user_management.core.DataTransferObjects;

namespace user_management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public TenantsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("id")]
        public async Task<IActionResult> FetchTenantById([FromRoute] string id)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenent([FromBody] CreateTenantDto request)
        {
            var response = await Mediator.Send(new HandleCreateTenant.Command 
            { 
                Email = request.Email, 
                Name = request.Name
            });
            return Ok(response);
        }

        [HttpPatch("add-lock")]
        public async Task<IActionResult> AddLockToTenant([FromBody] AddLockToTenantDto request)
        {
            var response = await Mediator.Send(new HandleAddLockToTenant.Command 
            { 
                LockId = request.LockId, 
                TenantId = request.TenantId
            });
            return Ok(response);
        }
    }
}