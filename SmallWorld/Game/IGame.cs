using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
    {
		List<Player> Players { get; }
		int CurrentTurn { get; }
		IMap MapBoard { get; }
		int CurrentPlayerId { get; }

        void NextPlayer();
        void Save();
		void End();

		event EventHandler<EventArgs> OnNextPlayer;
		event EventHandler<EventArgs> OnNextTurn;
		event EventHandler<EventArgs> OnEndGame;
    }
}
