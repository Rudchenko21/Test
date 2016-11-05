using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Nlog;
using GameStore.WEB.ViewModel;

namespace GameStore.WEB.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        private readonly ILoggingService _logger;

        public CommentController(ICommentService commServ, ILoggingService logger)
        {
            this._commentService = commServ;
            this._logger = logger;
        }

        public ActionResult Comments(string gamekey)
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
        public HttpStatusCodeResult Newcomment(CommentViewModel item)
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
                    //_logger.Error("Try to add a null entity. Message : {e.Message}  Method : {e.TargetSite}  StackTrace : {e.StackTrace}");
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