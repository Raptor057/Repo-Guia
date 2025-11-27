using Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Dtos.Users;
using Project.Application.UseCases.Users.UserLogin;
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
            var Login = new UserLoginDto(requestBody.Username, requestBody.Password);

            var request = UserLoginRequest.Login(Login);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
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
