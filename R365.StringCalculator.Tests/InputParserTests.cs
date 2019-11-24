using System;
using Xunit;
using R365.StringCalculator.Services;
using System.Linq;

namespace R365.StringCalculator.Tests
{
    public class InputParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ParseNumbers_NullOrEmptyInput_ReturnsEmptyCollection(string input)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        // one numbers
        [InlineData("1", 1)]
        // two numbers
        [InlineData(",", 0, 0)]
        [InlineData("1,2", 1, 2)]
        [InlineData("-8,-9", -8, -9)]
        [InlineData("4,-3", 4, -3)]
        //some numbers incorect
        [InlineData("aaa", 0)]
        [InlineData("a,b,c", 0,0,0)]
        [InlineData("a,1,c", 0, 1, 0)]
        public void ParseNumbers_OneOrMoreNumbersProvided_ReturnCollectionWithCorrectNumbers(string input, params int[] expectedNumbers)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            Assert.Equal(expectedNumbers.Length, result.Count());
            Assert.All(result, x => expectedNumbers.Contains(x));
        }

    }
}
