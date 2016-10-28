using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entities;
using Task.DAL.Interfaces;

namespace Task.Test.Services
{
    [TestClass]
    class GameServiceTest
    {
        [TestMethod]
        public void AddGame_SingleGameObject_ReturnedTrue()
        {
            var mock = new Mock<IUnitOfWork>();
            //mock.Setup(m => m.Game.Get()).Returns(new List<Game> { new Game { Name="sa"} });
        }
    }
}
