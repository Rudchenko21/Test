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
using System.Web.Mvc;
using Task;
using Task.Controllers;

namespace Task.Test.Controllers
{
    
    public class GameTestController
    {
        private Mock<IGameService> _mockGameService;
        private GameController controller;
        private List<Game> _gameList;

       
        public void Setup()
        {
            Mapper.CreateMap<Game, GameDTO>().ReverseMap();
            Mapper.CreateMap<Genre, GenreDTO>().ReverseMap();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>().ReverseMap();

            this._mockGameService = new Mock<IGameService>();
            this.controller = new Task.Controllers.GameController(_mockGameService.Object, null, null);

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
        public void GetAllGames()
        {
            //Arrange
            _mockGameService.Setup(m => m.GetAll()).Returns(Mapper.Map<ICollection<GameDTO>>(_gameList));
            //Act
            var result = (controller.GetAllGames() as JsonResult).Data as List<Game>;
            //Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
