using MediatR;
using Microsoft.Extensions.Options;
using user_management.core.Shared;
using user_management.infrastructure.Shared;

namespace user_management.core.Queries.AppEncryption
{
    public class HandleEncription
    {
        public class Query : IRequest<GenericResponse<string>>
        {
            public string Text { get; set; }
        }
        public class Handler : IRequestHandler<Query, GenericResponse<string>>
        {
            private readonly Settings _settings;

            public Handler(IOptions<Settings> options)
            {
                _settings = options.Value;
            }
            public async Task<GenericResponse<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                if(string.IsNullOrWhiteSpace(request.Text))
                    return GenericResponse<string>.Fail("Invalid Request", 400);

                var encryptedText = EncryptionHelper.Encrypt(request.Text, _settings.CipherKeyIvPhrase);
                return GenericResponse<string>.Success(encryptedText, $"Success");
            }
        }
    }
}