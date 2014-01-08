using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateLocalGame()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction);

            Assert.IsNotNull(GM.CurrentGame);
            Assert.IsNotNull(GM.CurrentGame.MapBoard);
            Assert.AreEqual(GM.CurrentGame.Players.Count, 2);

        }


        public GameMaster CreateLocalGame()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction);

            return GM;
        }


        [TestMethod]
        public void TestStartLocalGame()
        {
            GameMaster GM = CreateLocalGame();

            GM.CurrentGame.Start();

            Assert.IsNotNull(GM.CurrentGame.Players[0].CurrentFaction.Units);
            Assert.IsNotNull(GM.CurrentGame.Players[1].CurrentFaction.Units);
        }


        [TestMethod]
        public void TestSaveGame()
        {
            GameMaster GM = CreateLocalGame();
            GM.CurrentGame.Start();

            if (File.Exists(GameMaster.SAVEFILE_PATH))
                File.Delete(GameMaster.SAVEFILE_PATH);

            GM.SaveGame();

            Assert.IsTrue(File.Exists(GameMaster.SAVEFILE_PATH));
        }


        [TestMethod]
        public void TestLoadGame()
        {
            GameMaster GM;

            if (!File.Exists(GameMaster.SAVEFILE_PATH))
            {
                GM = CreateLocalGame();
                GM.SaveGame();
                Assert.IsTrue(File.Exists(GameMaster.SAVEFILE_PATH)); // Save is not working
                GM.DestroyGame();
                Assert.IsNull(GM.CurrentGame);
            }
            else
            {
                GM = new GameMaster();
            }

            GM.LoadGame();

            Assert.IsNotNull(GM.CurrentGame);
        }
    }
}
