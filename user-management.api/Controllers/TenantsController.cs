using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateTenent([FromBody] object request)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }
    }
}