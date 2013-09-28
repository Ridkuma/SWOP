using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface GameMaster
    {
        Game NewGame(string strategy, List<Tuple<string, string>> players);

        Game LoadGame();
    }
}
