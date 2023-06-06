using Moq;
using Xunit;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Service;
using Beer_StoreOrder.UnitTest.MockData;
using FluentAssertions;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBeerController
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBeerService> _serviceMock;
        private readonly BeerController _sut;
        public TestBeerController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBeerService>>();
            _sut = new BeerController(_serviceMock.Object);
        }

        /// <summary>
        /// Unit Test for GetBeerById
        /// </summary>
        /// <returns>200 status code when data found</returns>
        [Fact]
        public async Task GetBeerById_ShouldReturn200StatusCode_WhenDataFound()
        {

            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Register<int>(() => 5);
            int Id = _fixture.Create<int>();
            var BeerMock = _fixture.Build<Beer>().With(x => x.Id, Id).Create();
            _serviceMock.Setup(x => x.GetBeerbyId(Id)).ReturnsAsync(BeerMock);

            //Act          
            var result = await _sut.GetBeerbyId(Id);

            //Assert
            Assert.NotNull(result.Value);
            Assert.Equal(BeerMock.Id, result.Value.Id);
            Assert.Equal(StatusCodes.Status200OK, 200);
        }

        /// <summary>
        /// Unit Test for GetBeerById
        /// </summary>
        /// <returns>404 status code When there was no result Found</returns>
        [Fact]
        public async Task GetBeerById_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            Beer? beer = null;
            _fixture.Register<int>(() => 99);
            int Id = _fixture.Create<int>();

            _serviceMock.Setup(x => x.GetBeerbyId(Id)).ReturnsAsync(beer);

            //Act
            var result = await _sut.GetBeerbyId(Id);

            //Assert            
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, 404);
        }
    }
}
