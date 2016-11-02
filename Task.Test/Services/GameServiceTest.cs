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
using System.Linq.Expressions;
using Task.DAL.Repository;
using Task.BLL.Services;
using Task.BLL.DTO;
using Task.DAL.UnitOfWork;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Task.BLL.Interfaces;
using Task.Controllers;
using System.Web.Mvc;
using NUnit.Core;
using NUnit.Framework;
using Task.BLL.Nlog;
using Task.ViewModel;
using Task.MappingUI;


namespace Task.Test.Controllers
{
    [TestFixture]
    public class GameServiceTest
    {
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IRepository<Game>> _mockRepo;
        IQueryable<Game> _games;
        IGameService _gameService;
        Mock<ILoggingService> _logger;
        List<Genre> _genres;
        List<PlatformType> _platformTypes;

        [SetUp]
        public void Set_up()
        {
            AutoMapperConfiguration.Configure();
            _logger = new Mock<ILoggingService>();

            _genres = new List<Genre>{
                new Genre{Name="Action",GenreId=1,ParentId=0},
                new Genre{Name="Football",GenreId=2,ParentId=0}
            };
            _platformTypes = new List<PlatformType>{
                new PlatformType{Key=1,Name="sd"}
            };
            _games = new List<Game>{
                new Game{Id=1,Key = "Fifa_2015",Name="Fifa",Description="Football",Genres=(new List<Genre> { _genres[0] }),PlatformTypes=(_platformTypes)},
                new Game{Id=2,Key = "Soccer_2015",Name="Soccer",Description="Football",Genres=(_genres),PlatformTypes=(_platformTypes)}
            }.AsQueryable();

            _mockRepo=new Mock<IRepository<Game>>();
            
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(m => m.Game).Returns(_mockRepo.Object);
             _gameService = new GameService(_mockUnitOfWork.Object,_logger.Object);
        }
        //AddGame_SingleGameObject_ReturnedTrue
        [Test]
        public void GetAll_AllGamesWhichConstistsinDB_ReturnTrue()
        {
            var newmock = new Mock<IUnitOfWork>();
            newmock.Setup(m => m.Game.GetAll()).Returns(_games.ToList());

            GameService gs = new GameService(newmock.Object,_logger.Object);
            var result = gs.GetAll();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result.ToList()[0].Id);
            Assert.AreEqual(2, result.ToList()[1].Id);
        }

