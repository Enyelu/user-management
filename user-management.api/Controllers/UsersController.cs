using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Queries.User;

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

        [HttpGet("{searchParameter}")]
        public async Task<IActionResult> FetchUserBySearchParameter([FromRoute]string searchParameter)
        {
            var response = await Mediator.Send(new HandleFetchUserBySearchParameter.Query { SearchParameter = searchParameter});
            return Ok(response);
        }
    }
}
