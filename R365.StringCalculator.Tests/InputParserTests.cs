using Xunit;
using R365.StringCalculator.Services;

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
            TestHelpers.CollectionsAreEqual(expectedNumbers, result);
        }

        [Theory]
        [InlineData(",\n", 0, 0, 0)]
        [InlineData("1\n2", 1, 2)]
        [InlineData("1\n2,3", 1, 2, 3)]
        public void ParseNumbers_UseAlternativeNewLineDelimeter_ReturnCollectionWithCorrectNumbers(string input, params int[] expectedNumbers)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            TestHelpers.CollectionsAreEqual(expectedNumbers, result);
        }

        [Theory]
        [InlineData("//#\n1#2#3", 1, 2, 3)] // special character
        [InlineData("//3\n13234", 1, 2, 4)] // numeric character
        [InlineData("//a\n1a2a3", 1, 2, 3)] // alphabet character
        [InlineData("//$\n1,2\n3$4", 1, 2, 3, 4)] // mix of formats
        public void ParseNumbers_SingleCharDelimeterSpecifiedInInputString_ReturnCollectionWithCorrectNumbers(string input, params int[] expectedNumbers)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            TestHelpers.CollectionsAreEqual(expectedNumbers, result);
        }

        [Theory]
        [InlineData("//#$\n1#$2#$3", 0,0)] // special character
        [InlineData("//34\n1342345", 0, 1342345)] // numeric character
        [InlineData("//ab\n1ab2ab3", 0,0)] // alphabet character
        [InlineData("//$1a\n1$1a2$1a3", 0,0)] // mix of formats
        public void ParseNumbers_SingleCharDelimeterSpecifiedInInputStringHasMoreThanOneCharacter_IgnoresDelimeter(string input, params int[] expectedNumbers)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            TestHelpers.CollectionsAreEqual(expectedNumbers, result);
        }

        [Theory]
        //single delimeter
        [InlineData("//[###]\n1###2###3", 1, 2, 3)] // special character
        [InlineData("//[123]\n112321233", 1, 2, 3)] // numeric character
        [InlineData("//[abc]\n1abc2abc3", 1, 2, 3)] // alphabet character
        [InlineData("//[1$a]\n11$a21$a3", 1, 2, 3)] // mix of formats
        //many delimeters
        [InlineData("//[*][aa][123][$a1]\n1*2aa31234$a15", 1, 2, 3, 4, 5)]
        public void ParseNumbers_OneOrMoreStringDelimetersSpecifiedInInputString_ReturnCollectionWithCorrectNumbers(string input, params int[] expectedNumbers)
        {
            var parser = new InputParser();
            var result = parser.ParseNumbers(input);
            TestHelpers.CollectionsAreEqual(expectedNumbers, result);
        }
    }
}
