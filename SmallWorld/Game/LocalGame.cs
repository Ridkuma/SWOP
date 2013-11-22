using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public class LocalGame : IGame
    {
        public Map MapBoard { get; protected set; }
		public int CurrentTurn { get; protected set; }
        public List<Player> Players { get; protected set; }
        public int CurrentPlayerId { get; protected set; }
        

		public LocalGame(Map map, List<Player> players)
		{
            Players = players;
            MapBoard = map;
            CurrentTurn = 1;
            CurrentPlayerId = 0;
			Console.WriteLine("[Log] LocalGame created");
		}

		public void NextPlayer()
		{
			CurrentPlayerId++;
			if (CurrentPlayerId >= Players.Count)
			{
				if (CurrentTurn >= MapBoard.TotalNbTurn) // Is end of game ?
				{
					End();
				}
				else
				{
					CurrentPlayerId = 0;
					CurrentTurn++;
				}
			}
			
			// Put each units on Idle state
			foreach (Player p in Players)
			{
				foreach (IUnit u in p.CurrentFaction.Units)
					u.ChangeState(UnitState.Idle);
			}
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
