using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneCGame.Server.Models;

namespace OneCGame.Server
{
    public class GameLogic
    {
        private readonly User userA;
        private readonly User userB;
        private readonly Game game;

        private char[] map = new char[3 * 3];
        private User active;

        public GameLogic(Game game, User userA, User userB)
        {
            this.userA = userA;
            this.userB = userB;
            this.game = game;
            active = userA;
        }

        [ExecuteFromClient]
        public void Play()
        {
            game.SendEvent("NextTurn");
        }

        [ExecuteFromClient]
        public void Insert(User user, string IndexStr)
        {
            if (active != user)
                return;

            var symbol = active == userA ? 'O' : 'X';
            var index = int.Parse(IndexStr);

            if (map[index] != '\0')
                return;

            map[index] = symbol;
            game.SendEvent($"Set{symbol}?{IndexStr}");

            if (CheckVin(symbol))
            {
                game.SendEvent($"EndGame?Victory {active.Name}!");
            }
            else if (CheckEndGame())
            {
                game.SendEvent($"EndGame?There is no winner");
            }
            else
            {
                game.SendEvent("NextTurn");
                active = user == userA ? userB : userA;
            }
        }

        private bool CheckVin(char t)
        {
            for (int i = 0; i < 3; i++)
            {
                if (map[i * 3] == t && map[i * 3 + 1] == t && map[i * 3 + 2] == t)
                    return true;

                if (map[i] == t && map[i + 3] == t && map[i + 6] == t)
                    return true;
            }

            if (map[0] == t && map[5] == t && map[8] == t)
                return true;

            if (map[2] == t && map[5] == t && map[7] == t)
                return true;

            return false;
        }

        private bool CheckEndGame()
        {
            return !map.Any(c => c == '\0');
        }
    }
}