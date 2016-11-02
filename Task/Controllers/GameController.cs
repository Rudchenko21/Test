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
    [LogIPFilter] // todo I guess, these two filters can be registered as global filters
                  // todo https://www.asp.net/mvc/overview/older-versions/hands-on-labs/aspnet-mvc-4-custom-action-filters Task 4: Registering Filters Globally
    [NLogException]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly ILoggingService _logger;
        //static Logger _logger = LogManager.GetCurrentClassLogger(); // todo please remove all commented code
        public GameController(IGameService gameServ, ICommentService commServ,ILoggingService logger)
        {
            this._gameService = gameServ;
            this._commentService = commServ;
            this._logger = logger;
        }// todo please use empty lines between all methods, logical parts and other for beautifying read code
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
        [PerfomanceAction] // todo looks like this filter can be registered as global filter
        public ActionResult GetAllGamesByGenre(int id)
        {
            if(!this._gameService.ExistEntity(id)) // todo please remove this. where it is not needed
            {
                return HttpNotFound();
            }

            var gamesDtos = _gameService.GetByGenre(id);
            var gamesViewModels = Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gamesDtos);

            return Json(gamesViewModels, JsonRequestBehavior.AllowGet);

        }// todo please remove empty lines like above, because such things make your code hard-readable
        [PerfomanceAction]
        public ActionResult GetAllGamesByPlatformType(int id)
        {
            if (id <= 0)
            {
                return HttpNotFound();
            }
            return Json(Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(_gameService.GetAllByPlatformType(id)), JsonRequestBehavior.AllowGet);
            // todo please introduce additional variables like at example above
        }
        [PerfomanceAction]
        public FileResult DownloadGameToFile(int gamekey)
        {
            // todo this method is untestable by unit tests, please use DI for TxtWriter
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
                var a =  Mapper.Map<IEnumerable<CommentDTO>,IEnumerable<CommentViewModel>>(_commentService.GetAllByGame(gamekey)); // todo please meanful names of variables
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
        public HttpStatusCodeResult AddCommentToGame(CommentViewModel item) // todo Single responsibility principle violated. This controller is responsible for managing games
        {
            if (ModelState.IsValid)
            {
                if (item != null)
                {
                    _commentService.AddCommentToGame(Mapper.Map<CommentDTO>(item));
                    return new HttpStatusCodeResult(HttpStatusCode.Created);
                }
                else return new HttpStatusCodeResult(500); // todo I guess, this case is unreachable because ModelState.IsValid should return false when item is null...
            }
            else
            {
                return new HttpStatusCodeResult(404); // todo when model state is not valid 404 error? Are you sure?
            }
        }

        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult AddGame(GameViewModel model)
        {
            if (model != null) // todo too much if's, I guess, this one: if (ModelState.IsValid) will be enough
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _gameService.AddGame(Mapper.Map<GameViewModel, GameDTO>(model));
                        return new HttpStatusCodeResult(HttpStatusCode.Created);
                    }
                    catch (ArgumentNullException e)
                    {
                        _logger.Error(
                            $"Object to add is null {e.Message}  TargetSite: {e.TargetSite}  StackTrace {e.StackTrace}");
                        // todo mess at logs. Try to beautify logs. For example: 
                        // todo _logger.Error($"Try to add null entity. Message: {e.Message}. Method: {e.TargetSite}. StackTrace: {e.StackTrace}.");
                    }
                    catch (ArgumentException e)
                    {
                        _logger.Error($"{e.Message}  TargetSite: {e.TargetSite}  StackTrace {e.StackTrace}");
                    }

                }
                return new HttpStatusCodeResult(404);
            }else return new HttpStatusCodeResult(500);
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult update(GameViewModel model) // todo Follow Code conventions. Methods should starts with capital letter
        {
            if (ModelState.IsValid)
            {
                _gameService.Edit(Mapper.Map<GameViewModel, GameDTO>(model));
                return new HttpStatusCodeResult(HttpStatusCode.Created); // todo wrong status code
            }
            else return HttpNotFound(); // todo why not found ..? It's just invalid model.
        }
        [HttpPost]
        [PerfomanceAction]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult remove(int key) // todo I suppose, it's id, not key...
        {
            if (key > 0) // todo why verification ?
            {
                _gameService.DeleteGame(key);
                return new HttpStatusCodeResult(HttpStatusCode.Created); // todo wrong status code
            }
            
            return new HttpStatusCodeResult(500); // todo why 500?
        }

        public JsonResult IsGameIdExist(int GameId) // todo Follow Code conventions. Variables should starts with small letter
        {
            return Json(_gameService.ExistEntity(GameId), JsonRequestBehavior.AllowGet);
        }
    }
}