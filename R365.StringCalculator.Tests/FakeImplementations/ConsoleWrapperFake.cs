using R365.StringCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace R365.StringCalculator.Tests.FakeImplementations
{
    class ConsoleWrapperFake : IConsoleWrapper
    {
        public readonly ConsoleKeyInfo EnterKey = new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);

        public ConsoleWrapperFake()
        {
            SetupReadKeysQueue();
        }

        /// <summary>
        /// Queue contains Keys to be returned by ReadKey() method
        /// </summary>
        public Queue<ConsoleKeyInfo> KeysToRead = new Queue<ConsoleKeyInfo>();

        private readonly StringBuilder writeLog = new StringBuilder();
        
        /// <summary>
        /// Returns strings written to the console
        /// </summary>
        public string WriteLog => writeLog.ToString();

        /// <summary>
        /// Sets up ReadKey() method to return a sequence of characters from the provided string
        /// The last key returned is always Enter to correctly complete input process and prevent infinite loop
        /// </summary>
        /// <param name="userInput"></param>
        public void SetupReadKeysQueue(string userInput="")
        {
            this.KeysToRead.Clear();
            if (userInput != null)
            {
                foreach (char c in userInput)
                {
                    KeysToRead.Enqueue(new ConsoleKeyInfo(c, ConsoleKey.NoName, false, false, false));
                }
            }
            // The last key returned is always Enter to correctly complete input process and prevent infinite loop
            KeysToRead.Enqueue(EnterKey);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return KeysToRead.Dequeue();
        }

        public void WriteLine(string str = "")
        {
            writeLog.AppendLine(str);
        }
    }
}
