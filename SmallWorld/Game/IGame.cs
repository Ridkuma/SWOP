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
		Map MapBoard { get; }
		int CurrentPlayerId { get; }


        void NextPlayer();

        void Save();

        void End();
    }
}
