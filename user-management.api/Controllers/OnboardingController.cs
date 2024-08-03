using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Commands.Onboarding;
using user_management.core.Queries.Onboarding;

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
        public async Task<IActionResult> ForgotPassword([FromQuery]string email)
        {
            var response = await Mediator.Send(new HandleForgotPassword.Query{ Email = email});
            return Ok(response);
        }

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery]string email, [FromQuery]string token)
        {
            var response = await Mediator.Send(new object());
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery]string token)
        {
            var response = await Mediator.Send(new HandleConfirmEmail.Command { Email = email, Token = token});
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