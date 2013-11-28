using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class GameBuilder
    {
        public IGame Build(string strategy, List<Tuple<string, FactionName>> playersInfo)
		{
            IMap map = BuildMap(strategy);
            List<Player> players = BuildPlayers(playersInfo);

			IGame game = new LocalGame(map, players);

			GenerateAllUnits(game);

			return game;
		}

		public Map BuildMap(string strategy)
		{
			IMapBuilder mapBuilder;
			switch (strategy)
			{
				case "demo":
					mapBuilder = new DemoMapBuilder();
					break;

				case "small":
					mapBuilder = new SmallMapBuilder();
					break;

				case "normal":
					mapBuilder = new NormalMapBuilder();
					break;

				default:
                    throw new NotImplementedException();
			}

			Map map = mapBuilder.Build();

			return map;
		}

		public List<Player> BuildPlayers(List<Tuple<string, FactionName>> playersInfo)
		{
            List<Player> players = new List<Player>();

            foreach (Tuple<string, FactionName> pi in playersInfo)
			{
                players.Add(new Player(pi.Item1, pi.Item2));
            }

            return players;
		}

        public void GenerateAllUnits(IGame game)
		{
			foreach (Player p in game.Players)
				p.CurrentFaction.GenerateUnits(game.MapBoard.TotalNbUnits);
		}
    }
}
