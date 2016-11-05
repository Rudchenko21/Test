using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Moq;
using NUnit.Framework;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Nlog;
using GameStore.DAL.Entities;
using GameStore.WEB.Controllers;
using GameStore.WEB.Mapping;
using GameStore.WEB.ViewModel;

namespace GameStore.Test.Controllers
{
    public class CommentControllerTests
    {
        private Mock<IGameService> _mockGameService;

        private Mock<ICommentService> _mockCommentService;

        private CommentController _commentController;

        private Mock<ILoggingService> _logger;

        private List<Comment> _commentList;

        [SetUp]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();
            _mockGameService = new Mock<IGameService>();
            _mockCommentService = new Mock<ICommentService>();
            _logger = new Mock<ILoggingService>();
            _commentController = new CommentController(_mockCommentService.Object, _logger.Object);
            _commentList = new List<Comment>
            {
                new Comment { }
            };
        }

        [Test]
        public void Comments_GetListOfGamesByGameKey_GameKeyValid()
        {
            //Arrange
            _mockCommentService.Setup(m => m.GetAllByGame(It.IsAny<string>())).Returns(Mapper.Map<ICollection<CommentDTO>>(_commentList));
            _mockGameService.Setup(m => m.ExistEntity(It.IsAny<string>())).Returns(true);
            var result = ((_commentController.Comments("GTA_5") as JsonResult).Data as IEnumerable<CommentViewModel>).ToList();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void NewComment_AddCommentToGame_CommentViewModelConsistModelError()
        {
            //Arrange
            _commentController.ModelState.AddModelError("test", "test");
            //Act
            var statusCode = _commentController.Newcomment(It.IsAny<CommentViewModel>()) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 406);
        }

        [Test]
        public void NewComment_AddCommentToGame_CommentViewModelIsValid()
        {

            //Arrange
            var a = new CommentViewModel { Name = "Yaroslav", Body = "Description are represented here", GameId = 1 };
            //Act
            var statusCode = _commentController.Newcomment(a) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(statusCode.StatusCode, 201);
        }

        [Test]
        public void NewComment_AddCommentToGame_CommentViewModelIsNull()
        {
            _mockCommentService.Setup(m => m.AddCommentToGame(It.IsAny<CommentDTO>())).Throws<ArgumentNullException>();
            var statusCode = _commentController.Newcomment(null) as HttpStatusCodeResult;
            Assert.AreEqual(statusCode.StatusCode, 400);
        }


    }
}
