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

namespace Task.Test.Controllers
{
    [TestClass]
    public class GameControllerTest
    {
        Mock<GameStoreContext> _mockContext;
        Mock<DbSet<Game>> mockSet;
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IRepository<Game>> _mockRepo;
        IQueryable<Game> games;
        IGameService gameService;

        [TestInitialize]
        public void Set_up()
        {
            Mapper.CreateMap<Game, GameDTO>().ReverseMap();
            Mapper.CreateMap<Genre, GenreDTO>().ReverseMap();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>().ReverseMap();

            var genres = new List<Genre>{
                new Genre{Name="fdf",GenreId=1,ParentId=0}
            };
            var ptypes = new List<PlatformType>{
                new PlatformType{Key=1,Name="sd"}
            };
            games = new List<Game>{
                new Game{Key=1, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)},
                new Game{Key=2, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)}
            }.AsQueryable();
            mockSet = new Mock<DbSet<Game>>();
            mockSet.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(games.Provider);
            mockSet.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(games.Expression);
            mockSet.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(games.ElementType);
            mockSet.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(games.GetEnumerator());

            _mockContext = new Mock<GameStoreContext>();
            _mockContext.Setup(m => m.Set<Game>()).Returns(mockSet.Object);
            _mockContext.Setup(m => m.Games).Returns(mockSet.Object);
            _mockRepo=new Mock<IRepository<Game>>();
            
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            //_mockRepo.Setup(m => m.GetById(It.Is<int>(s => s != 1))).Returns<Game>(null);
            _mockUnitOfWork.Setup(m => m.Game).Returns(_mockRepo.Object);
            gameService = new GameService(_mockUnitOfWork.Object);
        }
        //AddGame_SingleGameObject_ReturnedTrue
        [TestMethod]
        public void GetAll_AllGamesWhichConstistsinDB()
        {
            var games = new List<Game>{
                new Game{Name="Fifa",Description="Football"}
            };
            var newmock = new Mock<IUnitOfWork>();
            newmock.Setup(m => m.Game.GetAll()).Returns(games);

            GameService gs = new GameService(newmock.Object);
            var result = gs.GetAll();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Fifa", result.ToList()[0].Name);
        }
        [TestMethod]
        public void GetGame_GetGameByKeyWhichConstistsinDB_ReturnTrue()
        {
            var games = new List<GameDTO>{
                new GameDTO{Name="Fifa",Description="Football",Key=1},
                new GameDTO{Name="NBA",Description="Basketball",Key=2}
            };
            _mockUnitOfWork.Setup(m => m.Game.GetAll()).Returns(Mapper.Map<ICollection<Game>>(games));
            _mockUnitOfWork.Setup(m => m.Game.GetById(It.IsAny<int>())).Returns(Mapper.Map<Game>(games[0]));
            
            GameDTO result = Mapper.Map<GameDTO>(gameService.GetGameByKey(1));
            
            Assert.IsNotNull(result);
            Assert.AreEqual<GameDTO>(result, games[0]);
        }
        [TestMethod]
        public void AddGame_GameIsNew_SuccesfullyAdd()
        {
            var FakeGame = new GameDTO
            {
                Name = "Mortal Combat",
                Description = "Fight"
            };
            gameService.AddGame(Mapper.Map<GameDTO>(FakeGame));
            _mockRepo.Verify(m => m.Add(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void AddGame_GameIsNull_NotAdded()
        {
            gameService.AddGame(Mapper.Map<GameDTO>(null));
            _mockRepo.Verify(m => m.Add(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Never);
        }
        [TestMethod]
        public void DeleteGame_RecordWithKeyExist_GameDeleted()
        {
            int key = 2;
             gameService.DeleteGame(key);
            _mockRepo.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
