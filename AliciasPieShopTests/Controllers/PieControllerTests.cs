using AliciasPieShop.Controllers;
using AliciasPieShop.ViewModels;
using AliciasPieShopTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace AliciasPieShopTests.Controllers
{
    // there is no need for a base class when using Xunit
    // however, classes MUST be public
    public class PieControllerTests
    {

        // initialise test logging object
        private readonly ITestOutputHelper _outputHelper;
        // then constructor inject it like normal
        public PieControllerTests(ITestOutputHelper output)
        {
            _outputHelper = output;
        }

        // test method names should describe what is being tested
        // the [fact] attribute denotes that the following method is a unit test

        [Fact]
        public void List_EmptyCategory_ReturnsAllPies()
        {
            // ******ARRANGE******

            // create the necessary repositories to make a PieController
            var mockPieRepository = RepositoryMocks.GetPieRepository();                 // this is a static method which means we don't need an object instance (I think??)
            var mockCategoryRepository = RepositoryMocks.GetCategoryRepository();

            // .Object exposes the 'mocked' object instance (Moq Mock repositories have type Moq.Mock<T>; the Object property exposes an instance of object T, e.g. IPieRepository, ICategoryRepository etc)
            var pieController = new PieController(mockPieRepository.Object, mockCategoryRepository.Object);

            // once you have created the ITestOutputHelper instance through constructor injection, you can use it identically to an ILogger instance. messages will appear under 'standard output' in the test log.
            _outputHelper.WriteLine("controller created successfully at: " + DateTime.Now);

            // ******ACT******
            var result = pieController.List("");


            // ******ASSERT******

            // we know the contents of our mock repositories. we can therefore test our controller by checking the outputs of action methods against the expected output given the content of the mocks.

            // check that the controller returns an object of type ViewResult. this method also returns an instance of the object is the assertion is successful, so we can assign to a variable (viewResult)
            var viewResult = Assert.IsType<ViewResult>(result);

            // check that the view model passed to the view is of type PieListViewModel
            var pieListViewModel = Assert.IsAssignableFrom<PieListViewModel>(viewResult.ViewData.Model);

            // check that all pies are returned - there are 10 pies in our mock repository so the pieListViewModel.Pies.Count() method should return 10. (remember we are testing that List("") returns all pies
            Assert.Equal(10, pieListViewModel.Pies.Count());
        }
    }
}
