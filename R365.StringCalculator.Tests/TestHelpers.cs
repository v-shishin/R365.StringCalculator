using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace R365.StringCalculator.Tests
{
    static class TestHelpers
    {
        /// <summary>
        /// Checks if two collections are identical using default equality comparer for elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedCollection"></param>
        /// <param name="actualCollection"></param>
        public static void CollectionsAreEqual<T>(IEnumerable<T> expectedCollection, IEnumerable<T> actualCollection)
        {
            Assert.Equal(expectedCollection?.Count(), actualCollection?.Count());
            Assert.All(expectedCollection, x => actualCollection.Contains(x));
        }

        /// <summary>
        /// Creates ConsoleKeyInfo object with the character provided
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ConsoleKeyInfo ConsoleKeyInfoFromChar(char c)
        {
            return new ConsoleKeyInfo(c, ConsoleKey.NoName, false, false, false);
        }
    }
}
