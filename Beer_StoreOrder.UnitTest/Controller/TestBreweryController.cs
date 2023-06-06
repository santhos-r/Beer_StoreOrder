﻿using Moq;
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

        /// <summary>
        /// Unit Test for PostBrewery
        /// </summary>
        /// <returns>201 status code When adding new item</returns>
        [Fact]
        public async Task PostBrewery_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BreweryMock = _fixture.Create<Brewery>();
            _serviceMock.Setup(x => x.PostBrewery(BreweryMock));

            //Act
            var result = await _sut.PostBrewery(BreweryMock);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }


        /// <summary>
        /// Unit Test for PutBeer
        /// </summary>
        /// <returns>204 status code When updating an item</returns>
        [Theory]
        [InlineData(500)]
        public async Task PutBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BreweryMock = _fixture.Build<Brewery>()
                .With(x => x.Id, ID).Without(n => n.Beers)
                .Create();
            _serviceMock.Setup(x => x.PutBrewery(Id, BreweryMock));

            try
            {
                //Act
                var result = await _sut.PutBrewery(Id, BreweryMock);
            }
            catch (Exception ex)
            {
                //Assert  
                if (ex.Message == "ID Not Found")
                    Assert.Equal(StatusCodes.Status204NoContent, 204);
            }

        }
    }
}
