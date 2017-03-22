using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;
using WordCount.Web.Controllers;
using WordCount.Web.Infrastructure;

namespace WordCount.Web.Tests.Controllers
{
    [TestClass]
    public class LoyalBooksDataControllerTests
    {
        /// <summary>
        /// Because our dummy words collection has only 21 items, page 3 should just return 1 record!
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task NextTenWords_Should_Return_Only_1_Word_When_Page_3_Is_Requested()
        {

            string bookName = "DummyBookName";

            Mock<ISessionWrapper> mockSession = new Mock<ISessionWrapper>();

            mockSession.Setup(x => x.GetString("bookName")).Returns(bookName);


            Mock<IServiceProviderWrapper> mockServices = new Mock<IServiceProviderWrapper>();

            mockServices.Setup(svc => svc.GetRequiredService()).Returns(mockSession.Object);

            IEnumerable<WordOccurance> wordOccurances = this.GetWordOccurances();

            Mock<IWebApiManager> mockManager = new Mock<IWebApiManager>();

            mockManager.Setup(x => x.GetIndivisualWordsCount(bookName)).Returns(Task.FromResult(wordOccurances));

            Mock<IDependencyResolver> mockDepResolver = new Mock<IDependencyResolver>();

            mockDepResolver.Setup(x => x.GetWebApiManagerByName(null)).Returns(mockManager.Object);

            LoyalBooksDataController controller = new LoyalBooksDataController(mockDepResolver.Object, mockServices.Object);

            //
            // Act.
            //

            IEnumerable<WordOccurance> result = await controller.NextTenWords(3);

            //
            // Assert.
            //

            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber21").Count, 21);

            Assert.IsNull(result.FirstOrDefault(item => item.Word == "WordNumber20"));
            Assert.IsNull(result.FirstOrDefault(item => item.Word == "WordNumber1"));
        }


        [TestMethod]
        public async Task NextTenWords_SHould_Return_Last_10_Words_When_WordsToSkip_Goes_Beyond_The_Total_Number_Of_Words_In_The_Collection()
        {
            string bookName = "DummyBookName";

            Mock<ISessionWrapper> mockSession = new Mock<ISessionWrapper>();

            mockSession.Setup(x => x.GetString("bookName")).Returns(bookName);


            Mock<IServiceProviderWrapper> mockServices = new Mock<IServiceProviderWrapper>();

            mockServices.Setup(svc => svc.GetRequiredService()).Returns(mockSession.Object);

            IEnumerable<WordOccurance> wordOccurances = this.GetWordOccurances();

            Mock<IWebApiManager> mockManager = new Mock<IWebApiManager>();

            mockManager.Setup(x => x.GetIndivisualWordsCount(bookName)).Returns(Task.FromResult(wordOccurances));

            Mock<IDependencyResolver> mockDepResolver = new Mock<IDependencyResolver>();

            mockDepResolver.Setup(x => x.GetWebApiManagerByName(null)).Returns(mockManager.Object);

            LoyalBooksDataController controller = new LoyalBooksDataController(mockDepResolver.Object, mockServices.Object);

            //
            // Act.
            //

            IEnumerable<WordOccurance> result = await controller.NextTenWords(4);

            //
            // Assert.
            //

            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber12").Count, 12);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber13").Count, 13);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber14").Count, 14);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber15").Count, 15);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber16").Count, 16);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber17").Count, 17);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber18").Count, 18);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber19").Count, 19);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber20").Count, 20);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber21").Count, 21);

            Assert.IsNull(result.FirstOrDefault(item => item.Word == "WordNumber1"));
        }

        [TestMethod]
        public async Task NextTenWordsTest_SHould_Return_First_10_Words_When_No_Page_Number_Is_Given()
        {
            string bookName = "DummyBookName";

            Mock<ISessionWrapper> mockSession = new Mock<ISessionWrapper>();

            mockSession.Setup(x => x.GetString("bookName")).Returns(bookName);

            
            Mock<IServiceProviderWrapper> mockServices = new Mock<IServiceProviderWrapper>();

            mockServices.Setup(svc => svc.GetRequiredService()).Returns(mockSession.Object);

            IEnumerable<WordOccurance> wordOccurances = this.GetWordOccurances();

            Mock<IWebApiManager> mockManager = new Mock<IWebApiManager>();
            
            mockManager.Setup(x => x.GetIndivisualWordsCount(bookName)).Returns(Task.FromResult(wordOccurances));

            Mock<IDependencyResolver> mockDepResolver = new Mock<IDependencyResolver>();

            mockDepResolver.Setup(x => x.GetWebApiManagerByName(null)).Returns(mockManager.Object);

            LoyalBooksDataController controller = new LoyalBooksDataController(mockDepResolver.Object, mockServices.Object);

            //
            // Act.
            //

            IEnumerable<WordOccurance> result = await controller.NextTenWords();

            //
            // Assert.
            //

            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber1").Count, 1);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber2").Count, 2);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber3").Count, 3);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber4").Count, 4);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber5").Count, 5);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber6").Count, 6);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber7").Count, 7);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber8").Count, 8);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber9").Count, 9);
            Assert.AreEqual(result.FirstOrDefault(item => item.Word == "WordNumber10").Count, 10);

            Assert.IsNull(result.FirstOrDefault(item => item.Word == "WordNumber11"));
        }

        private IEnumerable<WordOccurance> GetWordOccurances()
        {
            return new List<WordOccurance>()
            {
                new WordOccurance(){Word = "WordNumber1", Count = 1, PrimeNumberStatus = "Unit"},
                new WordOccurance(){Word = "WordNumber2", Count = 2, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber3", Count = 3, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber4", Count = 4, PrimeNumberStatus = "NO"},
                new WordOccurance(){Word = "WordNumber5", Count = 5, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber6", Count = 6, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber7", Count = 7, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber8", Count = 8, PrimeNumberStatus = "NO"},
                new WordOccurance(){Word = "WordNumber9", Count = 9, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber10", Count = 10, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber11", Count = 11, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber12", Count = 12, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber13", Count = 13, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber14", Count = 14, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber15", Count = 15, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber16", Count = 16, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber17", Count = 17, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber18", Count = 18, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber19", Count = 19, PrimeNumberStatus = "YES"},
                new WordOccurance(){Word = "WordNumber20", Count = 20, PrimeNumberStatus = "No"},
                new WordOccurance(){Word = "WordNumber21", Count = 21, PrimeNumberStatus = "No"},

            };
        }
    }
}
