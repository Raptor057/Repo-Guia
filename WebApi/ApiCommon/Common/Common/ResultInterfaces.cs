namespace Common
{
    public interface ISuccess
    {
        string Message { get; }
    }

    public interface IFailure
    {
        string Message { get; }
        int StatusCode => 400;
    }
}
