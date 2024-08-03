using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using user_management.core.Shared;
using user_management.domain.Models;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;
using user_management.infrastructure.Shared;
using EntityModels = user_management.domain.Entities;

namespace user_management.core.Commands.Tenant
{
    public class HandleCreateTenant
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
        public class Handler : IRequestHandler<Command, GenericResponse<string>>
        {
            public readonly IMapper _mapper;
            private readonly Settings _settings;
            private readonly ILogger<Handler> _logger;
            private readonly IMailService _mailService;
            private readonly ApplicationContext _dbContext;

            public Handler(ApplicationContext dbContext, IOptions<Settings> options, IMailService mailService, ILogger<Handler> logger, IMapper mapper)
            {
                _dbContext = dbContext;
                _logger = logger;
                _settings = options.Value;
                _dbContext = dbContext;
                _mailService = mailService;
                _mapper = mapper;
            }
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingTenant = await _dbContext.Tenants.Where(x => x.Name == request.Name || x.Email == request.Email).FirstOrDefaultAsync();

                if (existingTenant != null)
                    return GenericResponse<string>.Fail("Tenant already exist", 400);

                var tenant = _mapper.Map<EntityModels.Tenant>(request);

                _dbContext.Tenants.Add(tenant);
                await _dbContext.SaveChangesAsync();

                var mail = new EmailRequest()
                {
                    ToEmail = request.Email,
                    Subject = "Tenant Created",
                    Attechments = null,
                    Body = ApplicationConstants.OnboardTenantMsg
                    .Replace("Name", request.Name, StringComparison.CurrentCultureIgnoreCase)
                    .Replace("TenantId", tenant.Id, StringComparison.CurrentCultureIgnoreCase)
                };

                var result = await _mailService.SendMailAsync(mail);
                if (!result)
                    _logger.LogError($"Unable to send tenant creation message for created user {request.Name} at {DateTime.UtcNow}.");

                return GenericResponse<string>.Success("Success", "Tenant created successfully");
            }
        }
    }
}
