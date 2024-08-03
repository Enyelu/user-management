using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using user_management.core.DataTransferObjects;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.domain.Models;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;
using user_management.infrastructure.Shared;

namespace user_management.core.Commands.Onboarding
{
    public class HandleSignUp
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Gender { get; set; }
            public string Avatar { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public bool IsTenantStaff { get; set; }
            public string? TenantId { get; set; }
            public string RoleId { get; set; }
            public DateTime DateOfBirth { get; set; }
            public AddressDto Address { get; set; }
        }

        public class Handler : IRequestHandler<Command, GenericResponse<string>>
        {
            public readonly IMapper _mapper;
            private readonly ApplicationContext _dbContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly Settings _settings;
            private readonly IMailService _mailService;
            private readonly ILogger<Handler> _logger;

            public Handler(ApplicationContext dbContext, IMapper mapper, UserManager<AppUser> userManager, 
                IOptions<Settings> options, IMailService mailService, ILogger<Handler> logger)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _userManager = userManager;
                _settings = options.Value;
                _mailService = mailService;
                _logger = logger;
            }
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Attempting to create user {request.UserName} at {DateTime.UtcNow}");

                var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == request.TenantId);
                if (request.IsTenantStaff && (string.IsNullOrWhiteSpace(request.TenantId) || tenant == null))
                    return GenericResponse<string>.Fail($"TenantId cannot be null when user is tennant staff. Please enter correct value and retry...");

                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId);
                if (role == null)
                    return GenericResponse<string>.Fail($"Role with Id {request.RoleId} not found. Please enter correct value and retry...");

                var appUser = _mapper.Map<AppUser>(request);
                var address = _mapper.Map<Address>(request.Address);
                address.CreatedBy = appUser.Id;
                address.ModifiedBy = appUser.Id;
                appUser.Address = address;

                var password = EncryptionHelper.Decrypt(request.Password, _settings.CipherKeyIvPhrase);
                var response = await _userManager.CreateAsync(appUser, password);

                if(!response.Succeeded)
                    return GenericResponse<string>.Fail($"Registration not successful because {string.Join(",", response.Errors.Select(x => x.Description))}", 400);

                await _userManager.AddToRoleAsync(appUser, role.Name!);

                if (request.IsTenantStaff)
                {
                    var newStaff = new Staff
                    {
                        AppUserId = Guid.Parse(appUser.Id),
                        Tenant = tenant
                    };

                    tenant.StaffMembers.Add(newStaff);
                    await _dbContext.SaveChangesAsync();
                }

                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                var encodedEmailToken = Encoding.UTF8.GetBytes(emailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                var callbackUrl = $"{_settings.ConfirmEmailUrl}?email={appUser.Email}&token={validEmailToken}";

                var mail = new EmailRequest()
                {
                    ToEmail = appUser.Email,
                    Subject = "Email Confirmation",
                    Attechments = null,
                    Body = ApplicationConstants.EmailConfirmationMsg
                    .Replace("FirstName", appUser.FirstName, StringComparison.CurrentCultureIgnoreCase)
                    .Replace("callbackUrl", callbackUrl, StringComparison.CurrentCultureIgnoreCase)
                };

                var result = await _mailService.SendMailAsync(mail);
                if(!result)
                    _logger.LogError($"Unable to send email confirmation message for created user {request.UserName} at {DateTime.UtcNow}.");

                _logger.LogInformation($"Created user {request.UserName} at {DateTime.UtcNow} successfully");
                return GenericResponse<string>.Success("Success", "Registration was successful, please check your email to complete process.");
            }
        }
    }
}
