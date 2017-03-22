using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers.Tests
{
    [TestClass]
    public class TextProcessorTests
    {
        [TestMethod]
        public void CountWords_Should_Return_Count_Of_Each_Word_In_A_Dictionary()
        {
            //
            // Arrange.
            //

            const string Text = "one, two two, three three three, four four four four!";

            ITextProcessor processor = new TextProcessor();

            //
            // Act.
            //

            IDictionary<string, int> wordCounts = processor.CountWords(Text);

            //
            // Assert.
            //

            Assert.AreEqual(wordCounts["ONE"], 1);
            Assert.AreEqual(wordCounts["TWO"], 2);
            Assert.AreEqual(wordCounts["THREE"], 3);
            Assert.AreEqual(wordCounts["FOUR"], 4);
        }


        [TestMethod]
        public void BreakIntoChunks_should_break_a_large_string_into_7_chunks()
        {
            //
            // Arrange.
            //

            const string Text = "one, two two, three three three, four four four four!";

            ITextProcessor processor = new TextProcessor();

            //
            // Act.
            //

            IList<string> sevenChunks = processor.BreakIntoChunks(Text);

            //
            // Assert.
            //

            Assert.AreEqual(sevenChunks.Count, 7);
        }

        [TestMethod]
        public void MakeSureThatTheEndingAndItsNextStartingCharatersAreNotLettersOrNumbers()
        {
            //
            // Arrange.
            //

            const string Text = "one's,  and's two's two, three.Three/three?three///I'm Number THREE, four four four four!";

            ITextProcessor processor = new TextProcessor();


            //
            // Act.
            //
            IList<string> sevenChunks = processor.BreakIntoChunks(Text);

            //
            // Assert.
            //
            char ch0 = sevenChunks[0][sevenChunks[0].Length - 1];
            char ch1 = sevenChunks[1][0];

            Assert.AreNotEqual(char.IsLetterOrDigit(ch0), char.IsLetterOrDigit(ch1) || ch1 == '\'');

        }
    }
}
