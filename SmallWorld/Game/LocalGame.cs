using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public class LocalGame : IGame
    {
		/// <summary>
		/// Properties
		/// </summary>
        public IMap MapBoard { get; protected set; }
        public List<Player> Players { get; protected set; }
		public int CurrentPlayerId { get; protected set; }
		public int CurrentTurn { get; protected set; }

		/// <summary>
		/// Events
		/// </summary>
		public event EventHandler<EventArgs> OnStartGame;
		public event EventHandler<EventArgs> OnNextPlayer;
		public event EventHandler<EventArgs> OnEndGame;

		/// <summary>
		/// Constructor
		/// </summary>
		public LocalGame(IMap map, List<Player> players)
		{
			MapBoard = map;
			Players = players;
		}

		/// <summary>
		/// Launch the game
		/// </summary>
		public void Start()
		{
            CurrentTurn = 1;
            CurrentPlayerId = 0;

			OnRaiseStartGame();
		}

		/// <summary>
		/// End of player turn, possible outcomes : next player, next game turn or end of game
		/// </summary>
		public void NextPlayer()
		{
			CurrentPlayerId++;
			if (CurrentPlayerId >= Players.Count)
			{
				if (CurrentTurn >= MapBoard.TotalNbTurn) // Is end of total turns ? => game over
				{
					End();
				}
				else
				{
					CurrentPlayerId = 0;
					CurrentTurn++;
					OnRaiseNextPlayer();
				}
			}
			else
			{
				OnRaiseNextPlayer();
			}
			
			// Put each units on Idle state
			foreach (Player p in Players)
			{
				foreach (IUnit u in p.CurrentFaction.Units)
					u.ChangeState(UnitState.Idle);
			}

		}

		/// <summary>
		/// Save the game in an external file
		/// </summary>
		public void Save()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// End of the game
		/// </summary>
		public void End()
		{
			OnRaiseEndGame();
        }


		#region Events

		protected virtual void OnRaiseStartGame()
		{
			if (OnStartGame != null)
				OnStartGame(this, new EventArgs());
		}

		protected virtual void OnRaiseNextPlayer()
		{
			if (OnNextPlayer != null)
				OnNextPlayer(this, new EventArgs());
		}

		protected virtual void OnRaiseEndGame()
		{
			if (OnEndGame != null)
				OnEndGame(this, new EventArgs());
		}

		#endregion
    }
}
