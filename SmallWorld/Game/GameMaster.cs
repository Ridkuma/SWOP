using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SmallWorld
{
    public class GameMaster
    {
		public static GameMaster GM { get; protected set; }
		public GameBuilder GameBuilder { get; protected set; }
		public IGame CurrentGame { get; protected set; }

        public const string SAVEFILE_PATH = "gamesave.dat";
        private BinaryFormatter formatter;

		/// <summary>
		/// Constructor of singleton GameMaster class
		/// </summary>
		public GameMaster()
		{
			if (GM != null)
				Console.WriteLine("[WARNING] GameMaster must be instancied only once");
			GM = this;

			GameBuilder = new GameBuilder();
            formatter = new BinaryFormatter();
		}


		/// <summary>
		/// Create a new game (map, players...)
		/// </summary>
        public void NewGame(BuilderGameStrategy gameStrategy, BuilderMapStrategy mapStrategy, List<Tuple<string, FactionName>> playersInfo)
		{
			if (CurrentGame != null)
				DestroyGame();

			CurrentGame = GameBuilder.Build(gameStrategy, mapStrategy, playersInfo);
		}


        /// <summary>
        /// Save current game (map, players...)
        /// </summary>
        public void SaveGame()
        {
            if (CurrentGame.GetType() != typeof(LocalGame)) // Only save local game
                return;

            try
            {
                // Create a FileStream that will write data to file.
                FileStream fileStream = new FileStream(SAVEFILE_PATH, FileMode.Create, FileAccess.Write);
                formatter.Serialize(fileStream, (LocalGame)CurrentGame);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to save game data : [" + e.Source + "]" + e.Message);
                if (File.Exists(SAVEFILE_PATH))
                    File.Delete(SAVEFILE_PATH);
            }
        }


		/// <summary>
		/// Load last saved game (map, players...)
		/// </summary>
        public void LoadGame()
		{
            if (CurrentGame != null)
                DestroyGame();
            
            if (File.Exists(SAVEFILE_PATH))
            {
                try
                {
                    // Read data file.
                    FileStream fileStream = new FileStream(SAVEFILE_PATH, FileMode.Open, FileAccess.Read);
                    CurrentGame = (LocalGame) formatter.Deserialize(fileStream);
                    fileStream.Close();
                    CurrentGame.MapBoard.RefreshAlgoMap();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Unable to read game data : [" + e.Source + "]" + e.Message);
                }
            }
		}


        public void FinishLoadGame()
        {
            if (CurrentGame != null)
                CurrentGame.OnRaiseStartGame();
        }

		/// <summary>
		/// 'Destroy' the game from memory
		/// </summary>
		public void DestroyGame()
		{
			if (CurrentGame == null)
				return;

			CurrentGame.End();
			CurrentGame = null;
		}
    }
}
