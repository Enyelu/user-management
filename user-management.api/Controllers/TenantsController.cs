using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Commands.Tenant;
using user_management.core.DataTransferObjects;
using user_management.core.Queries.Tenant;
using user_management.core.Shared;
using user_management.domain.Entities;

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

        [HttpGet("{id}")]
        [Authorize(Roles = "TenantSuperAdmin,TenantAdmin,SuperAdmin,Admin")]
        public async Task<IActionResult> FetchTenantById([FromRoute] string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && User.IsInRole("TenantSuperAdmin") || User.IsInRole("TenantAdmin"))
                return Unauthorized();

            var actualTenantId = string.IsNullOrWhiteSpace(id) ? GetRequiredValues().tenantId : id;

            var response = await Mediator.Send(new HandleFetchTenantById.Query { TenantId = id});
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> CreateTenent([FromBody] CreateTenantDto request)
        {
            var response = await Mediator.Send(new HandleCreateTenant.Command
            {
                Email = request.Email,
                Name = request.Name,
                UserId = GetRequiredValues().userId
            });
            return Ok(response);
        }
    }
}