using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers.Tests
{
    [TestClass]
    public class LoyalBooksWebApiParallelManagerTests
    {
        private Mock<IMemoryCacheWrapper> mockMemory;
        private Mock<ITextProcessor> mockTextProcessor;

        [TestInitialize]
        public void Initialise()
        {
            this.mockMemory = new Mock<IMemoryCacheWrapper>();
            mockTextProcessor = new Mock<ITextProcessor>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.mockMemory = null;

            this.mockTextProcessor = null;
        }

        [TestMethod]
        public async Task GetIndivisualWordCount_Should_Return_Word_And_Count_For_Given_BookName()
        {
            //
            // Arrange.
            //

            Dictionary<string, int> wc = new Dictionary<string, int>();
            wc.Add("Mine", 5);
            wc.Add("Yours", 4);

            IEnumerable<WordOccurance> wordCount = this.GetWordOccurances();
            ITextProcessor textProcessor = new TextProcessor();
            
            string BookText = "one's,  and's two's two, three.Three/three?three///I'm Number THREE, four four four four!Five's.";
            const string BookName = "BookName";
            Mock<IWebApiProcessor> mockProcessor = new Mock<IWebApiProcessor>();

            mockProcessor.Object.ApiPath = "TargetResource/";
            mockProcessor.Object.ApiPath += BookName;
            
            // mockTextProcessor.Setup(tp => tp.BreakIntoChunks(BookText)).Returns(textProcessor.BreakIntoChunks(BookText));

            // mockTextProcessor.Setup(tp => tp.CountWords(It.IsAny<string>())).Returns(wc);
            

            mockMemory.Setup(m => m.TryGetValue(BookName, out wordCount)).Returns(false);

            mockMemory.Setup(m => m.TryGetValue(mockProcessor.Object.ApiPath, out BookText)).Returns(true);


            IWebApiManager manager = new LoyalBooksWebApiParallelManager(mockProcessor.Object, mockMemory.Object,
                textProcessor);

            //
            // Act.
            //

            List<WordOccurance> result = (await manager.GetIndivisualWordsCount(BookName)).ToList();

            //
            // Assert.
            //

            /*
            Assert.AreEqual(result[0].Word, "TWO'S");
            Assert.AreEqual(result[0].Count, 1);

            Assert.AreEqual(result[2].Word, "THREE");
            Assert.AreEqual(result[2].Count, 5);*/
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "THREE").Count, 5);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "TWO'S").Count, 1);
        }


        [TestMethod]
        public async Task GetIndivisualWordCount_Should_Return_Word_And_Count_For_Given_BookName_From_Cache()
        {
            //
            // Arrange.
            //

            IEnumerable<WordOccurance> wordCount = this.GetWordOccurances();

            const string BookName = "BookName";
            Mock<IWebApiProcessor> mockProcessor = new Mock<IWebApiProcessor>();

            mockProcessor.Object.ApiPath = "TargetResource/";
            mockProcessor.Object.ApiPath += BookName;

            
            
            string textOfTheBook = "Contents of the book";

            this.mockMemory.Setup(m => m.TryGetValue(BookName, out wordCount)).Returns(true);


            IWebApiManager manager = new LoyalBooksWebApiParallelManager(mockProcessor.Object, mockMemory.Object, mockTextProcessor.Object);




            //
            // Act.
            //

            IEnumerable<WordOccurance> result = await manager.GetIndivisualWordsCount(BookName);

            //
            // Assert.
            //


            Assert.AreEqual(result.Count(), wordCount.Count());

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