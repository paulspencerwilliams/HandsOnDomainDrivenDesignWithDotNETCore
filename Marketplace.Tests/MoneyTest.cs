using System.Reflection.Metadata.Ecma335;
using Marketplace.Domain;
using Xunit;

namespace Marketplace.Tests
{
    public class MoneyTest
    {
        [Fact]
        public void Money_objects_with_the_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5);
            var secondAmount = Money.FromDecimal(5);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin_1 = Money.FromDecimal(1);
            var coin_2 = Money.FromDecimal(2);
            var coin_3 = Money.FromDecimal(2);
            Money bank_note = Money.FromDecimal(5);
            Assert.Equal(bank_note, coin_1 + coin_2 + coin_3);
        }

        [Fact]
        public void Difference_of_money_gives_difference()
        {
            Assert.Equal(Money.FromDecimal(1), Money.FromDecimal(3) - Money.FromDecimal(2));
        }
    }
}