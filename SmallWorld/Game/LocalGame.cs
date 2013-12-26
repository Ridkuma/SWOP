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
		public int CurrentTurn { get; protected set; }
		public List<Player> Players { get; protected set; }
		public int LocalPlayerId { get; protected set; } // -1 if local game
		public int CurrentPlayerId { get; protected set; }
		public bool CurrentPlayerIsMe { get { return (LocalPlayerId == -1 || LocalPlayerId == CurrentPlayerId); } }

		/// <summary>
		/// Constructor
		/// </summary>
		public LocalGame(IMap map, List<Player> players)
		{
			MapBoard = map;
			CurrentTurn = 0;
			Players = players;
			LocalPlayerId = -1;
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
		/// Return current playing Player
		/// </summary>
		/// <returns></returns>
        public Player GetCurrentPlayer()
        {
            return this.Players[this.CurrentPlayerId];
        }

		/// <summary>
		/// Launch the game
		/// </summary>
		public virtual void Start(bool generateUnits = true)
		{
            CurrentTurn = 1;
            CurrentPlayerId = 0;

			if (MapBoard == null)
				throw new NotSupportedException(); // Map must exist

			if (generateUnits)
			{
				for (int i = 0; i < Players.Count; i++)
					Players[i].CurrentFaction.GenerateUnits(MapBoard.TotalNbUnits, MapBoard.GetStartTile(i));
			}

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
				if (p.CurrentFaction == null || p.CurrentFaction.Units == null)
					throw new NotImplementedException(); // Faction and/or units MUST already be existing here (adding a player in middle of the game ??)

                foreach (IUnit u in p.CurrentFaction.Units)
                {
                    u.ChangeState(UnitState.Idle);
                    u.EndTurn();
                }
			}
		}

		/// <summary>
		/// Process the move of a unit on the map
		/// </summary>
		public virtual void MoveUnit(IUnit unit, ITile destination)
		{
			unit.RealMove(destination);
			OnRaiseMoveUnit();
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

        public event EventHandler<EventArgs> OnMoveUnit;
        protected virtual void OnRaiseMoveUnit()
        {
            if (OnMoveUnit != null)
                OnMoveUnit(this, new EventArgs());
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
