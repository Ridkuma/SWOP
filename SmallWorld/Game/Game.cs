using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface Game
    {
        void Pause();

        void Resume();

        void Launch();

        void Save();
    }
}
