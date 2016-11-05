using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using GameStore.DAL.Entities;
using GameStore.BLL.DTO;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using GameStore.BLL.Nlog;
using GameStore.DAL.Interfaces;
using GameStore.WEB.ViewModel;
using GameStore.WEB.Mapping;

namespace GameStore.Test.Controllers
{
    [TestFixture]
    public class GameControllerTests
    {
        private Mock<IGameService> _mockGameService;

        private Mock<ICommentService> _mockCommentService;

        private GameController _gameController;

        private Mock<ILoggingService> _logger;

        private Mock<IWriter> _writer;

        private List<Game> _gameList;

        private Mock<IUnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();
            _mockGameService = new Mock<IGameService>();
            _mockCommentService = new Mock<ICommentService>();
            _logger = new Mock<ILoggingService>();
            _writer = new Mock<IWriter>();
            _gameController = new GameController(_mockGameService.Object, _mockCommentService.Object, _logger.Object, _writer.Object);
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            var genres = new List<Genre>{
                new Genre{Name="fdf",Id=1,ParentId=0}
            };

            var ptypes = new List<PlatformType>{
                new PlatformType{Id=1,Name="sd"}
            };

            _gameList = new List<Game>{
                new Game{Id= 1, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)},
                new Game{Id= 2, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)}
            };
        }

        [Test]
        public void GetAllGames_GetListOfGamesFromDB()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetAll()).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList)); 
            //Act
            var result = ((_gameController.GetAllGames() as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            //Assert
            Assert.AreEqual(2, result.Count);
        }


        [Test]
        public void AddGame_ReturnHttpStatusCode406_GameViewModelConsistModelStateError()
        {
            //Arrange
            var item = new GameViewModel
            {
                Key = "New_Game",
                Description = "Some Description",
                Name = "GTA"
            };
            _gameController.ModelState.AddModelError("test", "test");
            //Act
            var statusCode = _gameController.New(item) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 406);
        }

        [Test]
        public void AddGame_ReturnHttpStatusCode400_GameViewModelIsNull()
        {
            //Arrange
            GameViewModel model = null;
            //Act
            var statusCode = _gameController.New(model) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 406);
        }

        [Test]
        public void AddGame_ReturnHttpStatusCode201_GameViewModelIsValid()
        {
            //Arrange
            var a = new GameViewModel();
            //Act
            var statusCode = _gameController.New(a) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 201);
        }

        [Test]
        public void RemoveGame_ReturnHttpStatusCode200_IdIsValid()
        {
            //Arrange
            int Id = 1;
            //Act
            var statusCode = _gameController.Remove(Id) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 200);
        }

        [Test]
        public void RemoveGame_ReturnHttpStatusCode400_NotValidKey()
        {
            //Arrange
            GameDTO obj = null;
            int Id = -1;
            _mockGameService.Setup(m => m.DeleteGame(It.IsAny<int>())).Throws<ArgumentNullException>();
            //Act
            var statusCode = _gameController.Remove(Id) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 400);
        }

        [Test]
        public void UpdateGame_ReturnHttpStatusCode200_GameViewModelIsValid()
        {
            //Arrange
            var game = new GameViewModel();
            //Act
            var statusCode = _gameController.Update(game) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 200);
        }

        [Test]
        public void UpdateGame_ReturnHttpStatusCode400_GameViewModelHasModelStateError()
        {
            //Arrange
            _gameController.ModelState.AddModelError("test", "test");
            //Act
            var statusCode = _gameController.Update(It.IsAny<GameViewModel>()) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 400);
        }

        [Test]
        public void GetAllGamesByGenre_ReturnHttpStatusCode400_IdOfGenreIsNotValid()
        {
            //Arrange
            int notvalidkey = -1;
            _mockGameService.Setup(m => m.GetGamesByGenre(It.IsAny<int>())).Throws<ArgumentNullException>();
            //Act
            var statuscode = _gameController.GetAllGamesByGenre(notvalidkey) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statuscode.StatusCode, 400);
        }

        [Test]
        public void GetAllGamesByGenre_ReturnListOfGameViewModel_IdOfGenreIsValid()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetGamesByGenre(It.IsAny<int>()))
                .Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));
            //Act
            var result = ((_gameController.GetAllGamesByGenre(It.IsAny<int>()) as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            //Assert
            _mockGameService.Verify(m => m.GetGamesByGenre(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(result.Count, 2);
        }

        [Test]
        public void GetAllGamesByPlatformType_ReturnHttpStatusCode400_PlatformTypeIdIsNotValid()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetGamesByPlatformType(It.IsAny<int>())).Throws<ArgumentNullException>();
            int notvalidId = 0;
            //Act
            var result = _gameController.GetAllGamesByPlatformType(notvalidId) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        public void GetAllGamesByPlatformType_ReturnListOfGameViewModel_PlatformTypeIdIsValidKey()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetGamesByPlatformType(It.IsAny<int>())).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));
            int validkey = 1;
            //Act
            var result =
            ((_gameController.GetAllGamesByPlatformType(validkey) as JsonResult).Data as
                IEnumerable<GameViewModel>).ToList();
            //Assert
            _mockGameService.Verify(m => m.GetGamesByPlatformType(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void DownloadGameToFile_ReturnHttpStatusCode400_NotExistGameKeyinGetGameByKeyMethod()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetGameById(It.IsAny<int>())).Throws<ArgumentNullException>();
            int notvalidkey = -1;
            //Act
            var result = _gameController.DownloadGameToFile(notvalidkey) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

    }
}
