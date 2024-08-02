using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace user_management.api.Controllers
{
    public class RolesController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public RolesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("id")]
        public async Task<IActionResult> FetchRoleById([FromRoute] string id)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> FetchRoles([FromRoute] string tenantId)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPatch("assign")]
        public async Task<IActionResult> AssignUserRole([FromRoute] string searchParameter)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] object request)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }
    }
}
