using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
    {
		IMap MapBoard { get; }
		List<Player> Players { get; }
		int CurrentPlayerId { get; }
		int CurrentTurn { get; }

		event EventHandler<EventArgs> OnStartGame;
		event EventHandler<EventArgs> OnNextPlayer;
		event EventHandler<EventArgs> OnEndGame;

		void Start();
        void NextPlayer();
        void Save();
		void End();
    }
}
