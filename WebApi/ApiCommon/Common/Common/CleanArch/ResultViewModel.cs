namespace Common.CleanArch
{
    public class ResultViewModel<T>
    {
        public bool IsSuccess { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public object? Data { get; private set; }

        public ResultViewModel<T> Ok(object? data = null, string? message = null)
        {
            IsSuccess = true;
            Data = data;
            Message = message ?? string.Empty;
            return this;
        }

        public ResultViewModel<T> Fail(string message)
        {
            IsSuccess = false;
            Message = message;
            Data = null;
            return this;
        }
    }

    public sealed class GenericViewModel<T> : ResultViewModel<T>
    {
    }
}
