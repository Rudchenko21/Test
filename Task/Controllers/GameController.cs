using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.BLL.Services;
using Task.ViewModel;

namespace Task.Controllers
{
    public class GameController : Controller
    {
        IGameService gameService;
        ICommentService commentService;
        public GameController(IGameService gameServ,ICommentService commServ)
        {
            this.gameService = gameServ;
            this.commentService = commServ;
        }
        public JsonResult GetAllGames()
        {
            return Json(Mapper.Map<IEnumerable<GameDTO>,IEnumerable<GameViewModel>>(gameService.GetAll()), JsonRequestBehavior.AllowGet);
        }
        //restriction 2 lavels of down 
        public JsonResult GetAllCommentsByGames()
        {
            var a = (commentService.GetAll());
            return Json(a, JsonRequestBehavior.AllowGet);
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