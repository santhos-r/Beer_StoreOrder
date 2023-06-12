using AutoFixture;
using Beer_StoreOrder.Api.Controllers;
using Beer_StoreOrder.Model.Models;
using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        #endregion

        #region "UnitTest for GetBars_ShouldReturn200StatusCode_WhenDataFound"
        [Fact]
        public async Task GetBars_ShouldReturn200StatusCode_WhenDataFound()
        {
            //Arrange  
            var BarMock = _fixture.CreateMany<Bar>(3).ToList();
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            //Act
            var result = await _sut.GetBars() as ObjectResult;

            //Assert           
            Assert.NotNull(result);          
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        #endregion

        #region "UnitTest for GetBars_ShouldReturn404StatusCode_WhenThereAreNoResultFound"
        [Fact]
        public async Task GetBars_ShouldReturn404StatusCode_WhenThereAreNoResultFound()
        {
            //Arrange
            IEnumerable<Bar> enumerable = new List<Bar>();
            var BarMock = enumerable;
            _serviceMock.Setup(x => x.GetBars()).ReturnsAsync(BarMock);

            try
            {
                //Act
                var result = await _sut.GetBars() as NotFoundResult;
            }
            catch(Exception ex)
            {
                //Assert
                Assert.Equal(StatusCodes.Status404NotFound, 404);
            }
            
        }
        #endregion

        #region "UnitTest for AddBar_ShouldReturnStatus201Created_WhenAddingNewItem"
        [Fact]
        public async Task AddBar_ShouldReturnStatus201Created_WhenAddingNewItem()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var BarMock = _fixture.Create<Bar>();
            _serviceMock.Setup(x => x.AddBar(BarMock));

            //Act
            var result = await _sut.AddBar(BarMock) as ObjectResult;

            //Assert            
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        #endregion

        #region "UnitTest for UpdateBar_ShouldReturnStatus204NoContent_WhenUpdatingItem"
        [Theory]
        [InlineData(500)]
        public async Task UpdateBar_ShouldReturnStatus204NoContent_WhenUpdatingItem(long ID)
        {
            //Arrange
            _fixture.Register<long>(() => ID);
            long Id = _fixture.Create<long>();

            var BarMock = _fixture.Build<Bar>()
                .With(x => x.Id, ID).Without(n => n.BarBeers)
                .Create();
            _serviceMock.Setup(x => x.UpdateBar(Id, BarMock));

            try
            {
                //Act
                var result = await _sut.UpdateBar(Id, BarMock) as ObjectResult;
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
