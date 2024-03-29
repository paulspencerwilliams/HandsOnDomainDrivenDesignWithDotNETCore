using System;
using System.Runtime.ExceptionServices;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        protected Price() {}
        private Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode,
            currencyLookup)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Price can not be negative", nameof(amount));
            }
        }

        internal Price(decimal amount, String currencyCode) : base(amount, new Currency {CurrencyCode = currencyCode})
        {
        }

        public new static Price FromDecimal(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) =>
            new Price(amount, currencyCode, currencyLookup);
    }
}