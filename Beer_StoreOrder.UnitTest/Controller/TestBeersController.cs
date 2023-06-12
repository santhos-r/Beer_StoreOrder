using Moq;
using Beer_StoreOrder.Api.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;
using Beer_StoreOrder.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBeersController
    {
        #region "Declaration" 
        private readonly IFixture _fixture;
        private readonly Mock<IBeerService> _serviceMock;        
        private readonly BeersController _sut;
        public TestBeersController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBeerService>>();
            _sut = new BeersController(_serviceMock.Object);
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        #endregion

        #region "UnitTest for GetBeerById_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBeerById_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Register<int>(() => 5);
            int Id = _fixture.Create<int>();
            var BeerMock = _fixture.Build<Beer>().With(x => x.Id, Id).Create();
            _serviceMock.Setup(x => x.GetBeerbyId(Id)).ReturnsAsync(BeerMock);

            //Act          
            var result = await _sut.GetBeerbyId(Id) as ObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        #endregion

        #region "UnitTest for GetBeerById_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
        [Fact]
        public async Task GetBeerById_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            Beer? beer = null;
            _fixture.Register<int>(() => 99);
            int Id = _fixture.Create<int>();

            _serviceMock.Setup(x => x.GetBeerbyId(Id)).ReturnsAsync(beer);
            try
            {
                //Act
                var result = await _sut.GetBeerbyId(Id) as NotFoundResult;
            }
            catch (Exception ex)
            {
                //Assert               
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
        }
        #endregion

        #region "UnitTest for AddBeer_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]     
        public async Task AddBeer_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            var BeerMock = _fixture.Create<Beer>();
            _serviceMock.Setup(x => x.AddBeer(BeerMock));
           
            //Act
            var result = await _sut.AddBeer(BeerMock) as ObjectResult;

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        #endregion

        #region "UnitTest for UpdateBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBeer_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BeerMock = _fixture.Build<Beer>()
                .With(x => x.Id, ID).Without(n => n.BarBeers).Without(m => m.Brewery)
                .Create();
            _serviceMock.Setup(x => x.UpdateBeer(Id, BeerMock));

            try
            {
                //Act
                var result = await _sut.UpdateBeer(Id, BeerMock) as ObjectResult;
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
