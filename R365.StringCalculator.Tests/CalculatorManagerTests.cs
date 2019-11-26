using System;
using Xunit;
using R365.StringCalculator.Services;
using Moq;
using R365.StringCalculator.Interfaces;
using R365.StringCalculator.Tests.FakeImplementations;
using R365.StringCalculator.Models;

namespace R365.StringCalculator.Tests
{
    public class CalculatorManagerTests
    {
        [Fact]
        public void Execute_CalculatorInstanceIsNull_PrintsError()
        {
            var consoleMock = new ConsoleWrapperFake();
            string expectedResultMessage = $"Error: Calculator instance was not initialized";

            var calcManager= new CalculatorManager(null, consoleMock);
            calcManager.Execute();

            Assert.Contains(expectedResultMessage, consoleMock.WriteLog);
        }

        [Fact]
        public void Execute_ConsoleInstanceIsNull_ThrowsError()
        {
            var calcStub = new Mock<ICalculator>();
            string expectedResultMessage = $"Console instance was not initialized";
            var calcManager= new CalculatorManager(calcStub.Object, null);

            var ex = Assert.Throws<ApplicationException>(() => calcManager.Execute());

            Assert.Equal(expectedResultMessage, ex.Message);
        }

        [Fact]
        public void Execute_PrintsIntroMessage()
        {
            var calcStub = new Mock<ICalculator>();
            calcStub.Setup(x => x.Calculate(It.IsAny<string>())).Returns(new CalculationResult());
            var consoleMock = new ConsoleWrapperFake();
            string expectedIntroMessage = "Enter numbers separated by commas or new line (CTRL+ENTER)."
                + Environment.NewLine
                + "When done, press ENTER.";

            var calcManager= new CalculatorManager(calcStub.Object, consoleMock);
            calcManager.Execute();

            Assert.Contains(expectedIntroMessage, consoleMock.WriteLog);
        }

        [Fact]
        public void Execute_CalculatorComputesResult_PrintsResult()
        {
            int result = 123;
            var calcStub = new Mock<ICalculator>();
            calcStub.Setup(x => x.Calculate(It.IsAny<string>())).Returns(new CalculationResult {Result= result });
            var consoleMock = new ConsoleWrapperFake();
            string expectedResultMessage = $"Result: {result}";

            var calcManager= new CalculatorManager(calcStub.Object, consoleMock);
            calcManager.Execute();

            Assert.Contains(expectedResultMessage, consoleMock.WriteLog);
        }

        [Fact]
        public void Execute_CalculatorComputesResult_PrintsFormula()
        {
            string formula = "1+2+3=6";
            var calcStub = new Mock<ICalculator>();
            calcStub.Setup(x => x.Calculate(It.IsAny<string>())).Returns(new CalculationResult { Formula= formula });
            var consoleMock = new ConsoleWrapperFake();
            string expectedResultMessage = $"Formula: {formula}";

            var calcManager = new CalculatorManager(calcStub.Object, consoleMock);
            calcManager.Execute();

            Assert.Contains(expectedResultMessage, consoleMock.WriteLog);
        }

        [Fact]
        public void Execute_CalculatorThrowsException_PrintsErrorMessage()
        {
            string errorMessage = "Test message";
            var calcStub = new Mock<ICalculator>();
            calcStub.Setup(x => x.Calculate(It.IsAny<string>())).Throws(new Exception(errorMessage));
            var consoleMock = new ConsoleWrapperFake();
            string expectedResultMessage = $"Error: {errorMessage}";

            var calcManager= new CalculatorManager(calcStub.Object, consoleMock);
            calcManager.Execute();

            Assert.Contains(expectedResultMessage, consoleMock.WriteLog);
        }

        [Fact]
        public void Execute_EnteringNewLineKey_AddsNewLineCharacterToUserInputString()
        {
            var calcMock = new Mock<ICalculator>();
            calcMock.Setup(x => x.Calculate(It.IsAny<string>())).Returns(new CalculationResult());
            var consoleStub = new ConsoleWrapperFake();
            consoleStub.KeysToRead.Clear();
            consoleStub.KeysToRead.Enqueue(TestHelpers.ConsoleKeyInfoFromChar('1'));
            consoleStub.KeysToRead.Enqueue(TestHelpers.ConsoleKeyInfoFromChar(','));
            consoleStub.KeysToRead.Enqueue(TestHelpers.ConsoleKeyInfoFromChar('2'));
            var newLineKey = new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);
            consoleStub.KeysToRead.Enqueue(newLineKey);
            consoleStub.KeysToRead.Enqueue(TestHelpers.ConsoleKeyInfoFromChar('3'));
            consoleStub.KeysToRead.Enqueue(consoleStub.EnterKey);
            string expectedInputString = "1,2\n3";

            var calcManager= new CalculatorManager(calcMock.Object, consoleStub);
            calcManager.Execute();

            calcMock.Verify(x => x.Calculate(expectedInputString), Times.Once());
        }

        [Theory]
        //empty input
        [InlineData("")]
        //single character
        [InlineData("1")]
        [InlineData("a")]
        //multiple characters
        [InlineData("123")]
        [InlineData("abcd")]
        //multiple characters with delimeters
        [InlineData("1,2")]
        [InlineData("a,2\n3")]
        public void Execute_PassesRightValueToCalculator(string input)
        {
            var calcMock = new Mock<ICalculator>();
            calcMock.Setup(x => x.Calculate(It.IsAny<string>())).Returns(new CalculationResult());
            var consoleStub = new ConsoleWrapperFake();
            consoleStub.SetupReadKeysQueue(input);

            var calcManager= new CalculatorManager(calcMock.Object, consoleStub);
            calcManager.Execute();

            calcMock.Verify(x => x.Calculate(input), Times.Once());
        }
    }
}
