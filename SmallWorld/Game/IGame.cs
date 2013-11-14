using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
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

        int CurrentPlayerId
        {
            get;
            set;
        }

        void NextPlayer();

        void Save();

        void End();
    }
}
