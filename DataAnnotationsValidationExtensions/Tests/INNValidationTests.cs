using Validator = DataAnnotationsValidationExtensions.Helper.Validator;

namespace Tests
{
    public class INNValidationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("7830002293")]
        [TestCase("500100732259")]
        [TestCase("325507450247")]
        public void Validate_WhenValidINNIsGood_ShouldReturnTrue(string inn)
        {
            var result =Validator.IsValidINN(inn);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("7713456564")]
        [TestCase("500100732252")]
        [TestCase("7727563777")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void Validate_WhenValidINNIsFail_ShouldReturnFalse(string inn)
        {
            var result = Validator.IsValidINN(inn);
            Assert.IsFalse(result);
        }
    }
}