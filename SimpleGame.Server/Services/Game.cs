using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using OneCGame.Server.Models;
using System.Threading;

namespace OneCGame.Server
{
    public class Game
    {
        private readonly Dictionary<User, AutoResetEvent> UserListE = new Dictionary<User, AutoResetEvent>();
        private readonly Dictionary<User, int> UserListIndex = new Dictionary<User, int>();
        private readonly List<string> EventList = new List<string>();

        private GameLogic gameLogic;

        public User UserA { get; set; }
        public User UserB { get; set; }

        public Game(User userA)
        {
            UserA = userA;
            UserListIndex.Add(UserA, 0);
            UserListE.Add(UserA, new AutoResetEvent(false));
        }

        public void Join(User userB)
        {
            UserB = userB;

            UserListIndex.Add(UserB, 0);
            UserListE.Add(UserB, new AutoResetEvent(false));

            gameLogic = new GameLogic(this, UserA, UserB);

            SendEvent("play");
        }

        public async Task<List<string>> GetEventAsync(User user)
        {
            if (EventList.Count == UserListIndex[user])
            {
                await Task.Run(() => UserListE[user].WaitOne());
            }

            int delta = EventList.Count - UserListIndex[user];
            UserListIndex[user] = EventList.Count;

            return EventList.TakeLast(delta).ToList();
        }

        public void ReciveEvent(User from, string EventName, string arg)
        {
            var method = gameLogic.GetType().GetMethods()
                .Where(m => m.Name.Equals(EventName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (method == null || method.GetCustomAttributes<ExecuteFromClientAttribute>() == null)
            {
                return;
            }

            if (arg != null)
                method.Invoke(gameLogic, new object[] { from, arg });
            else
                method.Invoke(gameLogic, new object[] { from });
        }

        public void SendEvent(string EventName)
        {
            EventList.Add(EventName);

            foreach (var item in UserListE.Values)
            {
                item.Set();
            }
        }
    }
}