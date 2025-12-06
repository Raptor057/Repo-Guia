using Common;
using Common.CleanArch;
using Project.Application.UseCases.Users.UserDisable.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class UserDisablePresenter : IPresenter<UserDisableResponse>
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public UserDisablePresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UserDisableResponse notification, CancellationToken cancellationToken)
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
