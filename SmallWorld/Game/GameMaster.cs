using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class GameMaster
    {
		public static GameMaster GM;
		
		private GameBuilder gameBuilder;

		public IGame CurrentGame
		{
			get;
			protected set;
		}

		public Map CurrentMap
		{
			get;
			protected set;
		}


		/// <summary>
		/// Constructor of singleton GameMaster class
		/// </summary>
		public GameMaster()
		{
			if (GM != null)
				Console.WriteLine("[WARNING] GameMaster must be instancied only once");
			GM = this;

			gameBuilder = new GameBuilder();
		}


		/// <summary>
		/// Create a new game (map, players...)
		/// </summary>
        public void NewGame(string strategy)//, List<Tuple<string, string>> players)
		{
			CurrentGame = gameBuilder.Build();
			CurrentMap = gameBuilder.BuildMap(strategy);
		}


		/// <summary>
		/// Load an existing game (map, players...)
		/// </summary>
        public void LoadGame()
		{
			throw new NotImplementedException();
		}

    }
}
