using Common.CleanArch;
using Project.Application.UseCases.Users.UserLogin.Responses;

namespace Project.Application.UseCases.Users.UserLogin
{
    internal sealed class UserLoginHandler : IInteractor<UserLoginRequest, UserLoginResponse>
    {
        public UserLoginHandler()
        {
            
        }
        public Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
