using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Task;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Task.DAL.Entities;
using Task.DAL.Interfaces;
using Task.DAL.Context;
using System.Data.Entity;
using Task.DAL.Repository;
using Task.BLL.Services;
using Task.BLL.DTO;
using Task.DAL.UnitOfWork;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Task.BLL.Interfaces;
using Task.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using Task.BLL.Nlog;
using Task.ViewModel;
using Task.MappingUI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Task.Test.Controllers
{
    [TestFixture]
    public class GameTestController // todo rename please. Convention next: YouTestClassNameTests. Here: GameControllerTests.
        // todo please apply all comments from the first test to all tests
    {
        private Mock<IGameService> _mockGameService;
        private Mock<ICommentService> _mockCommentService;
        private GameController controller;
        private Mock<ILoggingService> logger;
        private List<Game> _gameList;
        private List<Comment> _commentList;
       [SetUp]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();

            this._mockGameService = new Mock<IGameService>();
            this._mockCommentService=new Mock<ICommentService>();
            logger = new Mock<ILoggingService>();
            this.controller = new Task.Controllers.GameController(_mockGameService.Object, _mockCommentService.Object, logger.Object);

            var genres = new List<Genre>{
                new Genre{Name="fdf",GenreId=1,ParentId=0}
            };
            var ptypes = new List<PlatformType>{
                new PlatformType{Key=1,Name="sd"}
            }; // todo please beautify this code
            _gameList = new List<Game>{
                new Game{Id= 1, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)},
                new Game{Id= 2, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)}
            };
            _commentList=new List<Comment>
            {
                new Comment { }
            };
        }
        [Test]
        public void GetAllGames_ReturnList() // todo please use next name convention: MethodName_ShouldDoSomething_WhenSomeCondition (third part is optional)
        { // todo please call test properly. Name of test should answer to question what it tests. 
            //Arrange
            _mockGameService.Setup(m => m.GetAll()).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList)); // todo add extra empty lines
            //Act
            var result = ((controller.GetAllGames() as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            //Assert
            Assert.AreEqual(2, result.Count); // todo use just one Assert per one unit test
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [Test]
        public void GetAllCommentsByGame_KeyValid()
        {
             _mockCommentService.Setup(m => m.GetAllByGame(It.IsAny<string>())).Returns(Mapper.Map<ICollection<CommentDTO>>(_commentList));
            _mockGameService.Setup(m => m.ExistStringKey(It.IsAny<string>())).Returns(true);
            //Act
            var result = ((controller.GetAllCommentsByGames("GTA_5") as JsonResult).Data as IEnumerable<CommentViewModel>).ToList();
            //Assert
            Assert.AreEqual(1, result.Count);
        }
        
        [Test]
        public void AddCommentToGame_NotValidModelState_StatusCode404()
        {
            controller.ModelState.AddModelError("test", "test");
            var statusCode = controller.AddCommentToGame(It.IsAny<CommentViewModel>()) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 404);
        }

        [Test]
        public void AddCommentToGame_ValidModel_StatusCode201()
        {
            var a = new CommentViewModel();
            var statusCode = controller.AddCommentToGame(a) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 201);
        }
        [Test]
        public void AddCommentToGame_NullModel_StatusCode500()
        {
            var statusCode = controller.AddCommentToGame(It.IsAny<CommentViewModel>()) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 500);
        }

        
        [Test]
        public void AddGame_NotValidModelState_StatusCode404()
        {
            var item = new GameViewModel
            {
                Key = "New_Game",
                Description = "Some Description",
                Name = "GTA"
            };
            controller.ModelState.AddModelError("test", "test");
            var statusCode = controller.AddGame(item) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 404);
        }
        [Test]
        public void AddGame_NullModel_StatusCode500()
        {

            var statusCode = controller.AddGame(null) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 500);
        }
        [Test]
        public void AddGame_ValidModel_StatusCode201()
        {
            var a = new GameViewModel();
            var statusCode = controller.AddGame(a) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 201);
        }
        [Test]
        public void Remove_ValidModelState_StatusCode200()
        {
            var statusCode = controller.remove(1) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 201);
        }
        [Test]
        public void Remove_NotValidKey_StatusCode500()
        {
            var statusCode = controller.remove(-1) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 500);
        }
        [Test]
        public void Update_ValidModelState_StatusCode200()
        {
            var statusCode = controller.update(It.IsAny<GameViewModel>()) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 201);
        }
        [Test]
        public void Update_ValidModelState_StatusCode400()
        {
            controller.ModelState.AddModelError("test", "test");
            var statusCode = controller.update(It.IsAny<GameViewModel>()) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 404);
        }

        [Test]
        public void GetAllGamesByGenre_NotValidKey_StatusCode404()
        {
            _mockGameService.Setup(m => m.ExistEntity(It.IsAny<int>())).Returns(false);
            var statuscode = controller.GetAllGamesByGenre(It.IsAny<int>()) as HttpStatusCodeResult;
            Assert.AreEqual(statuscode.StatusCode,404);
        }

        [Test]
        public void GetAllGamesByGenre_ValidKey_ListWithGames()
        {
            _mockGameService.Setup(m => m.ExistEntity(It.IsAny<int>())).Returns(true);
            _mockGameService.Setup(m => m.GetByGenre(It.IsAny<int>()))
                .Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));

            var result = ((controller.GetAllGamesByGenre(It.IsAny<int>()) as JsonResult).Data as IEnumerable<GameViewModel>).ToList();

            _mockGameService.Verify(m=>m.GetByGenre(It.IsAny<int>()),Times.Once);
            Assert.AreEqual(result.Count, 2);
        }

        [Test]
        public void GetAllGamesByPlatformType_NotValidKey_StatusCode404()
        {
            int value = 0;
            var result=controller.GetAllGamesByPlatformType(value) as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode,404);
        }

        [Test]
        public void GetAllGamesByPlatformType_ValidKey_ListWithGames()
        {
            _mockGameService.Setup(m => m.GetAllByPlatformType(It.IsAny<int>())).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));
            var result =
            ((controller.GetAllGamesByPlatformType(1) as JsonResult).Data as
                IEnumerable<GameViewModel>).ToList();

            _mockGameService.Verify(m=>m.GetAllByPlatformType(It.IsAny<int>()),Times.Once);
            Assert.AreEqual(2,result.Count);
        }
    }
}
