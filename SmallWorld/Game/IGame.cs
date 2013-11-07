using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public enum GameState
	{
		Idle,
		Loading,
		Saving,
		Playing,
		Pausing,
		Ending,
	}

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

        IGame(Map map, List<Player> players);
    
        void Pause();

        void Resume();

        void Launch();

        void Save();
    }
}
