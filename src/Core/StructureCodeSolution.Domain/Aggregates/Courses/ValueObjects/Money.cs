using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Courses.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "USD")
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(amount));

            return new Money(amount, currency);
        }

        public static Money Free() => new Money(0, "USD");

        public bool IsFree() => Amount == 0;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString() => $"{Amount} {Currency}";

        public void UpdateAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(amount));

            Amount = amount;
        }

        public void UpdateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty", nameof(currency));

            Currency = currency;
        }
    }
}
