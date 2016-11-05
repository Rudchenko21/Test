using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System.Linq.Expressions;
using GameStore.BLL.Services;
using GameStore.BLL.DTO;
using AutoMapper;
using GameStore.BLL.Interfaces;
using NUnit.Framework;
using GameStore.BLL.Nlog;
using GameStore.WEB.ViewModel;
using GameStore.WEB.Mapping;


namespace GameStore.Test.Controllers
{
    [TestFixture]
    public class GameServiceTest 
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;

        private Mock<IRepository<Game>> _mockRepo;

        private IQueryable<Game> _games;

        private IGameService _gameService;

        private Mock<ILoggingService> _logger;

        private List<Genre> _genres;

        private List<PlatformType> _platformTypes;

        [SetUp]
        public void Set_up()
        {
            AutoMapperConfiguration.Configure();
            _logger = new Mock<ILoggingService>();
            _mockRepo = new Mock<IRepository<Game>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(m => m.Game).Returns(_mockRepo.Object);
            _gameService = new GameService(_mockUnitOfWork.Object);

            _genres = new List<Genre>{
                new Genre{Name="Action",Id=1,ParentId=0},
                new Genre{Name="Football",Id=2,ParentId=0}
            };

            _platformTypes = new List<PlatformType>{
                new PlatformType{Id=1,Name="sd"}
            };

            _games = new List<Game>{
                new Game{Id=1,Key = "Fifa_2015",Name="Fifa",Description="Football",Genres=(new List<Genre> { _genres[0] }),PlatformTypes=(_platformTypes)},
                new Game{Id=2,Key = "Soccer_2015",Name="Soccer",Description="Football",Genres=(_genres),PlatformTypes=(_platformTypes)}
            }.AsQueryable();


        }
        [Test]
        public void GetAll_ReturnGameDTOList()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.GetAll()).Returns(_games.ToList());
            //Act
            var result = _gameService.GetAll();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetGameById_ReturnGameDTOByGameId_IdIsValid()
        {
            //Arrange
            int id = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            GameDTO result = Mapper.Map<GameDTO>(_gameService.GetGameById(id));
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetGameById_ThrowArgumentNullException_IdIsNotExistInDB()
        {
            //Arrange
            int gameId = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Id == gameId)).Returns(new List<Game> { });
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.GetGameById(gameId));
        }

        [Test]
        public void GetGameById_ThrowIndexOutOfRangeException_IdIsNotValid()
        {
            //Arrange
            int gameId = -1;
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Id == gameId)).Returns(new List<Game> { });
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.GetGameById(gameId));
        }

        [Test]
        public void AddGame_SuccesfullyAddNewGameToDB_GameViewModelIsValid()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });

            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            //Act
            _gameService.AddGame(Mapper.Map<GameDTO>(FakeGame));
            //Assert
            _mockUnitOfWork.Verify(m => m.Game.Add(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddGame_ThrowArgumentNullException_GameViewModelIsNull()
        {
            //Arrange
            GameViewModel game = null;
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.AddGame(Mapper.Map<GameDTO>(game)));
        }
        [Test]
        public void AddGame_ThrowArgumentException_GameObjectWithSameNameAlreadyHasInDB()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            Assert.Throws<InvalidOperationException>(() => _gameService.AddGame(Mapper.Map<GameDTO>(FakeGame)));
        }

        [Test]
        public void EditGame_EditGameEntity_GameViewModelIsValid()
        {
            //Arrange
            var editItem = new Game { Id = 1, Key = "Fifa_2016", Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { _genres[0] }), PlatformTypes = (_platformTypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            _gameService.EditGame(Mapper.Map<GameDTO>(editItem));
            //Assert
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void EditGame_GameObjectWasNotFoundInDB_EditingFailed()
        {
            //Arrange
            var editItem = new Game { Id = 1, Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { _genres[0] }), PlatformTypes = (_platformTypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList().FirstOrDefault()));
            //Act
            _gameService.EditGame(Mapper.Map<GameDTO>(editItem));
            //Assert
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Once);
        }

        [Test]
        //Test Delete Method
        public void DeleteGame_GameWithDefinatelyIdWasDeleted_GameIdIsValid()
        {
            //Arrange
            int id = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            _gameService.DeleteGame(id);
            //Assert
            _mockUnitOfWork.Verify(m => m.Game.Delete(id), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteGame_ThrowArgumentExceptionRecordWasNotDeleted_IdIsNotValid()
        {
            //Arrange
            int id = -1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.DeleteGame(id));
        }

        [Test]
        public void DeleteGame_ThrowArgumentNullException_NoRecordFind()
        {
            //Arrange
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.DeleteGame(key));
        }

        [Test]
        public void GetGamesByGenre_ReturnListOfGames_GenreIdIsValid()
        {
            //Arrange
            int _genreId = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { _games.ToList()[0] });
            //Act
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetGamesByGenre(_genreId));
            //Assert
            Assert.AreEqual(1, result.ToList().Count);
        }

        [Test]
        public void GetGamesByGenre_ReturnEmptyList_KeyIsNotValid()
        {
            //Arrange
            int _genreKey = 3;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });
            //Act
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetGamesByGenre(_genreKey));
            //Assert
            Assert.AreNotEqual(1, result.ToList().Count);
        }

        [Test]
        public void GetGamesByPlatformType_ReturnListOfGames_IdIsValid()
        {
            //Arrange
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { _games.ToList()[0] });
            //Act
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetGamesByPlatformType(_platformType));
            //Assert
            Assert.AreEqual(1, result.ToList().Count);
        }

        [Test]
        public void GetGamesByPlatformType_ReturnEmptyList_GameViewWasNotFindById()
        {
            //Arrange
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });
            //Act
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetGamesByPlatformType(_platformType));
            //Assert
            Assert.AreNotEqual(1, result.ToList().Count);
        }

        [Test]
        public void ExistEntity_ReturnTrue_EntityExist()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            var result = _gameService.ExistEntity(It.IsAny<int>());
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistEntity_ReturnFalse_IdIsNotValid()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });
            //Act
            var result = _gameService.ExistEntity(It.IsAny<int>());
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistEntity_WithValidStringId_ReturnTrue()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(_games.ToList());
            //Act
            var result = _gameService.ExistEntity(It.IsAny<string>());
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void ExistEntity_WithNOTValidStringId_ReturnFalse()
        {
            //Arrange
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            //Act
            var result = _gameService.ExistEntity(It.IsAny<string>());
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetGameByNameKey_WithValidKey_ReturnNotNULLGame()
        {
            //Arrange
            string key = "Fifa_2015";
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { _games.ToList().First() });
            //Act
            var result = _gameService.SearchByKey(key);
            //Assert
            Assert.AreEqual(result.Key, key);
        }
    }
}
