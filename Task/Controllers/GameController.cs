using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.BLL.Models;
using Task.BLL.Services;
using Task.ViewModel;

namespace Task.Controllers
{
    public class GameController : Controller
    {
        IGameService gameService;
        ICommentService commentService;
        IWriter writer;
        public GameController(IGameService gameServ,ICommentService commServ)
        {
            this.gameService = gameServ;
            this.commentService = commServ;
            writer = new TxtWriter();
        }
        public JsonResult GetAllGames()
        {
            return Json(Mapper.Map<IEnumerable<GameDTO>,IEnumerable<GameViewModel>>(gameService.GetAll()), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllGamesByGenre(int id)
        {
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gameService.GetByGenre(id)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllGamesByPlatformType(int id)
        {
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gameService.GetAllByPlatformType(id)), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadGameToFile(int gamekey)
        {
            writer.WriteToFile(Path.Combine(Server.MapPath("~/Download"), "GameInfo.txt"),Mapper.Map<GameDTO,GameViewModel>(gameService.GetGameByKey(gamekey)).ToString());
            return File("/Download/GameInfo.txt","application/text");
        }
        public JsonResult GetGameByKey(int key)
        {
            return Json(Mapper.Map<GameDTO, GameViewModel>(gameService.GetGameByKey(key)),JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllCommentsByGames(int gamekey)
        {
            var a = (commentService.GetAllByGame(gamekey));
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public HttpStatusCodeResult AddCommentToGame(CommentViewModel item)
        {
            commentService.AddCommentToGame(Mapper.Map<CommentDTO>(item));
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [HttpPost]
        public HttpStatusCodeResult AddGame(GameViewModel model)
        {
            gameService.AddGame(Mapper.Map<GameViewModel, GameDTO>(model));
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
        [HttpPost]
        public HttpStatusCodeResult update(GameViewModel model)
        {
            gameService.Edit(Mapper.Map<GameViewModel, GameDTO>(model));
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [HttpPost]
        public HttpStatusCodeResult EditGame(GameViewModel model)
        {
            gameService.Edit(Mapper.Map<GameViewModel, GameDTO>(model));
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

    }
}