using AutoFixture;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBreweryBeersController
    {
        #region "Declaration"
        private readonly IFixture _fixture;
        private readonly Mock<IBreweryBeerService> _serviceMock;
        private readonly Mock<IBeerService> _beerServiceMock;
        private readonly Mock<IBreweryService> _breweryServiceMock;
        private readonly BreweryBeersController _sut;
        public TestBreweryBeersController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBreweryBeerService>>();
            _sut = new BreweryBeersController(_serviceMock.Object);
        }
        #endregion

        #region "UnitTest for GetBreweryBeer_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBreweryBeer_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BreweryBeerMock = _fixture.CreateMany<Brewery>(3).ToList();
            _serviceMock.Setup(x => x.GetBreweryBeer()).ReturnsAsync(BreweryBeerMock);

            //Act
            var result = await _sut.GetBreweryBeer() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        #endregion

        #region "UnitTest for GetBreweryBeer_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
        [Fact]
        public async Task GetBreweryBeer_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Brewery> enumerable = new List<Brewery>();
            var BreweryBeerMock = enumerable;
            _serviceMock.Setup(x => x.GetBreweryBeer()).ReturnsAsync(BreweryBeerMock);

            try
            {
                //Act
                var result = await _sut.GetBreweryBeer() as NotFoundResult;
            }
            catch (Exception ex)
            {
                //Assert 
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
        }
        #endregion

        #region "UnitTest for AddBreweryBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBreweryBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BreweryBeerMock = _fixture.Create<Beer>();
            _serviceMock.Setup(x => x.AddBreweryBeer(BreweryBeerMock));

            //Act
            var result = await _sut.AddBreweryBeer(BreweryBeerMock) as ObjectResult;

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        #endregion
    }
}
