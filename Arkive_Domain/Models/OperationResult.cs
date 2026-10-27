namespace Arkive_Domain.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; private set; } = 200;
        public T? Value { get; set; }

        public OperationResult() { }

        public static OperationResult<T> Success(T value, int statusCode = 200)
        {
            return new OperationResult<T> { IsSuccess = true, StatusCode = statusCode, Value = value };
        }

        public static OperationResult<T> Failure(string error, int statusCode = 400)
        {
            return new OperationResult<T> { IsSuccess = false, Error = error, StatusCode = statusCode };
        }
    }
}
