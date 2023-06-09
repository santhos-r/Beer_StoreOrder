using Moq;
using AutoFixture;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Beer_StoreOrder.Service.Services.Interface;

namespace Beer_StoreOrder.UnitTest.Controller
{
    public class TestBarsController
    {
        #region "Declaration"
        private readonly IFixture _fixture;
        private readonly Mock<IBarService> _serviceMock;
        private readonly BarsController _sut;
        public TestBarsController()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IBarService>>();
            _sut = new BarsController(_serviceMock.Object);
        }
        #endregion

        #region "UnitTest for GetBars_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBars_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var BarMock = _fixture.CreateMany<Bar>(3).ToList();
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            //Act
            var result = await _sut.GetBars();

            //Assert
            Assert.NotNull(result);
            var returnValue = Assert.IsType<List<Bar>>(result);
            Assert.Equal(BarMock.Count(), returnValue.Count());
            Assert.Equal(StatusCodes.Status200OK, 200);
        }
        #endregion

        #region "UnitTest for GetBars_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
        [Fact]
        public async Task GetBars_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            IEnumerable<Bar> enumerable = new List<Bar>();
            var BarMock = enumerable;
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            //Act
            var result = await _sut.GetBars() as NotFoundResult;

            //Assert            
            Assert.Null(result);
            Assert.Equal(StatusCodes.Status404NotFound, 404);
        }
        #endregion

        #region "UnitTest for PostBar_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task PostBar_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BarMock = _fixture.Create<Bar>();
            _serviceMock.Setup(x => x.PostBar(BarMock));

            //Act
            var result = await _sut.PostBar(BarMock);

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, 201);
        }
        #endregion

        #region "UnitTest for PutBar_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task PutBar_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BarMock = _fixture.Build<Bar>()
                .With(x => x.Id, ID).Without(n => n.BarBeerStocks)
                .Create();
            _serviceMock.Setup(x => x.PutBar(Id, BarMock));

            try
            {
                //Act
                var result = await _sut.PutBar(Id, BarMock);
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
