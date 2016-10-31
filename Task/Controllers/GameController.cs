using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.BLL.Models;
using Task.BLL.Services;
using Task.ViewModel;
using Task.Filters;
using Task.BLL.Nlog;

namespace Task.Controllers
{
    [LogIPFilter]
    [NLogException]
    public class GameController : Controller
    {
        IGameService gameService;
        ICommentService commentService;
        ILoggingService logger;
        //static Logger logger = LogManager.GetCurrentClassLogger();
        public GameController(IGameService gameServ, ICommentService commServ,ILoggingService logger)
        {
            this.gameService = gameServ;
            this.commentService = commServ;
            this.logger = logger;
        }
        [PerfomanceAction]
        public JsonResult GetAllGames()
        {
            logger.Info("All games was shown by method GetAllGames() in GameController");            
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gameService.GetAll()), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public ActionResult GetAllGamesByGenre(int id)
        {
            if(!this.gameService.ExistEntity(id))
            {
                return HttpNotFound();
            }
            logger.Info( "All games were shown for genre with key : {id} by GetAllGamesByGenre in GameController");
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gameService.GetByGenre(id)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public JsonResult GetAllGamesByPlatformType(int id)
        {
            logger.Info( "All games were shown for platformtype with key : {id} by GetAllGamesByPlatformType in GameController");
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gameService.GetAllByPlatformType(id)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public FileResult DownloadGameToFile(int gamekey)
        {
            TxtWriter.WriteToFile(Path.Combine(Server.MapPath("~/Download"), "GameInfo.txt"), Mapper.Map<GameDTO, GameViewModel>(gameService.GetGameByKey(gamekey)).ToString());
            logger.Info( "File GameInfo.txt with detailed description of game with the gey {gamekey} by performing of method DownloadGameToFile in GameController");
            return File("/Download/GameInfo.txt", "application/text");
        }
        [PerfomanceAction]
        public JsonResult GetGameByKey(int key)
        {
            logger.Info( "Game with key : {key} was shown by method GetGameByKey in GameController");
            return Json(Mapper.Map<GameDTO, GameViewModel>(gameService.GetGameByKey(key)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public JsonResult GetAllCommentsByGames(int gamekey)
        {
            var a = (commentService.GetAllByGame(gamekey));
            logger.Info( "Get comments by game with the key : {gamekey} was shown by method GetAllCommentsByGames");
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public HttpStatusCodeResult AddCommentToGame(CommentViewModel item)
        {
            commentService.AddCommentToGame(Mapper.Map<CommentDTO>(item));
            logger.Info( "New comment was succesfully added to game with the key of {item.GameKey} by AddCommentToGame in  GameController");
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public HttpStatusCodeResult AddGame(GameViewModel model)
        {
            gameService.AddGame(Mapper.Map<GameViewModel, GameDTO>(model));
            logger.Info( "New game was added succesfully by AddGame in GameController");
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public HttpStatusCodeResult update(GameViewModel model)
        {
            gameService.Edit(Mapper.Map<GameViewModel, GameDTO>(model));
            logger.Info( "Game with key : {model.Key} was updated succesfully by update in GameController");
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public HttpStatusCodeResult remove(int key)
        {
            if (key > 0)
            {
                gameService.DeleteGame(key);
                logger.Info( "Game with key : {key} was removed succesfully");
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }
            logger.Info( "Game with key {key} was not removed. It could occur because key wasn't find in your db");
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}