using Common;
using Common.CleanArch;

namespace Project.Application.UseCases.Users.Common
{
    public abstract record UserCommandResponse : IResponse;

    public record SuccessUserCommandResponse(
        int UserId,
        string UserName,
        string? FullName,
        bool Active,
        string Message) : UserCommandResponse, ISuccess;

    public record FailureUserCommandResponse(string Message, int StatusCode = 400) : UserCommandResponse, IFailure
    {
        public int StatusCode { get; init; } = StatusCode;
    }
}
