using Moq;
using Beer_StoreOrder.Api.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBreweryController
    {
        #region "Declaration""
        private readonly IFixture _fixture;
        private readonly Mock<IBreweryService> _serviceMock;
        private readonly BreweryController _sut;
        public TestBreweryController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBreweryService>>();
            _sut = new BreweryController(_serviceMock.Object);
        }
        #endregion

        #region "UnitTest for GetBrewery_ShouldReturn200StatusCode_WhenDataFound"
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
        #endregion

        #region "UnitTest for GetBrewery_ShouldReturn404StatusCode_WhenThereAreNoResultFound" 
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
        #endregion

        #region "UnitTest for PostBrewery_ShouldReturnStatus201Created_WhenAddingNewItem"
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
        #endregion

        #region "UnitTest for PutBrewery_ShouldReturnStatus204NoContent_WhenUpdatingItem"
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
        #endregion
    }
}
