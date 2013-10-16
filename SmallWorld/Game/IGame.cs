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
        void Pause();

        void Resume();

        void Launch();

        void Save();
    }
}
