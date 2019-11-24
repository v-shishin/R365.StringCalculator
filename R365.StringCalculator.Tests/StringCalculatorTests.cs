using System;
using Xunit;
using R365.StringCalculator.Services;
using Moq;
using R365.StringCalculator.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(new List<int> { val });
            var calc = new Calculator(inputParserStub.Object);

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(val, result.Result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-3, -4)]
        [InlineData(-3, 4)]
        [InlineData(3, -4)]
        [InlineData(3, 4)]
        [InlineData(3, 4, 5, 6, 7)]
        public void Calculate_TwoOrMoreNumbers_ReturnsCorrectSum(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);
            var calc = new Calculator(inputParserStub.Object);

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(numbers.Sum(), result.Result);
        }


        [Theory]
        [InlineData(int.MinValue, -1)]
        [InlineData(int.MaxValue, 1)]
        public void Calculate_LargeOrSmallNumbers_ThrowsOverflowException(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);
            var calc = new Calculator(inputParserStub.Object);

            OverflowException ex = Assert.Throws<OverflowException>(() => calc.Calculate(null));
        }

    }
}
