using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.BLL.Nlog;
using Task.ViewModel;

namespace Task.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        private readonly ILoggingService _logger;

        private readonly IWriter _writer;

        public CommentController(ICommentService commServ, ILoggingService logger, IWriter writer)
        {
            this._commentService = commServ;
            this._logger = logger;
            this._writer = writer;
        }
        public ActionResult GetAllCommentsByGames(string gamekey)
        {
            IEnumerable<CommentViewModel> comments;
            try
            {
                comments =
                    Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(
                        _commentService.GetAllByGame(gamekey));
            }
            catch (ArgumentNullException e)
            {
                _logger.Error(e);
                return new HttpStatusCodeResult(400);
            }
            return Json(comments, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult AddCommentToGame(CommentViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _commentService.AddCommentToGame(Mapper.Map<CommentDTO>(item));
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                catch (ArgumentNullException e)
                {
                    _logger.Error($"Try to add a null entity. Message : {e.Message}  Method : {e.TargetSite}  StackTrace : {e.StackTrace}");
                    return new HttpStatusCodeResult(400);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }
    }
}