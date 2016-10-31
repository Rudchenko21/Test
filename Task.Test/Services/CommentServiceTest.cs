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


namespace Task.Test.Services
{
    [TestClass]
    public class CommentServiceTest
    {
        [TestMethod]
        public void GetCommentByGame_GetCommentByGameKey_ReturnedListComment()
        {

        }
    }
}