        [Test]
        public void GetGame_GetGame_ByKeyWhichConstistsinDB_ReturnTrue()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Id == key).Count).Returns(1);
            GameDTO result = Mapper.Map<GameDTO>(_gameService.GetGameByKey(key));
            
            Assert.IsNotNull(result);
            //Assert.AreEqual<GameDTO>(result,Mapper.Map<GameDTO>(_games.ToList()[0]));
        }
        [Test]
        public void GetGame_KeyWhichNotValid_IndexOutOfRange()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Id == key).Count).Returns(0);
            Assert.Throws<ArgumentNullException>(() => _gameService.GetGameByKey(1));
        }
        [Test]
        public void GetGame_KyeWhichNotV0alid_IndexOutOfRange()
        {
            int key = -1;
            Assert.Throws<IndexOutOfRangeException>(() => _gameService.GetGameByKey(key));
        }
        //Test Add Method
        [Test]
        public void AddGame_GameIsNew_SuccesfullyAdd()
        {
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(0);
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            _gameService.AddGame(Mapper.Map<GameDTO>(FakeGame));
            _mockUnitOfWork.Verify(m => m.Game.Add(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
        [Test]
        public void AddGame_GameIsNull_ReturnNull()
        {
            Assert.Throws<ArgumentNullException>(() => _gameService.AddGame(null));
        }
        [Test]
        public void AddGame_GameObjectWithSameNameAsInDB_NotSuccesfull()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(1);
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            Assert.Throws<ArgumentException>(() => _gameService.AddGame(Mapper.Map<GameDTO>(FakeGame)));
        }
        //Test Edit Method
        [Test]
        public void EditGame_GameObjectWithChangedName_EditingSuccesfull()
        {
            var editItem = new Game { Id = 12, Key = "Fifa_2016",Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { _genres[0] }), PlatformTypes = (_platformTypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == editItem.Key).Count).Returns(1);
            _gameService.Edit(Mapper.Map<GameDTO>(editItem));
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        [Test]
        public void EditGame_GameObjectWasNotFoundInDB_EditingFailed()
        {
            var editItem = new Game { Id = 1, Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { _genres[0] }), PlatformTypes = (_platformTypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == editItem.Key).Count).Returns(1);
            _gameService.Edit(Mapper.Map<GameDTO>(editItem));
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Once);
        }
        [Test]
        //Test Delete Method
        public void DeleteGame_KeyValidAndRecordExistsinDB_GameDeleted()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(_games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Id == key).Count).Returns(1);
            _gameService.DeleteGame(key);
            _mockUnitOfWork.Verify(m => m.Game.Delete(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
        [Test]
        public void DeleteGame_KeyNotValidAndRecordNOExistsinDB_GameNotDeleted()
        {
            int key = -1;
            Assert.Throws<ArgumentException>(() => _gameService.DeleteGame(key));
        }
        [Test]
        public void DeleteGame_KeyValidAndRecordNOExistsinDB_GameNotDeleted()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(1);
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            Assert.Throws<ArgumentNullException>(() => _gameService.DeleteGame(key));
        }
        [Test]
        //Test Get Games by Genres
        public void GetGamesByGenre_GetGameByKeyOfGenre_Successful()
        {
            int _genreKey = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { _games.ToList()[0] });
            var result=Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetByGenre(_genreKey));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual(1, result.ToList()[0].Id);
        }
        [Test]
        public void GetGamesByGenre_GetGameByKeyThatNoExist_NotFound()
        {
            int _genreKey = 3;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetByGenre(_genreKey));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreNotEqual(1, result.ToList().Count);
        }
        [Test]
        //Test Get Games by Platform Type
        public void GetAllByPlatformType_GetGameByKeyOfPlatformTypeThatExist_Successful()
        {
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { _games.ToList()[0] });
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetAllByPlatformType(_platformType));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual(1, result.ToList()[0].Id);
        }
        [Test]
        public void GetAllByPlatformType_GetGameByKeyOfPlatformTypeThatNotExist_NotFound()
        {
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> {  });
            var result = Mapper.Map<IEnumerable<GameDTO>>(_gameService.GetAllByPlatformType(_platformType));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreNotEqual(1, result.ToList().Count);
        }

        [Test]
        public void ExistEntity_WithValidKey_ReturnTrue()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(1);
            var result = _gameService.ExistEntity(It.IsAny<int>());
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistEntity_WithNotValidKey_ReturnFalse()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(0);
            var result = _gameService.ExistEntity(It.IsAny<int>());
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistStringKey_WithValidStringKey_ReturnTrue()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(1);
            var result = _gameService.ExistStringKey(It.IsAny<string>());
            Assert.IsTrue(result);
        }
        [Test]
        public void ExistStringKey_WithNOTValidStringKey_ReturnFalse()
        {
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(0);
            var result = _gameService.ExistStringKey(It.IsAny<string>());
            Assert.IsFalse(result);
        }

        [Test]
        public void GetGameByNameKey_WithValidKey_ReturnNotNULLGame()
        {
            string key = "Fifa_2015";
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> { _games.ToList()[0]});
            var result = _gameService.GetGameByNameKey(key);
            Assert.AreEqual(result.Key,key);
        }

        [Test]
        public void GetGameByNameKey_WithNotValidKey_ReturnNullGame()
        {
            string key = String.Empty;
            Assert.Throws<ArgumentNullException>(() => _gameService.GetGameByNameKey(key));
        }
    }
}
