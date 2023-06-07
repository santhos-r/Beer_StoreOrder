using AutoFixture;
using Moq;
using Beer_StoreOrder.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Beer_StoreOrder.Model;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBarBeerStocksController
    {

        private readonly IFixture _fixture;
        private readonly Mock<IBarBeerStockService> _serviceMock;
        private readonly BarBeerStocksController _sut;
        public TestBarBeerStocksController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBarBeerStockService>>();
            _sut = new BarBeerStocksController(_serviceMock.Object);
        }

        /// <summary>
        /// Unit Test for GetBarBeerStockDetail
        /// </summary>
        /// <returns>200 status code when data found</returns>
        [Fact]
        public async Task GetBarBeerStockDetail_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BarBeerStockMock = _fixture.CreateMany<Bar>(3).ToList();
            _serviceMock.Setup(x => x.GetBarBeerStockDetail()).ReturnsAsync(BarBeerStockMock);

            //Act
            var result = await _sut.GetBarBeerStockDetail();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarBeerStockMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }

        /// <summary>
        /// Unit Test for GetBarBeerStockDetail
        /// </summary>
        /// <returns>404 status code When there was no result Found</returns>
        [Fact]
        public async Task GetBarBeerStockDetail_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Bar> enumerable = new List<Bar>();
            var BarBeerStockMock = enumerable;
            _serviceMock.Setup(x => x.GetBarBeerStockDetail()).ReturnsAsync(BarBeerStockMock);

            //Act
            var result = await _sut.GetBarBeerStockDetail() as NotFoundResult;

            //Assert            
            Assert.Null(result);
            Assert.Equal(StatusCodes.Status404NotFound, 404);

        }

        /// <summary>
        /// Unit Test for PostBarBeerStock
        /// </summary>
        /// <returns>201 status code When adding new item</returns>
        [Fact]
        public async Task PostBarBeerStock_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BarBeerStockMock = _fixture.Create<BarBeerStock>();
            _serviceMock.Setup(x => x.PostBarBeerStock(BarBeerStockMock));

            //Act
            var result = await _sut.PostBarBeerStock(BarBeerStockMock);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
    }
}
