using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public class LocalGame : IGame
	{
        List<Player> Players
        {
            get;
            set;
        }

        int CurrentTurn
        {
            get;
            set;
        }

        Map MapBoard
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

		public void Pause()
		{
			throw new NotImplementedException();
		}

		public void Resume()
		{
			throw new NotImplementedException();
		}

		public void Launch()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}
	}
}
