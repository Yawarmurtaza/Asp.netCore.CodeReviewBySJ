using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers.Tests
{
    [TestClass]
    public class WebApiProcessorTests
    {
        [TestMethod]
        public async Task GetStringAsync_should_return_the_text()
        {
            //
            // Arrange.
            //
            const string TextToTest = "this is a text book";
            Mock<IHttpClientWrapper> mockHttpClient = new Mock<IHttpClientWrapper>();

            HttpResponseMessage mockResponse = new HttpResponseMessage();
            mockResponse.Content = new StringContent(TextToTest);
            
            IWebApiProcessor processor = new WebApiProcessor(mockHttpClient.Object);

            processor.ApiPath = "someApiPath";
            processor.WebLocation = "ServerName";
            
            string webPath = string.Format("{0}{1}", processor.WebLocation, processor.ApiPath);
            mockHttpClient.Setup(x => x.GetAsync(webPath)).Returns(Task.FromResult(mockResponse));
            
            //
            // Act.
            //

            string text = await processor.GetStringAsync();

            //
            // Assert.
            //

            Assert.AreEqual(text,TextToTest);
        }

        /// <summary>
        /// I havent completed this yet....
        /// </summary>
        /// <returns></returns>
        public async Task GetStringAsync_HandlesTheException()
        {
        }
    }
}