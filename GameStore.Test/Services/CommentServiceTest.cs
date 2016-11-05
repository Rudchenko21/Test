using System;
using System.Collections.Generic;
using Moq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.BLL.Services;
using GameStore.BLL.DTO;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Mapping;
using System.Linq.Expressions;
using NUnit.Framework;


namespace GameStore.Test.Services
{
    [TestFixture]
    public class CommentServiceTest
    {
        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IRepository<Game>> _mockRepo;
        private ICommentService commentService;
        private List<Comment> comments;
        [SetUp]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();
            comments = new List<Comment>
            {
                new Comment {Id = 1, Body = "First comment", Name = "Yaroslav"},
                new Comment {Id = 2, Body = "Second comment", Name = "Yaroslav"},
                new Comment {Id = 3, Body = "Third comment", Name = "Yaroslav"}
            };
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            commentService = new CommentService(_mockUnitOfWork.Object);
        }
        [Test]
        public void GetCommentByGame_KeyValid_ReturnedListComment()
        {
            _mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());
            var fakeComment = new Comment();
            commentService.AddCommentToGame(Mapper.Map<CommentDTO>(fakeComment));
            _mockUnitOfWork.Verify(m => m.Comment.Add(It.IsAny<Comment>()), Times.Once);

        }
        [Test]
        public void GetCommentByGame_ValueIsNull_ReturnedListComment()
        {
            _mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>()).Count).Returns(0);
            Assert.Throws<ArgumentNullException>(() => commentService.AddCommentToGame(Mapper.Map<CommentDTO>(null)));
        }

        [Test]
        public void GetCommentByGame_KeyNotValid_ReturnedListComment()
        {
            //_mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns();
            //var fakeComment = new Comment();
            //var result=commentService.AddCommentToGame(Mapper.Map<CommentDTO>(fakeComment));
            //Assert.IsFalse(result);
        }
    }
}
