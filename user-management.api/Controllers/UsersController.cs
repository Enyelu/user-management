using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace user_management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("id")]
        public async Task<IActionResult> FetchUserById([FromRoute] string id)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpGet("searchParameter")]
        public async Task<IActionResult> FetchUserBySearchParameter([FromRoute] string searchParameter)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }
    }
}
