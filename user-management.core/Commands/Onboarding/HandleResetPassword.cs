using AutoMapper;
using MediatR;
using user_management.core.Shared;
using user_management.infrastructure;

namespace user_management.core.Commands.Onboarding
{
    public class HandleResetPassword
    {
        public class Command : IRequest<GenericResponse<Result>>
        {

        }

        public class Result
        {

        }

        public class Handler : IRequestHandler<Command, GenericResponse<Result>>
        {
            public readonly IMapper _mapper;
            private readonly ApplicationContext _dbContext;

            public Handler(ApplicationContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }
            public async Task<GenericResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                return default;
            }
        }
    }
}
