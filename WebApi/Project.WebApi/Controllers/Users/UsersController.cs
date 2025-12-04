using Common;
using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.Create;
using Project.Application.UseCases.Users.Disable;
using Project.Application.UseCases.Users.Update;
using Project.Application.UseCases.Users.UserLogin;
using Project.Application.UseCases.Users.UserLogin.Responses;
using Project.WebApi.Controllers.Users.RequestBodys;

namespace Project.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<UsersController> _viewModel;

        public UsersController(ILogger<UsersController> logger, IMediator mediator, GenericViewModel<UsersController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost("login")]
        public async Task<IActionResult> ExecuteLogin([FromBody] LoginRequestBody requestBody)
        {
            try
            {
                var login = new UserLoginDto(requestBody.Username, requestBody.Password);
                var request = UserLoginRequest.Login(login);

                var response = await _mediator.Send(request).ConfigureAwait(false);
                return MapResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error de validación en login");
                return BadRequest(_viewModel.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestBody requestBody)
        {
            try
            {
                var dto = new CreateUserDto(requestBody.Username, requestBody.Password, requestBody.FullName);
                var request = CreateUserRequest.Create(dto);
                var response = await _mediator.Send(request).ConfigureAwait(false);
                return MapResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al crear usuario");
                return BadRequest(_viewModel.Fail(ex.Message));
            }
        }

        [HttpPatch("{userId:int}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequestBody requestBody)
        {
            try
            {
                var dto = new UpdateUserDto(userId, requestBody.Password, requestBody.FullName);
                var request = UpdateUserRequest.Create(dto);
                var response = await _mediator.Send(request).ConfigureAwait(false);
                return MapResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al actualizar usuario {UserId}", userId);
                return BadRequest(_viewModel.Fail(ex.Message));
            }
        }

        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DisableUser(int userId)
        {
            try
            {
                var request = DisableUserRequest.Create(userId);
                var response = await _mediator.Send(request).ConfigureAwait(false);
                return MapResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al deshabilitar usuario {UserId}", userId);
                return BadRequest(_viewModel.Fail(ex.Message));
            }
        }

        private IActionResult MapResponse(object response)
        {
            switch (response)
            {
                case ISuccess success:
                    return Ok(_viewModel.Ok(response, success.Message));
                case IFailure failure:
                    var status = failure.StatusCode;
                    if (status is >= 400 and < 600)
                    {
                        return StatusCode(status, _viewModel.Fail(failure.Message));
                    }

                    return BadRequest(_viewModel.Fail(failure.Message));
                case UserLoginResponse loginResponse:
                    return StatusCode(StatusCodes.Status500InternalServerError, _viewModel.Fail("Respuesta de login desconocida."));
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, _viewModel.Fail("Respuesta desconocida."));
            }
        }
    }
}
