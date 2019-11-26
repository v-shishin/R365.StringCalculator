using System;
using Xunit;
using R365.StringCalculator.Services;
using Moq;
using R365.StringCalculator.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R365.StringCalculator.Tests
{
    public class StringCalculatorTests
    {
        const int MAX_ALLOWED_NUMBER = 1000;

        [Fact]
        public void Calculate_InputParserNull_ThrowsArgumentNullException()
        {
            var calc = new Calculator(null);
            ApplicationException ex = Assert.Throws<ApplicationException>(() => calc.Calculate(null));

            Assert.Equal("Instance of InputParser was not provided in the constructor", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        //Maximum Allowed number
        [InlineData(MAX_ALLOWED_NUMBER)]
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
        [InlineData(3, 4)]
        [InlineData(3, 4, 5, 6, 7)]
        //Include Maximum Allowed number
        [InlineData(MAX_ALLOWED_NUMBER, 4, 5, 6, 7)]
        public void Calculate_TwoOrMoreNumbers_ReturnsCorrectSum(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);
            var calc = new Calculator(inputParserStub.Object);

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(numbers.Sum(), result.Result);
        }

        [Theory]
        //single negative number
        [InlineData(-1)]
        //several negative numbers
        [InlineData(-1,-2)]
        //mixed positive and negative numbers
        [InlineData(1, -2, 3, -4)]
        public void Calculate_SomeNumbersAreNegative_ThrowsException(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);

            string expectedMessage = "Negative numbers are not supported. Entered the following negative numbers: ";
            expectedMessage+=string.Join(", ", numbers.Where(x => x < 0));

            var calc = new Calculator(inputParserStub.Object);

            ApplicationException ex = Assert.Throws<ApplicationException>(() => calc.Calculate(null));

            Assert.Equal(expectedMessage, ex.Message);
        }

        [Theory]
        [InlineData(1001)]
        [InlineData(1, 1001)]
        [InlineData(1, 2, 1002, 3, 1003)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void Calculate_NumbersMoreThanMaxAllowed_Excluded(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);
            var calc = new Calculator(inputParserStub.Object);

            int expectedSum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] <= MAX_ALLOWED_NUMBER)
                {
                    expectedSum += numbers[i];
                }
            }

            var result = calc.Calculate(It.IsAny<string>());

            Assert.Equal(expectedSum, result.Result);
        }

        [Theory]
        [InlineData(0)] // single number
        [InlineData(1, 2, 3)] // many numbers
        [InlineData(1, MAX_ALLOWED_NUMBER + 1, 3, MAX_ALLOWED_NUMBER + 2)] // some numbers are greater than max allowed
        public void Calculate_ReturnsCorrectFormula(params int[] numbers)
        {
            var inputParserStub = new Mock<IInputParser>();
            inputParserStub.Setup(x => x.ParseNumbers(It.IsAny<string>())).Returns(numbers);
            var calc = new Calculator(inputParserStub.Object);
            
            //calculate formula
            StringBuilder formulaBuilder = new StringBuilder();
            for (int i = 0; i < numbers.Length; i++)
            {
                int num = numbers[i] <= MAX_ALLOWED_NUMBER ? numbers[i] : 0;
                formulaBuilder.Append(num);
                formulaBuilder.Append("+");
            }
            //replace last "+" with "="
            formulaBuilder[formulaBuilder.Length - 1] = '=';

            var result = calc.Calculate(It.IsAny<string>());
            
            //append calculation result to expected formula
            formulaBuilder.Append(result.Result);
            Assert.Equal(formulaBuilder.ToString(), result.Formula); ;
        }
    }
}
