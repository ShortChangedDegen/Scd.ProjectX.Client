using FluentAssertions;
using Scd.ProjectX.Client.Utility;

namespace Scd.ProjectX.Client.Tests.Utility
{
    public class GuardTests
    {
        [Fact]
        public void IsTrue_ShouldThrowArgumentException_WhenProvidedParamFailsPredicate()
        {
            Func<bool, bool> providedPredicate = x => x == true;
            const bool providedParam = false;
            const string providedParamName = "ProvidedParamName";
            const string providedMessage = "Provided message for the exception";

            var exception = Assert.Throws<ArgumentException>(() => Guard.IsTrue(providedPredicate, providedParam, providedParamName, providedMessage));
            exception.Message.Should().StartWith(providedMessage);
            exception.Message.Should().Contain(providedParamName);
        }

        [Fact]
        public void IsTrue_ShouldReturnParam_WhenPredicateIsTrue()
        {
            var result = Guard.IsTrue(x => x > 0, 5, "param", "Should be greater than zero");
            result.Should().Be(5);
        }

        [Fact]
        public void Not_ShouldThrowArgumentException_WhenPredicateIsTrue()
        {
            Func<int, bool> predicate = x => x == 0;
            var ex = Assert.Throws<ArgumentException>(() => Guard.Not(predicate, 0, "param", "Should not be zero"));
            ex.Message.Should().Contain("Should not be zero");
        }

        [Fact]
        public void Not_ShouldReturnParam_WhenPredicateIsFalse()
        {
            var result = Guard.Not(x => x == 0, 1, "param", "Should not be zero");
            result.Should().Be(1);
        }

        [Fact]
        public void NotNull_ShouldThrowArgumentNullException_WhenParamIsNull()
        {
            string? param = null;
            var ex = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(param, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void NotNull_ShouldReturnParam_WhenParamIsNotNull()
        {
            var param = "value";
            var result = Guard.NotNull(param, "param");
            result.Should().Be(param);
        }

        [Fact]
        public void NotNullOrEmptyCollection_ShouldThrowArgumentException_WhenCollectionIsEmpty()
        {
            var list = new List<int>();
            var ex = Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty<ICollection<int>, int>(list, "list"));
            ex.ParamName.Should().Be("list");
        }

        [Fact]
        public void NotNullOrEmptyCollection_ShouldReturnParam_WhenCollectionIsNotEmpty()
        {
            var list = new List<int> { 1 };
            var result = Guard.NotNullOrEmpty<List<int>, int>(list, "list");
            result.Should().BeSameAs(list);
        }

        [Fact]
        public void NotNullOrEmptyString_ShouldThrowArgumentException_WhenStringIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty("", "str"));
            ex.ParamName.Should().Be("str");
        }

        [Fact]
        public void NotNullOrEmptyString_ShouldThrowArgumentNullException_WhenStringIsNull()
        {
            string? str = null;
            var ex = Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(str, "str"));
            ex.ParamName.Should().Be("str");
        }

        [Fact]
        public void NotNullOrEmptyString_ShouldReturnParam_WhenStringIsNotEmpty()
        {
            var result = Guard.NotNullOrEmpty("abc", "str");
            result.Should().Be("abc");
        }

        [Fact]
        public void IsEarlierDate_ShouldThrowArgumentException_WhenEarlyDateIsAfterLateDate()
        {
            var early = new DateTime(2024, 6, 1);
            var late = new DateTime(2024, 5, 1);
            var ex = Assert.Throws<ArgumentException>(() => Guard.IsEarlierDate(early, late, "date"));
            ex.ParamName.Should().Be("date");
        }

        [Fact]
        public void IsEarlierDate_ShouldReturnEarlyDate_WhenEarlyDateIsBeforeLateDate()
        {
            var early = new DateTime(2024, 5, 1);
            var late = new DateTime(2024, 6, 1);
            var result = Guard.IsEarlierDate(early, late, "date");
            result.Should().Be(early);
        }

        [Fact]
        public void Not_WithBadValue_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.Not(0, 0, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void Not_WithGoodValue_ShouldReturnParam()
        {
            var result = Guard.Not(0, 1, "param");
            result.Should().Be(1);
        }

        [Fact]
        public void NotIn_ShouldThrowArgumentException_WhenParamIsInBadValues()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.NotIn(new[] { 1, 2, 3 }, 2, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void NotIn_ShouldReturnParam_WhenParamIsNotInBadValues()
        {
            var result = Guard.NotIn(new[] { 1, 2, 3 }, 4, "param");
            result.Should().Be(4);
        }

        [Fact]
        public void NotDefault_ShouldThrowArgumentException_WhenParamIsDefault()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.NotDefault(0, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void NotDefault_ShouldReturnParam_WhenParamIsNotDefault()
        {
            var result = Guard.NotDefault(5, "param");
            result.Should().Be(5);
        }

        [Fact]
        public void NotNegative_ShouldThrowArgumentException_WhenParamIsNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.NotNegative(-1, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void NotNegative_ShouldReturnParam_WhenParamIsZeroOrPositive()
        {
            Guard.NotNegative(0, "param").Should().Be(0);
            Guard.NotNegative(5, "param").Should().Be(5);
        }

        [Fact]
        public void IsGreaterThan_ShouldThrowArgumentException_WhenParamIsNotGreaterThanMinValue()
        {
            var ex = Assert.Throws<ArgumentException>(() => Guard.IsGreaterThan(5, 5, "param"));
            ex.ParamName.Should().Be("param");
        }

        [Fact]
        public void IsGreaterThan_ShouldReturnParam_WhenParamIsGreaterThanMinValue()
        {
            var result = Guard.IsGreaterThan(5, 6, "param");
            result.Should().Be(6);
        }
    }
}