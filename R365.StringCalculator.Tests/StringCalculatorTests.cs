using System;
using Xunit;
using R365.StringCalculator.Services;
using Moq;
using R365.StringCalculator.Interfaces;

namespace R365.StringCalculator.Tests
{
    public class StringCalculatorTests
    {
        [Fact]
        public void Calculate_InputParserNull_ThrowsArgumentNullException()
        {
            var calc = new Calculator(null);
            ApplicationException ex = Assert.Throws<ApplicationException>(() => calc.Calculate(null));

            Assert.Equal("Instance of InputParser was not provided in the constructor", ex.Message);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        [InlineData(20)]
        public void Calculate_SingleNumber_ReturnsSameNumber(int val)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(Tuple.Create(val, 0));
            var calc = new Calculator(inputParserStub.Object);

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(val, result.Result);
        }

        [Theory]
        [InlineData(int.MinValue, int.MinValue)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(-3, -4)]
        [InlineData(-3, 4)]
        [InlineData(3, -4)]
        [InlineData(3, 4)]
        [InlineData(0,0)]
        [InlineData(1, 5000)]
        public void Calculate_TwoNumbers_ReturnsCorrectSum(int val1, int val2)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(Tuple.Create(val1, val2));
            var calc = new Calculator(inputParserStub.Object);

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(val1+val2, result.Result);
        }

    }
}
