using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.Shared;
using user_management.infrastructure;

namespace user_management.core.Queries.Role
{
    public class HandleFetchRoles
    {
        public class Query : IRequest<GenericResponse<List<Result>>>
        {
            public string TenantId {  get; set; }
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
            private readonly ApplicationContext _context;

            public Handler(ApplicationContext context, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;
            }
            public async Task<GenericResponse<List<Result>>> Handle(Query request, CancellationToken cancellationToken)
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

                var roles = await query.ToListAsync(cancellationToken);

                if (roles == null || roles.Count <= 0)
                    return GenericResponse <List<Result>>.Fail($"No role(s) found");

                var mappedRole = _mapper.Map<List<Result>>(roles);
                return GenericResponse <List<Result>>.Success(mappedRole, $"Success");
            }
        }
    }
}