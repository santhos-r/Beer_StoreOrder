using AutoFixture;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBarBeerService
    {
        #region "Declaration"
        private IFixture _fixture;
        protected readonly Beer_StoreOrderContext _dbContext;
        public TestBarBeerService()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<Beer_StoreOrderContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _dbContext = new Beer_StoreOrderContext(options);
            _dbContext.Database.EnsureCreated();
        }
        #endregion

        #region "UnitTest for GetBarBeer_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBarBeer_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BarBeerMock = _fixture.CreateMany<Bar>(3).ToList();
            _dbContext.Bars.AddRange(BarBeerMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BarBeerService(_dbContext);

            //Act
            var result = await _sut.GetBarBeer();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarBeerMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for AddBarBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBarBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
         
            var AllBarBeerMock = _fixture.CreateMany<BarBeer>(3).ToList();
            var BarBeerMock = _fixture.Create<BarBeer>();

            _dbContext.BarBeers.AddRange(AllBarBeerMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BarBeerService(_dbContext);

            //Act
            await _sut.AddBarBeer(BarBeerMock);

            //Assert            
            ///Assert
            int expectedRecordCount = (AllBarBeerMock.Count() + 1);
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
