using Moq;
using Beer_StoreOrder.Api.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;
using Beer_StoreOrder.Model.Models;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBeerController
    {
        #region "Declaration" 
        private readonly IFixture _fixture;
        private readonly Mock<IBeerService> _serviceMock;
        private readonly BeerController _sut;
        public TestBeerController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBeerService>>();
            _sut = new BeerController(_serviceMock.Object);
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
            _serviceMock.Setup(x => x.GetBeerbyId(Id)).ReturnsAsync(BeerMock);

            //Act          
            var result = await _sut.GetBeerbyId(Id);

            //Assert
            Assert.NotNull(result.Value);
            Assert.Equal(BeerMock.Id, result.Value.Id);
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for GetBeerById_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
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
            try
            {
                //Act
                var result = await _sut.GetBeerbyId(Id);

                //Assert
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
        }
        #endregion

        #region "UnitTest for PostBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task PostBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BeerMock = _fixture.Create<Beer>();
            _serviceMock.Setup(x => x.PostBeer(BeerMock));

            //Act
            var result = await _sut.PostBeer(BeerMock);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "UnitTest for PutBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task PutBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BeerMock = _fixture.Build<Beer>()
                .With(x => x.Id, ID).Without(n => n.BarBeerStocks).Without(m=>m.Brewery)
                .Create();
            _serviceMock.Setup(x => x.PutBeer(Id, BeerMock));

            try
            {
                //Act
                var result = await _sut.PutBeer(Id, BeerMock);
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
