using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.UserCreate.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class UserCreatePresenter : IPresenter<UserCreateResponse>
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public UserCreatePresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UserCreateResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is ISuccess success)
            {
                _viewModel.Ok(success);
                await Task.CompletedTask;
            }
        }
    }
}
