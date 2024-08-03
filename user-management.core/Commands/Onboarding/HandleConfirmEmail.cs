using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.infrastructure;

namespace user_management.core.Commands.Onboarding
{
    public class HandleConfirmEmail
    {
        public class Command : IRequest<GenericResponse<string>>
        {
            public string Email { get; set; }
            public string Token { get; set; }
        }
        public class Handler : IRequestHandler<Command, GenericResponse<string>>
        {
            public readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;

            public Handler(ApplicationContext dbContext, IMapper mapper, UserManager<AppUser> userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }
            public async Task<GenericResponse<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if(user == null)
                    return GenericResponse<string>.Fail($"User with email {request.Email} not found");

                var emailTokenDecoded = WebEncoders.Base64UrlDecode(request.Token);
                var validEmailTokenDecoded = Encoding.UTF8.GetString(emailTokenDecoded);
                var response = await _userManager.ConfirmEmailAsync(user, validEmailTokenDecoded);

                if(response.Succeeded)
                    return GenericResponse<string>.Success("Success", "Email confirmation completed");

                return GenericResponse<string>.Fail("Email confirmation was unsuccessful. Try again");
            }
        }
    }
}
