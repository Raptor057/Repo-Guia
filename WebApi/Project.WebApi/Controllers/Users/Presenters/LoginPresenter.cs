using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.UserLogin.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class LoginPresenter<T> : IPresenter<UserLoginResponse>
        where T : UserLoginResponse
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public LoginPresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }
        public async Task Handle(UserLoginResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is ISuccess response)
            {
                _viewModel.Ok(response);
                await Task.CompletedTask;
            }
        }
    }
}
