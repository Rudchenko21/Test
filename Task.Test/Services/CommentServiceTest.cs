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
using NUnit.Core;
using NUnit.Framework;


namespace Task.Test.Services
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
                new Comment {Key = 1, Body = "First comment", Name = "Yaroslav"},
                new Comment {Key = 2, Body = "Second comment", Name = "Yaroslav"},
                new Comment {Key = 3, Body = "Third comment", Name = "Yaroslav"}
            };
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            commentService = new CommentService(_mockUnitOfWork.Object);
        }
        [Test]
        public void GetCommentByGame_KeyValid_ReturnedListComment()
        {
            _mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>()).Count).Returns(0);
            var fakeComment = new Comment();
            commentService.AddCommentToGame(Mapper.Map<CommentDTO>(fakeComment));
            _mockUnitOfWork.Verify(m=>m.Comment.Add(It.IsAny<Comment>()),Times.Once);

        }
        [Test]
        public void GetCommentByGame_ValueIsNull_ReturnedListComment()
        {
            _mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>()).Count).Returns(0);
            var result=commentService.AddCommentToGame(Mapper.Map<CommentDTO>(null));
            Assert.IsFalse(result);
        }

        [Test]
        public void GetCommentByGame_KeyNotValid_ReturnedListComment()
        {
            _mockUnitOfWork.Setup(m => m.Comment.Get(It.IsAny<Expression<Func<Comment, bool>>>()).Count).Returns(1);
            var fakeComment = new Comment();
            var result=commentService.AddCommentToGame(Mapper.Map<CommentDTO>(fakeComment));
            Assert.IsFalse(result);
        }
    }
}
