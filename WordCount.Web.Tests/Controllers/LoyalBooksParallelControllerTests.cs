using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WordCount.Web.Controllers;
using WordCount.Web.Infrastructure;
using WordCount.Web.ViewModels;

namespace WordCount.Web.Tests.Controllers
{
    [TestClass]
    public class LoyalBooksParallelControllerTests
    {
        [TestMethod]
        public void ShowBookContent_Should_Return_A_View_With_LoyalBooksTextViewModel_And_OperationName_Set()
        {
            //
            // Arrange.
            //
            const string BookName = "SomeDummyBookName";
            Mock<IServiceProviderWrapper> mockServices = new Mock<IServiceProviderWrapper>();

            Mock<ISessionWrapper> mockSession = new Mock<ISessionWrapper>();
            mockSession.Setup(session => session.SetString("bookName", BookName));

            mockServices.Setup(svc => svc.GetRequiredService()).Returns(mockSession.Object);

            LoyalBooksParallelController controller = new LoyalBooksParallelController(mockServices.Object);

            //
            // Act.
            //

            ActionResult aResult = controller.ShowBookContent(BookName);

            //
            // Assert.
            //

            Assert.IsNotNull(aResult);
            ViewResult vResult = aResult as ViewResult;
            Assert.IsNotNull(vResult);
            LoyalBooksTextViewModel model = vResult.Model as LoyalBooksTextViewModel;
            Assert.AreEqual(model.BookName, BookName);
            Assert.AreEqual(model.OperationName, "| Parallel");

        }
    }
}