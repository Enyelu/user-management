using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using user_management.core.Shared;
using user_management.domain.Models;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.core.Commands.Tenant
{
    public class HandleAddLockToTenant
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string LockId { get; set; }
            public string TenantId { get; set; }
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
                var tenantId = EncryptionHelper.Decrypt(request.TenantId, _settings.CipherKeyIvPhrase);
                var tenant = await _dbContext.Tenants.Where(x => x.Id == tenantId).FirstOrDefaultAsync();

                if (tenant == null)
                    return GenericResponse<string>.Fail("Tenant with provided Id does not exist");

                var lockId = EncryptionHelper.Decrypt(request.LockId, _settings.CipherKeyIvPhrase);

                var isParsed = Guid.TryParse(lockId, out var parsedLockId);

                if(!isParsed)
                    return GenericResponse<string>.Fail("The provided lockId is not valid");

                if (tenant.LockIds.Contains(request.LockId))
                    return GenericResponse<string>.Fail("Lock already synced with lock");

                tenant.LockIds.Add(request.LockId);
                await _dbContext.SaveChangesAsync();

                var mail = new EmailRequest()
                {
                    ToEmail = tenant.Email,
                    Subject = "Lock Added",
                    Attechments = null,
                    Body = ApplicationConstants.LockAddedMsg
                    .Replace("LockId", lockId, StringComparison.CurrentCultureIgnoreCase)
                };

                var result = await _mailService.SendMailAsync(mail);
                if (!result)
                    _logger.LogError($"Unable to send add-lock meassage to tenant for created user {tenant.Name} at {DateTime.UtcNow}.");

                return GenericResponse<string>.Success("Success", "Lock-Tenant sync completed");
            }
        }
    }
}
