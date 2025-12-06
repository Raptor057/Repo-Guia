using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserGet.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class UserGetPresenter : IPresenter<UserGetResponse>
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public UserGetPresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UserGetResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is ISuccess<UserDataDto> success)
            {
                _viewModel.Ok(success.Data);
                await Task.CompletedTask;
            }
        }
    }
}
