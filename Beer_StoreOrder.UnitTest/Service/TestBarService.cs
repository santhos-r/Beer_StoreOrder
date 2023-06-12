using Beer_StoreOrder.Model.Data;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using Microsoft.AspNetCore.Http;

namespace Beer_StoreOrder.UnitTest.Service
{
    public class TestBarService
    {
        #region "Declaration"
        private IFixture _fixture;
        protected readonly Beer_StoreOrderContext _dbContext;
        public TestBarService()
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

        #region "Unit Test for GetBars_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBars_ShouldReturn200StatusCode_WhenDataFound()
        {
            /// Arrange
            var BarMock = _fixture.CreateMany<Bar>(3).ToList();
            _dbContext.Bars.AddRange(BarMock);
            await _dbContext.SaveChangesAsync();
            var _sut = new BarService(_dbContext);

            /// Act
            var result = await _sut.GetBars();

            /// Assert                       
            result.Should().HaveCount(BarMock.Count);
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "Unit Test for AddBar_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBar_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            /// Arrange
            var AllBarMock = _fixture.CreateMany<Bar>(3).ToList();
            var BarMock = _fixture.Create<Bar>();

            _dbContext.Bars.AddRange(AllBarMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BarService(_dbContext);

            /// Act
            await _sut.AddBar(BarMock);

            ///Assert
            int expectedRecordCount = (AllBarMock.Count() + 1);
            _dbContext.Bars.Count().Should().Be(expectedRecordCount);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "Unit Test for UpdateBar_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBar_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var AllBarMock = _fixture.CreateMany<Bar>(3).ToList();
            var BarMock = _fixture.Build<Bar>()
                .With(x => x.Id, Id).Without(n => n.BarBeers)
                .Create();

            _dbContext.Bars.AddRange(BarMock);
            await _dbContext.SaveChangesAsync();

            var _sut = new BarService(_dbContext);

            try
            {
                //Act
                await _sut.UpdateBar(Id, BarMock);
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

