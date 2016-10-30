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
using Task.BLL.Services;
using Task.BLL.DTO;

namespace Task.Test.Services
{
    [TestClass]
    public class GameServiceTest
    {

        
        public void AddGame_SingleGameObject_ReturnedTrue()
        {
            var games = new List<Game>{
                new Game{Name="Fifa",Description="Football"}
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Game>>();
            mockSet.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(games.Provider);
            mockSet.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(games.Expression);
            mockSet.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(games.ElementType);
            mockSet.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(games.GetEnumerator());
            var mockContext = new Mock<GameStoreContext>();
            mockContext.Setup(m => m.Games).Returns(mockSet.Object);
            var mock = new Mock<IUnitOfWork>(mockContext.Object);
            var itemToAdd = new GameDTO { Name = "New Game", Description = "New game description" };
            // Action 
            GameService gameServ = new GameService(mock.Object);
            gameServ.AddGame(itemToAdd);
            //Assert
            Assert.AreEqual(3, gameServ.GetAll().Count);
            //mock.Setup(m => m.Game.Get()).Returns(new List<Game> { new Game { Name="sa"} });
        }
    }
}
