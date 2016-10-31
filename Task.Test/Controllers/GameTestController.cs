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
using Task.BLL.Nlog;
using Task.ViewModel;
using Task.MappingUI;

namespace Task.Test.Controllers
{
    [TestClass]
    public class GameTestController
    {
        private Mock<IGameService> _mockGameService;
        private GameController controller;
        private Mock<ILoggingService> logger;
        private Mock<IGameService> _mockGame;
        private List<Game> _gameList;
       [TestInitialize]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();

            this._mockGameService = new Mock<IGameService>();
            logger = new Mock<ILoggingService>();
            this.controller = new Task.Controllers.GameController(_mockGameService.Object, null, logger.Object);

            var genres = new List<Genre>{
                new Genre{Name="fdf",GenreId=1,ParentId=0}
            };
            var ptypes = new List<PlatformType>{
                new PlatformType{Key=1,Name="sd"}
            };
            _gameList = new List<Game>{
                new Game{Key=1, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)},
                new Game{Key=2, Name="Fifa",Description="Football",Genres=(genres),PlatformTypes=(ptypes)}
            };

        }
        [TestMethod]
        public void GetAllGames_ReturnList()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetAll()).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));
            //Act
            var result = ((controller.GetAllGames() as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Key);
            Assert.AreEqual(2, result[1].Key);
        }
        [TestMethod]
        public void GetAllGames_GetGamesIfNoRecodrs_Null()
        {
            ////Arrange
            //_mockGameService.Setup(m => m.GetAll()).Returns(Mapper.Map<ICollection<GameDTO>>(null));
            ////Act
            //var result = ((controller.GetAllGames() as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            ////Assert
            //Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllGamesByGenre_KeyOfGenreNotValid_ReturnListGamesNotSuccessible()
        {
            int key = -1;
            //Arrange
            
            //Act
            var result = ((controller.GetAllGamesByGenre(key) as JsonResult).Data as IEnumerable<GameViewModel>).ToList();
            //Assert
            Assert.AreEqual(1, result.Count);
            
        }
    }
}
