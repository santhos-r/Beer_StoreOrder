using Moq;
using Xunit;
using FluentAssertions;
using AutoFixture;
using System;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service;
using Beer_StoreOrder.UnitTest.MockData;
using Northwind.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBarsController
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBarService> _serviceMock;
        private readonly BarsController _sut;
        public TestBarsController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBarService>>();
            _sut = new BarsController(_serviceMock.Object);
        }


        /// <summary>
        /// Unit Test for GetBars
        /// </summary>
        /// <returns>200 status code when data found</returns>
        [Fact]
        public async Task GetBars_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BarMock = _fixture.CreateMany<Bar>(3).ToList();
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            //Act
            var result = await _sut.GetBars();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }


        /// <summary>
        /// Unit Test for GetBars
        /// </summary>
        /// <returns>404 status code When there was no result Found</returns>
        [Fact]
        public async Task GetBars_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Bar> enumerable = new List<Bar>();
            var BarMock = enumerable;
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            //Act
            var result = await _sut.GetBars() as NotFoundResult;

            //Assert            
            Assert.Null(result);
            Assert.Equal(StatusCodes.Status404NotFound, 404);

        }

    }
}
