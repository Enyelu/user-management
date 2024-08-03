using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;

namespace user_management.core.Queries.Role
{
    public class HandleFetchRoles
    {
        public class Query : IRequest<GenericResponse<List<Result>>>
        {
        }

        public class Result
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public class Handler : IRequestHandler<Query, GenericResponse<List<Result>>>
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
            public async Task<GenericResponse<List<Result>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.Roles.ToListAsync();
                if (role == null || role.Count <= 0)
                    return GenericResponse <List<Result>>.Fail($"No role(s) found");

                var mappedRole = _mapper.Map<List<Result>>(role);
                return GenericResponse <List<Result>>.Success(mappedRole, $"Success");
            }
        }
    }
}