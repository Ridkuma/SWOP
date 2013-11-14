using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public class LocalGame : IGame
    {
        public Map MapBoard
        {
            get;
            set;
        }

		public int CurrentTurn
        {
            get;
            set;
        }

        public List<Player> Players
        {
            get;
            set;
        }

        public int CurrentPlayerId
        {
            get;
            set;
        }

		public LocalGame(Map map, List<Player> players)
		{
            this.Players = players;
            this.MapBoard = map;
            this.CurrentTurn = 1;
			Console.WriteLine("[Log] LocalGame created");
		}

		public void NextPlayer()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public void End()
		{
			throw new NotImplementedException();
		}
    }
}
