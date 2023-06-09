﻿using AutoFixture;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBarBeerStocksService
    {
        #region "Declaration"
        private IFixture _fixture;
        protected readonly NorthwindContext _dbContext;
        public TestBarBeerStocksService()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<NorthwindContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _dbContext = new NorthwindContext(options);
            _dbContext.Database.EnsureCreated();
        }
        #endregion

        #region "UnitTest for GetBarBeerStockDetail_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBarBeerStockDetail_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BarBeerStockMock = _fixture.CreateMany<Bar>(3).ToList();
            _dbContext.Bars.AddRange(BarBeerStockMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BarBeerStockService(_dbContext);

            //Act
            var result = await _sut.GetBarBeerStockDetail();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarBeerStockMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for PostBarBeerStock_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task PostBarBeerStock_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

         
            var AllBarBeerStockMock = _fixture.CreateMany<BarBeerStock>(3).ToList();
            var BarBeerStockMock = _fixture.Create<BarBeerStock>();

            _dbContext.BarBeerStocks.AddRange(AllBarBeerStockMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BarBeerStockService(_dbContext);

            //Act
            await _sut.PostBarBeerStock(BarBeerStockMock);

            //Assert            
            ///Assert
            int expectedRecordCount = (AllBarBeerStockMock.Count() + 1);
            _dbContext.Breweries.Count().Should().Be(expectedRecordCount);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "Dispose"
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
        #endregion

    }
}
