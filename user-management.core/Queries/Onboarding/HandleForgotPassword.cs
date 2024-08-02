using AutoMapper;
using MediatR;
using user_management.core.Shared;
using user_management.infrastructure;

namespace user_management.core.Queries.Onboarding
{
    public class HandleForgotPassword
    {
        public class Query : IRequest<GenericResponse<Result>>
        {

        }

        public class Result
        {

        }

        public class Handler : IRequestHandler<Query, GenericResponse<Result>>
        {
            public readonly IMapper _mapper;
            private readonly ApplicationContext _dbContext;

            public Handler(ApplicationContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }
            public async Task<GenericResponse<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                return default;
            }
        }
    }
}
