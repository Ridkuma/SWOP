using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public enum BuilderGameStrategy
	{
		Local,
		Server,
		Client,
	}

	public enum BuilderMapStrategy
	{
		Demo,
		Small,
		Normal,
	}

    public class GameBuilder
    {
        public IGame Build(BuilderGameStrategy gameStrategy, BuilderMapStrategy mapStrategy, List<Tuple<string, FactionName>> playersInfo)
		{
			IMap map = BuildMap(mapStrategy);
            List<Player> players = BuildPlayers(playersInfo);
			IGame game = null;

			switch (gameStrategy)
			{
				case BuilderGameStrategy.Local:
					game = new LocalGame(map, players);
					break;

				case BuilderGameStrategy.Server:
					game = new ServerGame(map, players);
					break;

				case BuilderGameStrategy.Client:
					game = new ClientGame(map, players);
					break;

				default:
					throw new NotImplementedException();
			}
			return game;
		}

		public Map BuildMap(BuilderMapStrategy mapStrategy)
		{
			IMapBuilder mapBuilder;
			switch (mapStrategy)
			{
				case BuilderMapStrategy.Demo:
					mapBuilder = new DemoMapBuilder();
					break;

				case BuilderMapStrategy.Small:
					mapBuilder = new SmallMapBuilder();
					break;

				case BuilderMapStrategy.Normal:
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
