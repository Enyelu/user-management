using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace user_management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected (string userId, string tenantId) GetRequiredValues(bool isRequired = false)
        {
            var tenantId = User?.Claims.FirstOrDefault(x => x.Type == "TenantId")?.Value?.ToString();
            var userId = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToString();

            if (tenantId == string.Empty)
                tenantId = null;

            if ((string.IsNullOrWhiteSpace(tenantId) || string.IsNullOrWhiteSpace(userId)) && isRequired)
                throw new Exception("Unauthorized");
            return (userId, tenantId);
        }
    }
}
