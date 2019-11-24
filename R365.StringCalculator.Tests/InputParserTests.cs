using System;
using Xunit;
using R365.StringCalculator.Services;
using Moq;
using R365.StringCalculator.Interfaces;

namespace R365.StringCalculator.Tests
{
    public class InputParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ParseNumbers_NullOrEmptyInput_ReturnsZeros(string input)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            Assert.Equal(Tuple.Create(0, 0), result);
        }

        [Theory]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,4")]
        [InlineData("1,a,b,c")]
        public void ParseNumbers_MoreThanTwoNumbersProvided_ThrowsException(string input)
        {
            var parser = new InputParser();
            ApplicationException ex = Assert.Throws<ApplicationException>(() => parser.ParseNumbers(input));
            Assert.Equal("Maximum of 2 numbers accepted", ex.Message);
        }

        [Theory]
        [InlineData("1,2", 1, 2)]
        [InlineData("-8,-9", -8, -9)]
        [InlineData("-4,3", -4, 3)]
        [InlineData("4,-3", 4, -3)]
        public void ParseNumbers_TwoNumbersProvided_ReturnCorrectTuple(string input, int val1, int val2)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            Assert.Equal(Tuple.Create(val1, val2), result);
        }

        [Theory]
        [InlineData("1", 1, 0)]
        [InlineData("-4,", -4, 0)]
        [InlineData(",-3", 0, -3)]
        [InlineData("-8, asd9", -8, 0)]
        [InlineData("aaa,bbb", 0, 0)]
        [InlineData(", ", 0, 0)]
        public void ParseNumbers_OneOrNoneNumbersProvided_ReturnCorrectTuple(string input, int val1, int val2)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            Assert.Equal(Tuple.Create(val1, val2), result);
        }
    }
}
