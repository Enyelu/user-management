using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.core.Commands.Onboarding
{
    public class HandleResetPassword
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }
        public class Handler : IRequestHandler<Command, GenericResponse<string>>
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
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.NewPassword != request.ConfirmNewPassword)
                    return GenericResponse<string>.Fail("Password reset was unsuccessful. password and confirm-password mismatch");

                var user = await _userManager.FindByEmailAsync(request.Email);

                if(user == null)
                    return GenericResponse<string>.Fail("Password reset was unsuccessful. User not found");

                var decodeToken = WebEncoders.Base64UrlDecode(request.Token);
                var validDecodedToken = Encoding.UTF8.GetString(decodeToken);

                var tokenPurpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
                var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;
                var verifyToken = await _userManager.VerifyUserTokenAsync(user, tokenProvider, tokenPurpose, validDecodedToken);

                if(!verifyToken)
                    return GenericResponse<string>.Fail("Password reset was unsuccessful. Specified token is not valid for this user and purpose");

                var password = EncryptionHelper.Decrypt(request.NewPassword, _settings.CipherKeyIvPhrase);
                await _userManager.ResetPasswordAsync(user, request.Token, password);
                return GenericResponse<string>.Success("Success", "Password reset was successful");
            }
        }
    }
}
