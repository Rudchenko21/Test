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
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly ILoggingService _logger;
        //static Logger _logger = LogManager.GetCurrentClassLogger();
        public GameController(IGameService gameServ, ICommentService commServ,ILoggingService logger)
        {
            this._gameService = gameServ;
            this._commentService = commServ;
            this._logger = logger;
        }
        [PerfomanceAction]
        public ActionResult GetAllGames()
        {
            IEnumerable<GameViewModel> games;
            try
            {
                games = Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(_gameService.GetAll());
            }
            catch (ArgumentException e)
            {
                _logger.Error(e);
                return HttpNotFound();
            }
            return Json(games, JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public ActionResult GetAllGamesByGenre(int id)
        {
            if(!this._gameService.ExistEntity(id))
            {
                return HttpNotFound();
            }
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(_gameService.GetByGenre(id)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public ActionResult GetAllGamesByPlatformType(int id)
        {
            if (id <= 0)
            {
                return HttpNotFound();
            }
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(_gameService.GetAllByPlatformType(id)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public FileResult DownloadGameToFile(int gamekey)
        {
            TxtWriter.WriteToFile(Path.Combine(Server.MapPath("~/Download"), "GameInfo.txt"), Mapper.Map<GameDTO, GameViewModel>(_gameService.GetGameByKey(gamekey)).ToString());
            return File("/Download/GameInfo.txt", "application/text");
        }
        [PerfomanceAction]
        public JsonResult GetGameByKey(string key)
        {
            return Json(Mapper.Map<GameDTO, GameViewModel>(_gameService.GetGameByNameKey(key)), JsonRequestBehavior.AllowGet);
        }
        [PerfomanceAction]
        public ActionResult GetAllCommentsByGames(string gamekey)
        {
            if (_gameService.ExistStringKey(gamekey))
            {
                var a =  Mapper.Map<IEnumerable<CommentDTO>,IEnumerable<CommentViewModel>>(_commentService.GetAllByGame(gamekey));
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult AddCommentToGame(CommentViewModel item)
        {
            if (ModelState.IsValid)
            {
                if (item != null)
                {
                    _commentService.AddCommentToGame(Mapper.Map<CommentDTO>(item));
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                else return new HttpStatusCodeResult(500);
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult AddGame(GameViewModel model)
        {
            if (model.Key == String.Empty)
            {
                ModelState.AddModelError("Key","Key couldn't be empty");
            }
            if (model.Name == String.Empty)
            {
                ModelState.AddModelError("Name", "Name couldn't be empty");
            }
            if (model.Description == String.Empty)
            {
                ModelState.AddModelError("Description", "Description couldn't be empty");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _gameService.AddGame(Mapper.Map<GameViewModel, GameDTO>(model));
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                catch (ArgumentNullException e)
                {
                    _logger.Error($"Object to add is null {e.Message}  TargetSite: {e.TargetSite}  StackTrace {e.StackTrace}");
                    return new HttpStatusCodeResult(404);
                }
                catch (ArgumentException e)
                {
                    _logger.Error($"{e.Message}  TargetSite: {e.TargetSite}  StackTrace {e.StackTrace}");
                    return new HttpStatusCodeResult(404);
                }
            }
            else
            {
                return new HttpStatusCodeResult(404);
            } 
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult UpdateGame(GameViewModel model)
        {
            if (ModelState.IsValid)
            {
                _gameService.Edit(Mapper.Map<GameViewModel, GameDTO>(model));
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }
            else return HttpNotFound();
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult RemoveGame(int key)
        {
            if (key > 0)
            {
                _gameService.DeleteGame(key);
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }
            
            return new HttpStatusCodeResult(500);
        }

        public JsonResult IsGameIdExist(int GameId)
        {
            return Json(_gameService.ExistEntity(GameId), JsonRequestBehavior.AllowGet);
        }
    }
}