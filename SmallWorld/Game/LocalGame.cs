using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    [Serializable]
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
        public Player Victor { get; protected set; }

        /// <summary>
        /// Random numbers generator
        /// </summary>
        private static Random RAND = new Random();

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

            Players[CurrentPlayerId].MyTurn();
            OnRaiseStartGame();
		}

        /// <summary>
        /// Check if a player has been destroyed
        /// </summary>
        /// <returns>True if a player's faction was decimated</returns>
        public bool IsAnyPlayerOverkilled()
        {
            foreach (Player player in this.Players)
            {
                if (player.CurrentFaction.IsDecimated())
                    return true;
            }

            return false;
        }

		/// <summary>
		/// End of player turn, possible outcomes : next player, next game turn or end of game
		/// </summary>
		public virtual void NextPlayer()
		{
            // Update player score
            if (CurrentPlayerId < Players.Count)
            {
                int turnScore = Players[CurrentPlayerId].CurrentFaction.GetTurnScore();
                Players[CurrentPlayerId].Score += turnScore;
            }

			CurrentPlayerId++;

            if (this.IsAnyPlayerOverkilled())
            {
                End();
                return;
            }

			if (CurrentPlayerId >= Players.Count)
			{
				if (CurrentTurn >= MapBoard.TotalNbTurn) // Is end of total turns => game over
				{
                    CurrentPlayerId = 0;
                    End();
				}
				else
				{
					CurrentPlayerId = 0;
                    CurrentTurn++;
                    if (CurrentPlayerIsMe)
                        Players[CurrentPlayerId].MyTurn();
					OnRaiseNextPlayer();
				}
			}
			else
            {
                if (CurrentPlayerIsMe)
                    Players[CurrentPlayerId].MyTurn();
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
        /// Process attacking from a Unit to another
        /// </summary>
        public virtual void AttackUnit(IUnit unit, ITile destinaton)
        {
            unit.RealAttack(destinaton.GetBestDefUnit());
            OnRaiseAttackUnit();
        }

        /// <summary>
        /// Computes the chance percentage for the attacker to hit the defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public double ToHitChance(IUnit attacker, IUnit defender)
        {
            if (defender.Def == 0)
            {
                return 100d;
            }

            double realAtk = (double)attacker.Atk * ((double)attacker.Hp / attacker.HpMax);
            double realDef = (double)defender.Def * ((double)defender.Hp / defender.HpMax);

            double toHit;
            if (realAtk >= realDef)
                toHit = 0.5d + 0.5d * (1d - ((double)realDef / realAtk));
            else
                toHit = 1d - (0.5d + 0.5d * (1d - ((double)realAtk / realDef)));

            toHit *= 100d;

            return toHit;
        }

        /// <summary>
        /// Handles the actual fight between two Units
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public void Fight(IUnit attacker, IUnit defender)
        {
            if (defender.Def == 0)
            {
                defender.Hp = 0;
                return;
            }

            int maxHp = Math.Max(attacker.Hp, defender.Hp);
            int turns = RAND.Next(3, maxHp + 3);
            while (turns > 0 && attacker.Hp > 0 && defender.Hp > 0)
            {
                double toHit = this.ToHitChance(attacker, defender);
                int hit = RAND.Next(101);
                if (hit <= toHit || LocalPlayerId != -1)
                    defender.Hp--;
                else
                    attacker.Hp--;

                turns--;
            }
        }

		/// <summary>
		/// End of the game
		/// </summary>
		public virtual void End()
		{
            int maxScore = -1;

            foreach (Player player in this.Players)
            {
                if (!player.CurrentFaction.IsDecimated() && player.Score > maxScore)
                {
                    this.Victor = player;
                    maxScore = player.Score;
                }
            }

			OnRaiseEndGame();
        }


		/// <summary>
		/// Events
		/// </summary>
		#region Events

        [field:NonSerializedAttribute()] 
        public event EventHandler<EventArgs> OnStartGame;
		public virtual void OnRaiseStartGame()
		{
			if (OnStartGame != null)
				OnStartGame(this, new EventArgs());
		}

        [field:NonSerializedAttribute()] 
		public event EventHandler<EventArgs> OnNextPlayer;
		protected virtual void OnRaiseNextPlayer()
		{
			if (OnNextPlayer != null)
				OnNextPlayer(this, new EventArgs());
		}

        [field:NonSerializedAttribute()] 
		public event EventHandler<EventArgs> OnEndGame;
		protected virtual void OnRaiseEndGame()
		{
			if (OnEndGame != null)
				OnEndGame(this, new EventArgs());
		}

        [field:NonSerializedAttribute()] 
        public event EventHandler<EventArgs> OnMoveUnit;
        protected virtual void OnRaiseMoveUnit()
        {
            if (OnMoveUnit != null)
                OnMoveUnit(this, new EventArgs());
        }

        [field:NonSerializedAttribute()] 
        public event EventHandler<EventArgs> OnAttackUnit;
        protected virtual void OnRaiseAttackUnit()
        {
            if (OnAttackUnit != null)
                OnAttackUnit(this, new EventArgs());
        }

        [field:NonSerializedAttribute()] 
		public event EventHandler<StringEventArgs> OnNewChatMessage;
		public virtual void OnRaiseNewChatMessage(string text)
		{
			if (OnNewChatMessage != null)
				OnNewChatMessage(this, new StringEventArgs(text));
		}

		#endregion


	}
}
