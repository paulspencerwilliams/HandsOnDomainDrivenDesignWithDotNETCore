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
            var firstAmount = new Money(5);
            var secondAmount = new Money(5);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin_1 = new Money(1);
            var coin_2 = new Money(2);
            var coin_3 = new Money(2);
            Money bank_note = new Money(5);
            Assert.Equal(bank_note, coin_1 + coin_2 + coin_3);
        }

        [Fact]
        public void Difference_of_money_gives_difference()
        {
            Assert.Equal(new Money(1), new Money(3) - new Money(2));
        }
    }
}