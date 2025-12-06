using Common;
using Common.CleanArch;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserList.Responses;

namespace Project.WebApi.Controllers.Users.Presenters
{
    public sealed class UserListPresenter : IPresenter<UserListResponse>
    {
        private readonly GenericViewModel<UsersController> _viewModel;

        public UserListPresenter(GenericViewModel<UsersController> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UserListResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is ISuccess<IReadOnlyCollection<UserDataDto>> success)
            {
                _viewModel.Ok(success.Data);
                await Task.CompletedTask;
            }
        }
    }
}
