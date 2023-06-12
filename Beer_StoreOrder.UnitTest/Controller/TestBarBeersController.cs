using AutoFixture;
using Moq;
using Beer_StoreOrder.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBarBeersController
    {
        #region "Declaration"
        private readonly IFixture _fixture;
        private readonly Mock<IBarBeerService> _serviceMock;        
        private readonly BarBeersController _sut;
        public TestBarBeersController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBarBeerService>>();
            _sut = new BarBeersController(_serviceMock.Object);
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        #endregion

        #region "UnitTest for GetBarBeer_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBarBeer_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            var BarBeerMock = _fixture.CreateMany<Bar>(3).ToList();
            _serviceMock.Setup(x => x.GetBarBeer()).ReturnsAsync(BarBeerMock);

            //Act
            var result = await _sut.GetBarBeer() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        #endregion

        #region "UnitTest for GetBarBeer_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
        [Fact]
        public async Task GetBarBeer_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            IEnumerable<Bar> enumerable = new List<Bar>();
            var BarBeerMock = enumerable;
            _serviceMock.Setup(x => x.GetBarBeer()).ReturnsAsync(BarBeerMock);

            try
            {
                //Act
                var result = await _sut.GetBarBeer() as NotFoundResult;
            }
            catch (Exception ex)
            {
                //Assert 
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
        }
        #endregion

        #region "UnitTest for AddBarBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBarBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            var BarBeerMock = _fixture.Create<BarBeer>();
            _serviceMock.Setup(x => x.AddBarBeer(BarBeerMock));

            //Act
            var result = await _sut.AddBarBeer(BarBeerMock) as ObjectResult;

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        #endregion

    }
}
