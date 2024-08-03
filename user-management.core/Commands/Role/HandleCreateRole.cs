using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.core.Commands.Role
{
    public class HandleCreateRole
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Command, GenericResponse<string>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMailService _mailService;
            private readonly ILogger<Handler> _logger;

            public Handler(RoleManager<IdentityRole> roleManager, IMailService mailService, ILogger<Handler> logger)
            {
                _logger = logger;
                _mailService = mailService;
                _roleManager = roleManager;
            }
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Attempting to create role {request.Name} at {DateTime.UtcNow}");

                var response = await _roleManager.CreateAsync(new IdentityRole { Name = request.Name });

                if (response == null)
                    return GenericResponse<string>.Fail($"Role creation failed", 500);

                if (response != null && response.Errors.Any())
                    return GenericResponse<string>.Fail($"Role creation failed because {string.Join(",", response.Errors.Select(x => x.Description))}", 400);

                _logger.LogInformation($"{request.Name} created at {DateTime.UtcNow} successfully");
                return GenericResponse<string>.Success("Success", $"{request.Name} role created");
            }
        }
    }
}
