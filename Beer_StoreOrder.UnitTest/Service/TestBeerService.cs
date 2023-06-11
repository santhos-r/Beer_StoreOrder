using AutoFixture;
using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBeerService
    {

        #region "Declaration"
        private IFixture _fixture;
        protected readonly Beer_StoreOrderContext _dbContext;
        public TestBeerService()
        {
            _fixture = new Fixture();
            var options = new DbContextOptionsBuilder<Beer_StoreOrderContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _dbContext = new Beer_StoreOrderContext(options);
            _dbContext.Database.EnsureCreated();            
        }
        #endregion


        #region "UnitTest for GetBeerById_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBeerById_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Register<int>(() => 5);
            int Id = _fixture.Create<int>();
            var BeerMock = _fixture.Build<Beer>().With(x => x.Id, Id).Create();

            _dbContext.Beers.AddRange(BeerMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BeerService(_dbContext);

            /// Act
            var result = await _sut.GetBeerbyId(Id);

            //Assert
            Assert.NotNull(result.Value);
            Assert.Equal(BeerMock.Id, result.Value.Id);
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for AddBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var AllBeerMock = _fixture.CreateMany<Beer>(3).ToList();
            var BeerMock = _fixture.Create<Beer>();

            _dbContext.Beers.AddRange(AllBeerMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BeerService(_dbContext);

            //Act
            await _sut.AddBeer(BeerMock);

            //Assert            
            int expectedRecordCount = (AllBeerMock.Count() + 1);
            _dbContext.Beers.Count().Should().Be(expectedRecordCount);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "UnitTest for UpdateBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();            
            var BeerMock = _fixture.CreateMany<Beer>(3).ToList();
            var _beerMock = _fixture.Build<Beer>()
                .With(x => x.Id, Id).Without(n => n.BarBeers).Without(m => m.Brewery)
                .Create();
            await _dbContext.SaveChangesAsync();
            var _sut = new BeerService(_dbContext);

            try
            {
                //Act
                await _sut.UpdateBeer(Id, _beerMock);
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

