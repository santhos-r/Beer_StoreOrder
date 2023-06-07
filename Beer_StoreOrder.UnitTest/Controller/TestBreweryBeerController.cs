using AutoFixture;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Beer_StoreOrder.Model;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBreweryBeerController
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBreweryBeerService> _serviceMock;
        private readonly BreweryBeerController _sut;

        public TestBreweryBeerController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBreweryBeerService>>();
            _sut = new BreweryBeerController(_serviceMock.Object);
        }

        /// <summary>
        /// Unit Test for GetBreweryBeer
        /// </summary>
        /// <returns>200 status code when data found</returns>
        [Fact]
        public async Task GetBreweryBeer_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BreweryBeerMock = _fixture.CreateMany<Brewery>(3).ToList();
            _serviceMock.Setup(x => x.GetBreweryBeer()).ReturnsAsync(BreweryBeerMock);

            //Act
            var result = await _sut.GetBreweryBeer();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Brewery>>(result);
            Assert.Equal(BreweryBeerMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }

        /// <summary>
        /// Unit Test for GetBreweryBeer
        /// </summary>
        /// <returns>404 status code When there was no result Found</returns>
        [Fact]
        public async Task GetBreweryBeer_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Brewery> enumerable = new List<Brewery>();
            var BreweryBeerMock = enumerable;
            _serviceMock.Setup(x => x.GetBreweryBeer()).ReturnsAsync(BreweryBeerMock);

            //Act
            var result = await _sut.GetBreweryBeer() as NotFoundResult;

            //Assert            
            Assert.Null(result);
            Assert.Equal(StatusCodes.Status404NotFound, 404);

        }


        /// <summary>
        /// Unit Test for PostBreweryBeer
        /// </summary>
        /// <returns>201 status code When adding new item</returns>
        [Fact]
        public async Task PostBreweryBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BreweryBeerMock = _fixture.Create<Beer>();
            _serviceMock.Setup(x => x.PostBreweryBeer(BreweryBeerMock));

            //Act
            var result = await _sut.PostBreweryBeer(BreweryBeerMock);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
    }
}
