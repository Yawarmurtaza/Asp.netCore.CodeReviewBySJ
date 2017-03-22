using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers.Tests
{
    [TestClass]
    public class LoyalBooksWebApiManagerTests
    {
        [TestMethod]
        public async Task GetIndivisualWordsCount_Should_Retunr_WordCount_By_Not_Using_Cache_At_All()
        {
            //
            // Arrange.
            //
            const string BookName = "MockBookName.txt";
            string bookText = "one's,  and's two's two, three.Three/three?three///I'm Number THREE, four four four four!Five's.";
            IEnumerable<WordOccurance> wordCount = null;

            Mock<IWebApiProcessor> mockWebApiProcessor = new Mock<IWebApiProcessor>();
            mockWebApiProcessor.Object.WebLocation = "WebServerName";
            mockWebApiProcessor.Object.ApiPath = "ResourceUrl";

            // string webPath = string.Format("{0}{1}", mockWebApiProcessor.Object.WebLocation, mockWebApiProcessor.Object.ApiPath);

            mockWebApiProcessor.Setup(x => x.GetStringAsync()).Returns(Task.FromResult(bookText));
            Mock<IMemoryCacheWrapper> mockCache = new Mock<IMemoryCacheWrapper>();
            
            mockCache.Setup(cache => cache.TryGetValue(It.IsAny<string>(), out bookText)).Returns(false);
            mockCache.Setup(cache => cache.TryGetValue(BookName, out wordCount)).Returns(false);

            ITextProcessor textProcessor = new TextProcessor();

            IWebApiManager processor = new LoyalBooksWebApiManager(mockWebApiProcessor.Object, mockCache.Object, textProcessor);

            //
            // Act.
            //
            IEnumerable<WordOccurance> result = await processor.GetIndivisualWordsCount(BookName);

            //
            // Assert.
            //

            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "FOUR").Count, 4);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "THREE").Count, 5);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "FIVE'S").Count, 1);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "TWO'S").Count, 1);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "TWO").Count, 1);

        }

        [TestMethod]
        public async Task GetIndivisualWordsCount_Should_Return_WordCount_From_The_Cache()
        {
            //
            // Arrange.
            //
            const string BookName = "MockBookName.txt";
            Mock<IWebApiProcessor> mockWebApiProcessor = new Mock<IWebApiProcessor>();
            Mock<IMemoryCacheWrapper> mockCache = new Mock<IMemoryCacheWrapper>();
            IEnumerable<WordOccurance> wordCount = this.GetWordOccurances();
            mockCache.Setup(cache => cache.TryGetValue(BookName, out wordCount)).Returns(true);

            ITextProcessor textProcessor = new TextProcessor();

            IWebApiManager processor = new LoyalBooksWebApiManager(mockWebApiProcessor.Object, mockCache.Object, textProcessor);

            //
            // Act.
            //

            IEnumerable<WordOccurance> result = await processor.GetIndivisualWordsCount(BookName);

            //
            // Assert.
            //

            Assert.AreEqual(result.Count(item => item.PrimeNumberStatus == "Unit"), 1);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber15").Count, 15);

        }

        private IEnumerable<WordOccurance> GetWordOccurances()
        {
            return new List<WordOccurance>()
            {
                new WordOccurance() {Word = "WordNumber1", Count = 1, PrimeNumberStatus = "Unit"},
                new WordOccurance() {Word = "WordNumber2", Count = 2, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber3", Count = 3, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber4", Count = 4, PrimeNumberStatus = "NO"},
                new WordOccurance() {Word = "WordNumber5", Count = 5, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber6", Count = 6, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber7", Count = 7, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber8", Count = 8, PrimeNumberStatus = "NO"},
                new WordOccurance() {Word = "WordNumber9", Count = 9, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber10", Count = 10, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber11", Count = 11, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber12", Count = 12, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber13", Count = 13, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber14", Count = 14, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber15", Count = 15, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber16", Count = 16, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber17", Count = 17, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber18", Count = 18, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber19", Count = 19, PrimeNumberStatus = "YES"},
                new WordOccurance() {Word = "WordNumber20", Count = 20, PrimeNumberStatus = "No"},
                new WordOccurance() {Word = "WordNumber21", Count = 21, PrimeNumberStatus = "No"},

            };
        }
    }
}