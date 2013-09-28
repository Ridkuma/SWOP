using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface GameBuilder
    {
        Game Build();

        Map BuildMap(string strategy);

        List<Player> BuildPlayers(List<Tuple<string, string>> players);

        void PlaceUnits();
    }
}
