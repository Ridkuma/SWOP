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
		/// Constructor
		/// </summary>
		public LocalGame(IMap map, List<Player> players)
		{
			MapBoard = map;
			Players = players;
		}

        public Player GetCurrentPlayer()
        {
            return this.Players[this.CurrentPlayerId];
        }

		/// <summary>
		/// Add a new player to the existing list
		/// </summary>
		/// <param name="player"></param>
		public void AddNewPlayer(Player player)
		{
			Players.Add(player);
		}

		/// <summary>
		/// Launch the game
		/// </summary>
		public virtual void Start()
		{
            CurrentTurn = 1;
            CurrentPlayerId = 0;

			OnRaiseStartGame();
		}

		/// <summary>
		/// End of player turn, possible outcomes : next player, next game turn or end of game
		/// </summary>
		public virtual void NextPlayer()
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
                {
                    u.ChangeState(UnitState.Idle);
                    u.EndTurn();
                }
			}

		}

		/// <summary>
		/// Save the game in an external file
		/// </summary>
		public virtual void Save()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// End of the game
		/// </summary>
		public virtual void End()
		{
			OnRaiseEndGame();
        }


		/// <summary>
		/// Events
		/// </summary>
		#region Events

		public event EventHandler<EventArgs> OnStartGame;
		protected virtual void OnRaiseStartGame()
		{
			if (OnStartGame != null)
				OnStartGame(this, new EventArgs());
		}

		public event EventHandler<EventArgs> OnNextPlayer;
		protected virtual void OnRaiseNextPlayer()
		{
			if (OnNextPlayer != null)
				OnNextPlayer(this, new EventArgs());
		}

		public event EventHandler<EventArgs> OnEndGame;
		protected virtual void OnRaiseEndGame()
		{
			if (OnEndGame != null)
				OnEndGame(this, new EventArgs());
		}

		public event EventHandler<StringEventArgs> OnNewChatMessage;
		public virtual void OnRaiseNewChatMessage(string text)
		{
			if (OnNewChatMessage != null)
				OnNewChatMessage(this, new StringEventArgs(text));
		}

		#endregion


	}
}
