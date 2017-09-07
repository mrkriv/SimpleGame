using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneCGame.Server.Models;
using System.Net;

namespace OneCGame.Server.API
{
    [Produces("application/json")]
    public class GameController : Controller
    {
        private readonly GameManager GameManager;
        private readonly AuthManager AuthManager;
        private readonly DBContext db;

        public GameController(DBContext DbContext, GameManager gameManager, AuthManager authManager)
        {
            GameManager = gameManager;
            AuthManager = authManager;
            db = DbContext;
        }

        [Route("Game/event/{EventName}/{Arg?}")]
        public ActionResult SendEvent(string EventName, string Arg)
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            var game = GameManager.GetGame(token.User);

            game.ReciveEvent(token.User, EventName, Arg);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> WaitEvent()
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            var game = GameManager.GetGame(token.User);
            var events = await game.GetEventAsync(token.User);

            return Json(events);
        }

        [HttpGet]
        public ActionResult GetFreeGame()
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            return Json(GameManager.GetFreeGames().Select(u => u.UserA.Login));
        }

        [HttpGet]
        public async Task<ActionResult> JoinToGame(string UserALogin)
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            var userA = await db.User.SingleOrDefaultAsync(u => u.Login == UserALogin);

            if (userA == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            if (GameManager.JoinToUser(userA, token.User))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return Ok();
        }

        [HttpGet]
        public ActionResult StartFind()
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            GameManager.CreateGame(token.User);
            return Ok();
        }

        [HttpGet]
        public ActionResult EndFind()
        {
            if (!AuthManager.GetToken(HttpContext, out var token))
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            GameManager.DestroyGame(GameManager.GetGame(token.User));
            return Ok();
        }
    }
}