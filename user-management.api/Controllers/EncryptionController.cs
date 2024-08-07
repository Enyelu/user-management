using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_management.core.Queries.AppEncryption;
using user_management.core.Shared;

namespace user_management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ApiBaseController
    {
        [HttpGet("encrypt")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<IActionResult> Encrypt([FromQuery] string text)
        {
            var response = await Mediator.Send(new HandleEncription.Query { Text = text });
            return Ok(response);
        }
    }
}
