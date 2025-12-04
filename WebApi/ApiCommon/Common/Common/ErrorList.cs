namespace Common
{
    public sealed class ErrorList : List<string>
    {
        public bool IsEmpty => Count == 0;

        public Exception AsException()
        {
            var message = string.Join(" ", this);
            return new InvalidOperationException(message);
        }
    }
}
