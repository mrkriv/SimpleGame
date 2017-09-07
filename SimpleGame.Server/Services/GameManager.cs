using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using OneCGame.Server.Models;
using System.Collections.Generic;

namespace OneCGame.Server
{
    public class GameManager
    {
        private ConcurrentDictionary<User, Game> ActiveGames = new ConcurrentDictionary<User, Game>();

        public Game CreateGame(User user1)
        {
            var game = new Game(user1);

            ActiveGames.AddOrUpdate(user1, game, (u, i) => game);

            return game;
        }

        public Game GetGame(User user)
        {
            ActiveGames.TryGetValue(user, out var game);
            return game;
        }

        public bool JoinToUser(User host, User user2)
        {
            ActiveGames.TryGetValue(host, out var game);

            if (game == null)
            {
                return false;
            }

            ActiveGames.AddOrUpdate(user2, game, (u, i) => game);
            game.Join(user2);

            return true;
        }

        public IEnumerable<Game> GetFreeGames()
        {
            return ActiveGames.Values.Where(g => g.UserB == null);
        }

        public void DestroyGame(Game game)
        {
            ActiveGames.TryRemove(game.UserA, out var _);

            if (game.UserB != null)
                ActiveGames.TryRemove(game.UserB, out var _);
        }
    }
}