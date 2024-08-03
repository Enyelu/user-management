using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;

namespace user_management.core.Queries.Role
{
    public class HandleFetchRoleById
    {
        public class Query : IRequest<GenericResponse<Result>>
        {
            public string RoleId { get; set; }
        }

        public class Result
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public class Handler : IRequestHandler<Query, GenericResponse<Result>>
        {
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Handler(RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _roleManager = roleManager;
            }
            public async Task<GenericResponse<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == request.RoleId);
                if (role == null)
                    return GenericResponse<Result>.Fail($"RoleId {request.RoleId} is invalid");

                var mappedRole = _mapper.Map<Result>(role);
                return GenericResponse<Result>.Success(mappedRole, $"Success");
            }
        }
    }
}