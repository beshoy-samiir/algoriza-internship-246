using Core.Shared;

namespace Core.Shared
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        protected internal Result(bool isSuccess, Error error) : base(isSuccess, error) { }
        protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error) => _value = value;

        public T Value => IsSuccessful ? _value! : throw new InvalidOperationException("value of failure result can't be accessed");
    }
}
