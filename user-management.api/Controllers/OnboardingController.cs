using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace user_management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public OnboardingController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpGet("confirm-password")]
        public async Task<IActionResult> ConfirmPassword([FromRoute] string tenantId)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromRoute] string searchParameter)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] object request)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }
    }
}