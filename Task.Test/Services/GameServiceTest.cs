using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
using Task.MappingUI;
using System.Linq.Expressions;

namespace Task.Test.Controllers
{
    [TestClass]
    public class GameServiceTest
    {
        Mock<GameStoreContext> _mockContext;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IRepository<Game>> _mockRepo;
        IQueryable<Game> games;
        IGameService gameService;
        List<Genre> genres;
        List<PlatformType> ptypes;
        [TestInitialize]
        public void Set_up()
        {
            AutoMapperConfiguration.Configure();

            genres = new List<Genre>{
                new Genre{Name="Action",GenreId=1,ParentId=0},
                new Genre{Name="Football",GenreId=2,ParentId=0}
            };
            ptypes = new List<PlatformType>{
                new PlatformType{Key=1,Name="sd"}
            };
            games = new List<Game>{
                new Game{Key=1, Name="Fifa",Description="Football",Genres=(new List<Genre> { genres[0] }),PlatformTypes=(ptypes)},
                new Game{Key=2, Name="Soccer",Description="Football",Genres=(genres),PlatformTypes=(ptypes)}
            }.AsQueryable();

            _mockContext = new Mock<GameStoreContext>();
            _mockRepo=new Mock<IRepository<Game>>();
            
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(m => m.Game).Returns(_mockRepo.Object);
            gameService = new GameService(_mockUnitOfWork.Object);
        }
        //AddGame_SingleGameObject_ReturnedTrue
        [TestMethod]
        public void GetAll_AllGamesWhichConstistsinDB_ReturnTrue()
        {
            var newmock = new Mock<IUnitOfWork>();
            newmock.Setup(m => m.Game.GetAll()).Returns(games.ToList());

            GameService gs = new GameService(newmock.Object);
            var result = gs.GetAll();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result.ToList()[0].Key);
            Assert.AreEqual(2, result.ToList()[1].Key);
        }
        [TestMethod]
        public void GetAll_AllGamesWhichNotConstistsinDB_ReturnNull()
        {
            _mockUnitOfWork.Setup(m => m.Game.GetAll()).Returns(new List<Game> { });
            GameService gs = new GameService(_mockUnitOfWork.Object);
            var result = gs.GetAll();
            Assert.IsNull(result);
        }

        //Test GetAll() method 
        [TestMethod]
        public void GetGame_GetGameByKeyWhichConstistsinDB_ReturnTrue()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == key).Count).Returns(0);
            GameDTO result = Mapper.Map<GameDTO>(gameService.GetGameByKey(key));
            
            Assert.IsNotNull(result);
            Assert.AreEqual<GameDTO>(result,Mapper.Map<GameDTO>(games.ToList()[0]));
        }
        [TestMethod]
        public void GetGame_GetGameByKeyWhichNotConstistsinDB_ReturnNull()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == key).Count).Returns(1);
            GameDTO result = Mapper.Map<GameDTO>(gameService.GetGameByKey(key));

            Assert.IsNull(result);
        }

        //Test Add Method
        [TestMethod]
        public void AddGame_GameIsNew_SuccesfullyAdd()
        {
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(0);
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            gameService.AddGame(Mapper.Map<GameDTO>(FakeGame));
            _mockUnitOfWork.Verify(m => m.Game.Add(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void AddGame_GameIsNull_ReturnNull()
        {
            gameService.AddGame(Mapper.Map<GameDTO>(null));
            _mockRepo.Verify(m => m.Add(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        [TestMethod]
        public void AddGame_GameObjectWithSameNameAsInDB_NotSuccesfull()
        {
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()).Count).Returns(1);
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            gameService.AddGame(Mapper.Map<GameDTO>(FakeGame));
            _mockUnitOfWork.Verify(m => m.Game.Add(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        //Test Edit Method
        [TestMethod]
        public void EditGame_GameObjectWithChangedName_EditingSuccesfull()
        {
            var editItem = new Game { Key = 12, Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { genres[0] }), PlatformTypes = (ptypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == editItem.Key).Count).Returns(1);
            gameService.Edit(Mapper.Map<GameDTO>(editItem));
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        [TestMethod]
        public void EditGame_GameObjectWasNotFoundInDB_EditingFailed()
        {
            var editItem = new Game { Key = 1, Name = "NewGameName", Description = "Football", Genres = (new List<Genre> { genres[0] }), PlatformTypes = (ptypes) };
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == editItem.Key).Count).Returns(0);
            gameService.Edit(Mapper.Map<GameDTO>(editItem));
            _mockUnitOfWork.Verify(m => m.Game.Edit(It.IsAny<Game>()), Times.Once);
        }
        [TestMethod]
        //Test Delete Method
        public void DeleteGame_RecordWithKeyExist_GameDeleted()
        {
            int key = 1;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games.ToList()[0]));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == key).Count).Returns(0);
            gameService.DeleteGame(key);
            _mockUnitOfWork.Verify(m => m.Game.Delete(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void DeleteGame_RecordWithKeyNotExist_GameNotDeleted()
        {
            int key = 2;
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(null));
            _mockUnitOfWork.Setup(m => m.Game.Get(m1 => m1.Key == key).Count).Returns(1);
            gameService.DeleteGame(key);
            _mockUnitOfWork.Verify(m => m.Game.Delete(It.IsAny<int>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        [TestMethod]
        //Test Get Games by Genres
        public void GetGamesByGenre_GetGameByKeyOfGenre_Successful()
        {
            int _genreKey = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { games.ToList()[0] });
            var result=Mapper.Map<IEnumerable<GameDTO>>(gameService.GetByGenre(_genreKey));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual(1, result.ToList()[0].Key);
        }
        [TestMethod]
        public void GetGamesByGenre_GetGameByKeyThatNoExist_NotFound()
        {
            int _genreKey = 3;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { });
            var result = Mapper.Map<IEnumerable<GameDTO>>(gameService.GetByGenre(_genreKey));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreNotEqual(1, result.ToList().Count);
        }
        [TestMethod]
        //Test Get Games by Platform Type
        public void GetAllByPlatformType_GetGameByKeyOfPlatformTypeThatExist_Successful()
        {
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { games.ToList()[0] });
            var result = Mapper.Map<IEnumerable<GameDTO>>(gameService.GetAllByPlatformType(_platformType));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual(1, result.ToList()[0].Key);
        }
        [TestMethod]
        public void GetAllByPlatformType_GetGameByKeyOfPlatformTypeThatNotExist_NotFound()
        {
            int _platformType = 1;
            _mockUnitOfWork.Setup(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> {  });
            var result = Mapper.Map<IEnumerable<GameDTO>>(gameService.GetAllByPlatformType(_platformType));
            _mockUnitOfWork.Verify(m => m.Game.Get(It.IsAny<Expression<Func<Game, bool>>>()), Times.Once);
            Assert.AreNotEqual(1, result.ToList().Count);
        }
    }
}
