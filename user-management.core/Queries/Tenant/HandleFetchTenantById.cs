using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user_management.core.DataTransferObjects;
using user_management.core.Shared;
using user_management.infrastructure;

namespace user_management.core.Queries.Tenant
{
    public class HandleFetchTenantById
    {
        public class Query : IRequest<GenericResponse<Result>>
        {
            public string TenantId { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public List<string> LockIds { get; set; }
            public List<StaffDto> Staff { get; set; }
        }

        public class Handler : IRequestHandler<Query, GenericResponse<Result>>
        {
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            private readonly ApplicationContext _dbContext;

            public Handler(ApplicationContext dbContext, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _dbContext = dbContext;
            }
            public async Task<GenericResponse<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tenant = await _dbContext.Tenants
                    .Include(x => x.StaffMembers)
                    .FirstOrDefaultAsync(x => x.Id == request.TenantId);
                if (tenant == null)
                    return GenericResponse<Result>.Fail($"TenantId {request.TenantId} is invalid");

                var mappedRole = _mapper.Map<Result>(tenant);
                var appUserIds = tenant.StaffMembers.Select(sm => sm.AppUserId.ToString()).ToList();
                var staffMembers = await _dbContext.AppUsers
                    .Where(smId => appUserIds.Contains(smId.Id))
                    .Select(users => new StaffDto
                    {
                        Id = users.Id,
                        FirstName = users.FirstName,
                        MiddleName = users.MiddleName,
                        LastName =users.LastName,
                        Gender = users.Gender,
                        Avatar = users.Avatar,
                        IsActive = users.IsActive,
                        Email = users.Email
                    }).ToListAsync();

                mappedRole.Staff = staffMembers;
                return GenericResponse<Result>.Success(mappedRole, $"Success");
            }
        }
    }
}