using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.domain.Models;
using user_management.infrastructure.Services.Interfaces;
using user_management.infrastructure.Shared;

namespace user_management.core.Queries.Onboarding
{
    public class HandleForgotPassword
    {
        public class Query : IRequest<GenericResponse<string>>
        {
            public string Email { get; set; }
        }
        public class Handler : IRequestHandler<Query, GenericResponse<string>>
        {
            private readonly Settings _settings;
            private readonly ILogger<Handler> _logger;
            private readonly IMailService _mailService;
            private readonly UserManager<AppUser> _userManager;

            public Handler(UserManager<AppUser> userManager, IOptions<Settings> options, IMailService mailService, ILogger<Handler> logger)
            {
                _logger = logger;
                _settings = options.Value;
                _userManager = userManager;
                _mailService = mailService;
            }
            public async Task<GenericResponse<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return GenericResponse<string>.Fail($"{request.Email} is not a registered email");

                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedRestPasswordToken = Encoding.UTF8.GetBytes(resetPasswordToken);
                var validResetPasswordToken = WebEncoders.Base64UrlEncode(encodedRestPasswordToken);

                string url = $"{_settings.ResetPasswordEmailUrl}?email={request.Email}&token={validResetPasswordToken}";

                var mailRequest = new EmailRequest()
                {
                    ToEmail = request.Email,
                    Subject = "Reset Password",
                    Body = ApplicationConstants.ResetPasswordMsg
                    .Replace("FirstName", user.FirstName)
                    .Replace("url", url),
                };

                var emailResult = await _mailService.SendMailAsync(mailRequest);
                if(!emailResult)
                    _logger.LogError($"Unable to send password reset email for user {request.Email} at {DateTime.UtcNow}.");

                return GenericResponse<string>.Success("Success", $"Check your email {request.Email} to confirm your password reset");
            }
        }
    }
}
