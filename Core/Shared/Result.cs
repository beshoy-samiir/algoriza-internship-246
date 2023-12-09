using Core.Shared;

namespace Core.Shared
{
    public class Result
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccessful = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result<T> Success<T>(T? value) => new(value, true, Error.None);
        public static Result<T> Failure<T>(Error error) => new(false, error);
        public static Result Failure(Error error) => new(false, error);
    }
}
