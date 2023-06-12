using AutoFixture;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBreweryBeerService
    {

        #region "Declaration"
        private IFixture _fixture;
        protected readonly Beer_StoreOrderContext _dbContext;
        public TestBreweryBeerService()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<Beer_StoreOrderContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _dbContext = new Beer_StoreOrderContext(options);
            _dbContext.Database.EnsureCreated();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        #endregion

        #region "UnitTest for GetBreweryBeer_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBreweryBeer_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            var BreweryBeerMock = _fixture.CreateMany<Brewery>(3).ToList();
            _dbContext.Breweries.AddRange(BreweryBeerMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BreweryService(_dbContext);

            /// Act
            var result = await _sut.GetBrewery();

            /// Assert                       
            result.Should().HaveCount(BreweryBeerMock.Count);
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Brewery>>(result);
            Assert.Equal(BreweryBeerMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for AddBreweryBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBreweryBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            // Arrange
            var AllBreweryBeerMock = _fixture.CreateMany<Beer>(3).ToList();
            var BreweryBeerMock = _fixture.Create<Beer>();

            _dbContext.Beers.AddRange(AllBreweryBeerMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BreweryBeerService(_dbContext);

            /// Act
            await _sut.AddBreweryBeer(BreweryBeerMock);

            ///Assert
            int expectedRecordCount = (AllBreweryBeerMock.Count() + 1);
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
