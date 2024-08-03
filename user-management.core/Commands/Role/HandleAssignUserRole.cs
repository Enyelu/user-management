using Azure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.domain.Models;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.core.Commands.Role
{
    public class HandleAssignUserRole
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string Email { get; set; }
            public string RoleId { get; set; }
        }

        public class Handler : IRequestHandler<Command, GenericResponse<string>>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMailService _mailService;
            private readonly ILogger<Handler> _logger;

            public Handler(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
                 IMailService mailService, ILogger<Handler> logger)
            {
                _logger = logger;
                _userManager = userManager;
                _mailService = mailService;
                _roleManager = roleManager;
            }
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Attempting to add user {request.Email} to {request.RoleId} role at {DateTime.UtcNow}");

                var user = await _userManager.FindByEmailAsync(request.Email);
                
                if (user == null)
                    return GenericResponse<string>.Fail($"User with email {request.Email} not found");

                var role = await _roleManager.FindByIdAsync(request.RoleId);

                if (role == null)
                    return GenericResponse<string>.Fail($"Role assignment failed because role with Id {request.RoleId} does not exist", 400);

                var response = await _userManager.AddToRoleAsync(user, role.Name!);
                if (!response.Succeeded)
                    return GenericResponse<string>.Fail($"Role assignment not successful because {string.Join(",", response.Errors.Select(x => x.Description))}", 400);

                var mail = new EmailRequest()
                {
                    ToEmail = request.Email,
                    Subject = "Role Assignment",
                    Attechments = null,
                    Body = ApplicationConstants.RoleAssignmentMsg
                    .Replace("Name", user.FirstName + " " + user.LastName, StringComparison.CurrentCultureIgnoreCase)
                    .Replace("roleName", role.Name, StringComparison.CurrentCultureIgnoreCase)
                };

                var result = await _mailService.SendMailAsync(mail);
                if (!result)
                    _logger.LogError($"Unable to send email confirmation message for role assignment. user {request.Email} at {DateTime.UtcNow}.");

                _logger.LogInformation($"Assigned role to user {request.Email} at {DateTime.UtcNow} successfully");
                return GenericResponse<string>.Success("Success", "Token assignment was successful.");
            }
        }
    }
}
