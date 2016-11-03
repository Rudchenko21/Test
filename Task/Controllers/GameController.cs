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
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        private readonly ICommentService _commentService;

        private readonly ILoggingService _logger;

        private readonly IWriter _writer;

        public GameController(IGameService gameServ, ICommentService commServ, ILoggingService logger, IWriter writer)
        {
            this._gameService = gameServ;
            this._commentService = commServ;
            this._logger = logger;
            this._writer = writer;
        }

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

        public ActionResult GetAllGamesByGenre(int id)
        {
            var gamesDtos = _gameService.GetGamesByGenre(id);
            var gamesViewModels = Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(gamesDtos);
            return Json(gamesViewModels, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllGamesByPlatformType(int id)
        {
            IEnumerable<GameViewModel> gameByPlstform;
            try
            {
                gameByPlstform =
                    Mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(_gameService.GetGamesByPlatformType(id));
            }
            catch (ArgumentException e)
            {
                _logger.Error(e);
                return HttpNotFound();
            }
            return Json(gameByPlstform, JsonRequestBehavior.AllowGet);
        }


        public FileResult DownloadGameToFile(int gamekey)
        {
            var gameToWrite = Mapper.Map<GameDTO, GameViewModel>(_gameService.GetGameById(gamekey)).ToString();
            _writer.WriteToFile(Path.Combine(Server.MapPath("~/Download"), "GameInfo.txt"), gameToWrite);
            return File("/Download/GameInfo.txt", "application/text");
        }


        public ActionResult GetGameByKey(string key)
        {
            GameViewModel game;
            try
            {
                game = Mapper.Map<GameDTO, GameViewModel>(_gameService.SearchByKey(key));
            }
            catch (ArgumentNullException e)
            {
                _logger.Error(e);
                return new HttpStatusCodeResult(400);
            }
            return Json(game, JsonRequestBehavior.AllowGet);
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
                return  new HttpStatusCodeResult(400);
            }
            return Json(comments, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult AddGame(GameViewModel model)
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
                        _logger.Error(e);
                        return new HttpStatusCodeResult(400);
                    }
                    catch (ArgumentException e)
                    {
                        _logger.Error(e);
                        return new HttpStatusCodeResult(400);
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
        }

        [HttpPost]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult UpdateGame(GameViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _gameService.EditGame(Mapper.Map<GameViewModel, GameDTO>(model));
                }
                catch (ArgumentNullException e)
                {
                    _logger.Error(e);
                    return new HttpStatusCodeResult(400);
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else return new HttpStatusCodeResult(400);
        }


        [HttpPost]
        [OutputCache(Duration = 60)]
        public HttpStatusCodeResult RemoveGame(int id)
        {
            try
            {
                _gameService.DeleteGame(id);
            }
            catch (ArgumentNullException e)
            {
                _logger.Error(e);
                return new HttpStatusCodeResult(400);
            } 
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        public JsonResult IsGameIdExist(int gameId)
        {
            return Json(_gameService.ExistEntity(gameId), JsonRequestBehavior.AllowGet);
        }
    }
}