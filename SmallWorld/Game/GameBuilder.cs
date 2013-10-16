using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class GameBuilder
    {
        public IGame Build()
		{
			IGame game = new SoloGame();

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
					Console.WriteLine("[Error in GameBuilder.BuildMap()] Strategy '" + strategy + "' for building map");
					return null;
			}

			Map map = mapBuilder.Build();

			return map;
		}

		public List<Player> BuildPlayers(List<Tuple<string, string>> players)
		{
			throw new NotImplementedException();
		}

		public void PlaceUnits()
		{
			throw new NotImplementedException();
		}
    }
}
