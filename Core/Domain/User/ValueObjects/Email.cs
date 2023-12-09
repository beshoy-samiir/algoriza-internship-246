using Core.Domain.Base;
using Core.Shared;
using static Core.Shared.Error;

namespace Core.Domain.Users.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string emailInput)
        {
            if (string.IsNullOrEmpty(emailInput))
            {
                return Result.Failure<Email>(Errors.General.IsRequiredError("Email"));
            }

            Email email = new Email(emailInput);

            return Result.Success(email);
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
