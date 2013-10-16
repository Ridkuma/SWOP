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

		private IGame currentGame;
		private Map currentMap;


		/// <summary>
		/// Constructor of singleton GameMaster class
		/// </summary>
		public GameMaster()
		{
			if (GM != null)
				Console.WriteLine("[ERROR] GameMaster must be instancied only once");
			GM = this;

			gameBuilder = new GameBuilder();

			// tmp
			NewGame("small");//, onmetquoidéjàici??)
		}


		/// <summary>
		/// Create a new game (map, players...)
		/// </summary>
        void NewGame(string strategy)//, List<Tuple<string, string>> players)
		{
			currentGame = gameBuilder.Build();
			currentMap = gameBuilder.BuildMap(strategy);
		}

		/// <summary>
		/// Load an existing game (map, players...)
		/// </summary>
        void LoadGame()
		{
			throw new NotImplementedException();
		}

    }
}
