﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.WEB.ViewModel
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Game Id must be specified")]
        [Remote("IsGameIdExist", "Game", ErrorMessage = "Game Id with such key doesn't exists in db")]
        public int GameId { get; set; }

        public string GameKey { get; set; }

        [Required]
        [StringLength(40,MinimumLength = 3,ErrorMessage = "Please, specify a correct name namely from 3 to 40 symbols")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field can't be null.")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Please, specify a correct name namely from 3 to 40 symbols")]
        public string Body { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}