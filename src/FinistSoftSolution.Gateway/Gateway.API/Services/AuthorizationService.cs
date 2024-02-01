using Grpc.Net.Client;

namespace Gateway.API.Services
{
    public interface IAuthorizationService
    {
        public Task<string> SignInAsync(string phoneNumber, string password, CancellationToken cancellationToken = default);
    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly GrpcChannel _channel;
        public AuthorizationService()
        { 
            _channel = 
        }

        public Task<string> SignInAsync(string phoneNumber, string password, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
