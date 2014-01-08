using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
    {
		IMap MapBoard { get; }
		int CurrentTurn { get; }
		List<Player> Players { get; }
		int CurrentPlayerId { get; }
		bool CurrentPlayerIsMe { get; }
        Player Victor { get; }

        Player GetCurrentPlayer();
		void Start(bool generateUnits = true);
        void NextPlayer();
		void MoveUnit(IUnit unit, ITile destination);
        void AttackUnit(IUnit unit, IUnit enemy);
        double ToHitChance(IUnit attacker, IUnit defender);
        void Fight(IUnit attacker, IUnit defender);
        void Save();
		void End();

		event EventHandler<EventArgs> OnStartGame;
		event EventHandler<EventArgs> OnNextPlayer;
		event EventHandler<EventArgs> OnEndGame;
        event EventHandler<EventArgs> OnMoveUnit;
        event EventHandler<EventArgs> OnAttackUnit;
		event EventHandler<StringEventArgs> OnNewChatMessage;

        void OnRaiseStartGame();
		void OnRaiseNewChatMessage(string text);

    }

	public class StringEventArgs : EventArgs
	{
		public string Text { get; protected set; }

		public StringEventArgs(string text)
		{
			Text = text;
		}
	}
}
