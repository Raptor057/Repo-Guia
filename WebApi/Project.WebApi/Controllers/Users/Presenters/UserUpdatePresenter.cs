using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.UserUpdate.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class UserUpdatePresenter : IPresenter<UserUpdateResponse>
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public UserUpdatePresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UserUpdateResponse notification, CancellationToken cancellationToken)
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
