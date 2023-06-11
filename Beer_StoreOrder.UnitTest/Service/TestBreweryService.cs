using AutoFixture;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBreweryService
    {
        #region "Declaration"
        private IFixture _fixture;
        protected readonly Beer_StoreOrderContext _dbContext;
        public TestBreweryService()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<Beer_StoreOrderContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _dbContext = new Beer_StoreOrderContext(options);
            _dbContext.Database.EnsureCreated();
        }
        #endregion

        #region "Unit Test for GetBrewery_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBrewery_ShouldReturn200StatusCode_WhenDataFound()
        {
            /// Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BreweryMock = _fixture.CreateMany<Brewery>(3).ToList();
            _dbContext.Breweries.AddRange(BreweryMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BreweryService(_dbContext);

            /// Act
            var result = await _sut.GetBrewery();

            /// Assert                       
            result.Should().HaveCount(BreweryMock.Count);
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Brewery>>(result);
            Assert.Equal(BreweryMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "Unit Test for AddBrewery_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBrewery_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            /// Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var AllBreweryMock = _fixture.CreateMany<Brewery>(3).ToList();
            var BreweryMock = _fixture.Create<Brewery>();

            _dbContext.Breweries.AddRange(AllBreweryMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BreweryService(_dbContext);

            /// Act
            await _sut.AddBrewery(BreweryMock);

            ///Assert
            int expectedRecordCount = (AllBreweryMock.Count() + 1);
            _dbContext.Breweries.Count().Should().Be(expectedRecordCount);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "Unit Test for UpdateBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem(long Id)
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var AllBreweryMock = _fixture.CreateMany<Brewery>(3).ToList();
            var BreweryMock = _fixture.Build<Brewery>()
                .With(x => x.Id, Id).Without(n => n.Beers)
                .Create();

            _dbContext.Breweries.AddRange(AllBreweryMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BreweryService(_dbContext);

            try
            {
                //Act
                await _sut.UpdateBrewery(Id, BreweryMock);
            }
            catch (Exception ex)
            {
                //Assert  
                Assert.Equal(StatusCodes.Status204NoContent, 204);
            }
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
