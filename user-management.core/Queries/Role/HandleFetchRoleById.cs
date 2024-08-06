using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;
using user_management.infrastructure;

namespace user_management.core.Queries.Role
{
    public class HandleFetchRoleById
    {
        public class Query : IRequest<GenericResponse<Result>>
        {
            public string RoleId { get; set; }
            public string TenantId { get; set; }
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
            private readonly ApplicationContext _context;

            public Handler(ApplicationContext context, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;
            }
            public async Task<GenericResponse<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Roles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.TenantId))
                {
                    query = query.Where(x => x.CreatedBy == request.TenantId);
                }
                else
                {
                    query = query.Where(x => x.CreatedBy == null);
                }

                var role = await query.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

                if (role == null)
                    return GenericResponse<Result>.Fail($"RoleId {request.RoleId} is invalid");

                var mappedRole = _mapper.Map<Result>(role);
                return GenericResponse<Result>.Success(mappedRole, $"Success");
            }
        }
    }
}