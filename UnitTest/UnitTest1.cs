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


        [TestMethod]
        public void TestCreateServerGame()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("ServerPlayer", FactionName.Vikings));

            GM.NewGame(BuilderGameStrategy.Server, BuilderMapStrategy.Demo, listFaction);

            Assert.IsNotNull(GM.CurrentGame);
            Assert.IsNotNull(GM.CurrentGame.MapBoard);
            Assert.AreEqual(GM.CurrentGame.Players.Count, 1);
        }


        [TestMethod]
        public void TestStartGame_VikingDwarves_DemoMap()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction);

            GM.CurrentGame.Start();

            Assert.IsNotNull(GM.CurrentGame.Players[0].CurrentFaction.Units);
            Assert.IsNotNull(GM.CurrentGame.Players[1].CurrentFaction.Units);
        }


        [TestMethod]
        public void TestStartGame_GaulsDwarves_SmallMap()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Gauls));
            listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Small, listFaction);
            GM.CurrentGame.Start();

            Assert.IsNotNull(GM.CurrentGame.Players[0].CurrentFaction.Units);
            Assert.IsNotNull(GM.CurrentGame.Players[1].CurrentFaction.Units);
        }


        [TestMethod]
        public void TestStartGame_GaulsVikings_NormalMap()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Gauls));
            listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Vikings));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Normal, listFaction);
            GM.CurrentGame.Start();

            Assert.IsNotNull(GM.CurrentGame.Players[0].CurrentFaction.Units);
            Assert.IsNotNull(GM.CurrentGame.Players[1].CurrentFaction.Units);
        }


        [TestMethod]
        public void TestStartGame_WithAI()
        {
            GameMaster GM = new GameMaster();

            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("AI-TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("AI-LegolasDu93", FactionName.Dwarves));

            GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction);

            GM.CurrentGame.Start();

            Assert.IsNotNull(GM.CurrentGame.Players[0].CurrentFaction.Units);
            Assert.IsNotNull(GM.CurrentGame.Players[1].CurrentFaction.Units);
        }


        public GameMaster CreateLocalGame(int gameScenario = 0)
        {
            GameMaster GM = new GameMaster();
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            BuilderMapStrategy mapStrategy;

            switch (gameScenario)
            {
                case 0:
                    listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
                    listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));
                    mapStrategy = BuilderMapStrategy.Demo;
                    break;
                case 1:
                    listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Gauls));
                    listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Dwarves));
                    mapStrategy = BuilderMapStrategy.Small;
                    break;
                default:
                    listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Gauls));
                    listFaction.Add(new Tuple<string, FactionName>("LegolasDu93", FactionName.Vikings));
                    mapStrategy = BuilderMapStrategy.Normal;
                    break;
            }
            GM.NewGame(BuilderGameStrategy.Local, mapStrategy, listFaction);

            return GM;
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
            GM.FinishLoadGame();

            Assert.IsNotNull(GM.CurrentGame);
        }


        [TestMethod]
        public void TestEndGame()
        {
            GameMaster GM = CreateLocalGame();
            GM.CurrentGame.Start();

            GM.CurrentGame.End();
            GM.DestroyGame();

            Assert.IsNull(GM.CurrentGame);
        }


        [TestMethod]
        public void TestNextPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                GameMaster GM = CreateLocalGame(i);

                GM.CurrentGame.Start();

                Assert.AreEqual(GM.CurrentGame.CurrentPlayerId, 0);

                GM.CurrentGame.NextPlayer();

                Assert.AreEqual(GM.CurrentGame.CurrentPlayerId, 1);
            }
        }


        [TestMethod]
        public void TestGetScore()
        {
            GameMaster GM = CreateLocalGame();

            GM.CurrentGame.Start();

            Assert.AreEqual(GM.CurrentGame.Players[0].Score, 0);
            Assert.AreEqual(GM.CurrentGame.Players[1].Score, 0);

            int scorePlayer1 = GM.CurrentGame.Players[0].Score;
            int scorePlayer2 = GM.CurrentGame.Players[1].Score;

            GM.CurrentGame.NextPlayer();

            Assert.AreNotEqual(GM.CurrentGame.Players[0].Score + GM.CurrentGame.Players[1].Score, scorePlayer1 + scorePlayer2);
        }


        [TestMethod]
        public void TestMoveUnit()
        {
            for (int i = 0; i < 3; i++)
            {
                GameMaster GM = CreateLocalGame(i);

                GM.CurrentGame.Start();

                IUnit unit = GM.CurrentGame.Players[GM.CurrentGame.CurrentPlayerId].CurrentFaction.Units[0];
                ITile tileSource = unit.Position;
                ITile tileDestination = tileSource.AdjacentsTiles[0];
                foreach (ITile t in tileSource.AdjacentsTiles)
                {
                    if (GM.CurrentGame.MapBoard.CanMoveTo(tileSource, t))
                    {
                        tileDestination = t;
                        break;
                    }
                }
                unit.Move(tileDestination);

                Assert.AreEqual(unit.Position, tileDestination);
            }
        }


        [TestMethod]
        public void TestAttackUnit()
        {
            for (int i = 0; i < 3; i++)
            {
                GameMaster GM = CreateLocalGame(i);

                GM.CurrentGame.Start();

                IUnit unit = GM.CurrentGame.Players[GM.CurrentGame.CurrentPlayerId].CurrentFaction.Units[0];
                ITile tileSource = unit.Position;
                ITile tileDestination = tileSource.AdjacentsTiles[0];
                foreach (ITile t in tileSource.AdjacentsTiles)
                {
                    if (GM.CurrentGame.MapBoard.CanMoveTo(tileSource, t))
                    {
                        tileDestination = t;
                        break;
                    }
                }

                // Artificially move an enemy unit near the selected unit
                IUnit unitEnemy = GM.CurrentGame.Players[GM.CurrentGame.CurrentPlayerId + 1].CurrentFaction.Units[0];
                unitEnemy.RealMove(tileDestination);

                Assert.IsTrue(GM.CurrentGame.MapBoard.CanAttackTo(tileSource, tileDestination));

                int totalHp = unit.Hp + unitEnemy.Hp;

                unit.Attack(unitEnemy);

                Assert.AreNotEqual(totalHp, unit.Hp + unitEnemy.Hp);
            }
        }


    }
}
