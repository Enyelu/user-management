using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Commands.Onboarding;

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

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromRoute] string searchParameter)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] string email, string token)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SignUp([FromBody] HandleSignUp.Command request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}