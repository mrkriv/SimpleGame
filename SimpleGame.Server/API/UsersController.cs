using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneCGame.Server.Models;

namespace OneCGame.Server.API
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly AuthManager AuthManager;
        private readonly DBContext db;

        public UserController(DBContext DbContext, AuthManager authManager)
        {
            AuthManager = authManager;
            db = DbContext;
        }

        [HttpGet]
        public async Task<ActionResult> Auth(string Login, string Password)
        {
            if (!db.User.Any())
            {
                db.User.Add(new User { Name = "user name 1", Login = "user1", Password = "Password" });
                db.User.Add(new User { Name = "user name 2", Login = "user2", Password = "Password" });
                db.SaveChanges();
            }

            var user = await db.User.SingleOrDefaultAsync(u => u.Login == Login && u.Password == Password);

            if (user == null)
                return StatusCode(403);

            AuthManager.CreateToken(HttpContext, user);
            return Ok();
        }
    }
}