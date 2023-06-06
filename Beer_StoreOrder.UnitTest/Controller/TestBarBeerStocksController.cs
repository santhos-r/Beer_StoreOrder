using AutoFixture;
using Moq;
using Xunit;
using System;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Beer_StoreOrder.Service.Services;
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

    }
}
