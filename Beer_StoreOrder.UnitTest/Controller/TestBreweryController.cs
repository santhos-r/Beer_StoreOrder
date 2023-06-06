using Moq;
using Xunit;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service;
using Beer_StoreOrder.UnitTest.MockData;
using FluentAssertions;
using AutoFixture;
using Northwind.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBreweryController
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBreweryService> _serviceMock;
        private readonly BreweryController _sut;
      
        public TestBreweryController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBreweryService>>();
            _sut = new BreweryController(_serviceMock.Object);           
        }

        /// <summary>
        /// Unit Test for GetBrewery
        /// </summary>
        /// <returns>200 status code when data found</returns>
        [Fact]
        public async Task GetBrewery_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BreweryMock = _fixture.CreateMany<Brewery>(3).ToList();
            _serviceMock.Setup(x => x.GetBrewery()).ReturnsAsync(BreweryMock);

            //Act
            var result = await _sut.GetBrewery();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Brewery>>(result);
            Assert.Equal(BreweryMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }

        /// <summary>
        /// Unit Test for GetBrewery
        /// </summary>
        /// <returns>404 status code When there was no result Found</returns>
        [Fact]
        public async Task GetBrewery_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Brewery> enumerable = new List<Brewery>();
            var BreweryMock = enumerable;
            _serviceMock.Setup(x => x.GetBrewery()).ReturnsAsync(BreweryMock);

            //Act
            var result = await _sut.GetBrewery() as NotFoundResult;

            //Assert            
            Assert.Null(result);
            Assert.Equal(StatusCodes.Status404NotFound, 404);

        }  

    }
}
