using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserCreate;
using Project.Application.UseCases.Users.UserDisable;
using Project.Application.UseCases.Users.UserGet;
using Project.Application.UseCases.Users.UserList;
using Project.Application.UseCases.Users.UserLogin;
using Project.Application.UseCases.Users.UserUpdate;
using Project.WebApi.Controllers.Users.RequestBodys;
using Project.WebApi.Security;

namespace Project.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<UsersController> _viewModel;
        private readonly JwtTokenService _jwtTokenService;

        public UsersController(
            ILogger<UsersController> logger,
            IMediator mediator,
            GenericViewModel<UsersController> viewModel,
            JwtTokenService jwtTokenService)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestBody requestBody)
        {
            var login = new UserLoginDto(requestBody.Username, requestBody.Password);
            var request = UserLoginRequest.Login(login);

            try
            {
                var response = await _mediator.Send(request).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserLogin.Responses.SuccessUserLoginResponse success => Ok(_viewModel.Ok(new
                    {
                        token = _jwtTokenService.GenerateToken(success.UserId, success.UserName, success.RoleName),
                        userId = success.UserId,
                        userName = success.UserName,
                        fullName = success.FullName,
                        roleId = success.RoleId,
                        roleName = success.RoleName
                    })),
                    Project.Application.UseCases.Users.UserLogin.Responses.FailureUserLoginResponse failure => Unauthorized(_viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestBody requestBody, CancellationToken cancellationToken)
        {
            var dto = new UserCreateDto(
                requestBody.FullName,
                requestBody.Username,
                requestBody.Password,
                requestBody.RoleId);

            try
            {
                var request = UserCreateRequest.Create(dto);

                var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserCreate.Responses.SuccessUserCreateResponse success => Ok(_viewModel.Ok(success)),
                    Project.Application.UseCases.Users.UserCreate.Responses.FailureUserCreateResponse failure => StatusCode(500, _viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{userId:long}")]
        public async Task<IActionResult> UpdateUser(long userId, [FromBody] UserUpdateRequestBody requestBody, CancellationToken cancellationToken)
        {
            var dto = new UserUpdateDto(
                userId,
                requestBody.FullName,
                requestBody.Username,
                requestBody.Password,
                requestBody.RoleId);

            try
            {
                var request = UserUpdateRequest.Update(dto);

                var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserUpdate.Responses.SuccessUserUpdateResponse success => Ok(_viewModel.Ok(success)),
                    Project.Application.UseCases.Users.UserUpdate.Responses.FailureUserUpdateResponse failure => StatusCode(500, _viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("disable")]
        public async Task<IActionResult> DisableUser([FromBody] UserDisableRequestBody requestBody, CancellationToken cancellationToken)
        {
            var dto = new UserDisableDto(requestBody.UserId, requestBody.IsActive);
            try
            {
                var request = UserDisableRequest.Disable(dto);

                var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserDisable.Responses.SuccessUserDisableResponse success => Ok(_viewModel.Ok(success)),
                    Project.Application.UseCases.Users.UserDisable.Responses.FailureUserDisableResponse failure => StatusCode(500, _viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId:long}")]
        public async Task<IActionResult> GetUser(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var request = UserGetRequest.Create(userId);

                var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserGet.Responses.SuccessUserGetResponse success => Ok(_viewModel.Ok(success.Data)),
                    Project.Application.UseCases.Users.UserGet.Responses.FailureUserGetResponse failure => StatusCode(404, _viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                var request = UserListRequest.Create();

                var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return response switch
                {
                    Project.Application.UseCases.Users.UserList.Responses.SuccessUserListResponse success => Ok(_viewModel.Ok(success.Data)),
                    Project.Application.UseCases.Users.UserList.Responses.FailureUserListResponse failure => StatusCode(500, _viewModel.Fail(failure.Message)),
                    _ => StatusCode(500, _viewModel.Fail("Ocurrió un error inesperado."))
                };
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}
