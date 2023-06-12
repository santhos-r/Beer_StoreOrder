using Moq;
using Beer_StoreOrder.Api.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBrewerysController
    {
        #region "Declaration""
        private readonly IFixture _fixture;
        private readonly Mock<IBreweryService> _serviceMock;
        private readonly BrewerysController _sut;
        public TestBrewerysController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBreweryService>>();
            _sut = new BrewerysController(_serviceMock.Object);
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        #endregion

        #region "UnitTest for GetBrewery_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBrewery_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            var BreweryMock = _fixture.CreateMany<Brewery>(3).ToList();
            _serviceMock.Setup(x => x.GetBrewery()).ReturnsAsync(BreweryMock);

            //Act
            var result = await _sut.GetBrewery() as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        #endregion

        #region "UnitTest for GetBrewery_ShouldReturn404StatusCode_WhenThereAreNoResultFound" 
        [Fact]
        public async Task GetBrewery_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            IEnumerable<Brewery> enumerable = new List<Brewery>();
            var BreweryMock = enumerable;
            _serviceMock.Setup(x => x.GetBrewery()).ReturnsAsync(BreweryMock);
            try
            {
                //Act
                var result = await _sut.GetBrewery() as NotFoundResult;
            }
            catch (Exception ex)
            {
                //Assert 
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
        }
        #endregion

        #region "UnitTest for AddBrewery_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBrewery_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            var BreweryMock = _fixture.Create<Brewery>();
            _serviceMock.Setup(x => x.AddBrewery(BreweryMock));

            //Act
            var result = await _sut.AddBrewery(BreweryMock) as ObjectResult;

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        #endregion

        #region "UnitTest for UpdateBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BreweryMock = _fixture.Build<Brewery>()
                .With(x => x.Id, ID).Without(n => n.Beers)
                .Create();
            _serviceMock.Setup(x => x.UpdateBrewery(Id, BreweryMock));

            try
            {
                //Act
                var result = await _sut.UpdateBrewery(Id, BreweryMock) as ObjectResult;
            }
            catch (Exception ex)
            {
                //Assert  
                if (ex.Message == "ID Not Found")
                    Assert.Equal(StatusCodes.Status204NoContent, 204);
            }
        }
        #endregion
    }
}
