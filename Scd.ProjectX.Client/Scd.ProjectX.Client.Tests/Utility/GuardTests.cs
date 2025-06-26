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
        public void IsTrue_ShouldReturnProvidedParam_WhenProvidedParamPassesPredicate()
        {
            Func<bool, bool> providedPredicate = x => x == true;
            const bool providedParam = true;
            const string providedParamName = "ProvidedParamName";
            const string providedMessage = "Provided message for the exception";

            Guard.IsTrue(providedPredicate, providedParam, providedParamName, providedMessage)
                .Should().Be(providedParam);
        }
    }
}