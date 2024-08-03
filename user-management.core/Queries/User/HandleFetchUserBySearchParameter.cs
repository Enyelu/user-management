using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.DataTransferObjects;
using user_management.core.Shared;
using user_management.domain.Entities;
using user_management.infrastructure;
using user_management.infrastructure.Services.Interfaces;

namespace user_management.core.Queries.User
{
    public class HandleFetchUserBySearchParameter
    {
        public class Query : IRequest<GenericResponse<Result>>
        {
            public string SearchParameter { get; set; }
        }

        public class Result
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public string Avatar { get; set; }
            public bool IsActive { get; set; }
            public bool IsTenantStaff { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
            public AddressDto Address { get; set; }
        }

        public class Handler : IRequestHandler<Query, GenericResponse<Result>>
        {
            public readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            private readonly IMailService _mailService;
            private readonly ApplicationContext _dbContext;

            public Handler(UserManager<AppUser> userManager, IMailService mailService,
                ILogger<Handler> logger, IMapper mapper, ApplicationContext dbContext)
            {
                _mapper = mapper;
                _logger = logger;
                _dbContext = dbContext;
                _mailService = mailService;
            }
            public async Task<GenericResponse<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Attempting to fetch user with search sarameter: {request.SearchParameter} at {DateTime.UtcNow}");
                var user = await _dbContext.AppUsers
                    .Include(x => x.Address)
                    .FirstOrDefaultAsync(x => 
                    x.Id == request.SearchParameter 
                    || x.Email == request.SearchParameter 
                    || x.UserName == request.SearchParameter);

                if (user == null)
                    return GenericResponse<Result>.Fail($"User with search sarameter {request.SearchParameter} not found");

                var mappedUser = _mapper.Map<Result>(user);
                mappedUser.Address = _mapper.Map<AddressDto>(user.Address);

                _logger.LogInformation($"Retrieved user with search parameter: {request.SearchParameter} successfully");
                return GenericResponse<Result>.Success(mappedUser, "Success");
            }
        }
    }
}
