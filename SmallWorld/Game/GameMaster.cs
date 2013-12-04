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
        public void NewGame(BuilderGameStrategy gameStrategy, BuilderMapStrategy mapStrategy, List<Tuple<string, FactionName>> playersInfo)
		{
			if (CurrentGame != null)
				DestroyGame();

			CurrentGame = gameBuilder.Build(gameStrategy, mapStrategy, playersInfo);
		}


		/// <summary>
		/// Load an existing game (map, players...)
		/// </summary>
        public void LoadGame()
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Launch the game
		/// </summary>
		public void StartGame()
		{
			if (CurrentGame == null)
				throw new NotSupportedException();

			gameBuilder.GenerateAllUnits(CurrentGame);

			CurrentGame.Start();
		}


		/// <summary>
		/// 'Destroy' the game from memory
		/// </summary>
		public void DestroyGame()
		{
			CurrentGame.End();
			CurrentGame = null;
		}
    }
}
