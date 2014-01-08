using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace SmallWorld
{
    [Serializable]
    public class Player
    {
		public string Name { get; protected set; }
		public IFaction CurrentFaction { get; protected set; }
		public int Score { get; set; }
		
		private bool isAI = false;
        [NonSerialized] private DispatcherTimer aiTimer;
		private int aiCurrentUnit;
        private static Random random = new Random();

        public Player(string name, FactionName faction)
        {
			this.Name = name;
            this.Score = 0;
			switch (faction)
			{
				case FactionName.Dwarves:
                    this.CurrentFaction = new DwarvesFaction();
					break;

				case FactionName.Gauls:
                    this.CurrentFaction = new GaulsFaction();
					break;

				case FactionName.Vikings:
                    this.CurrentFaction = new VikingsFaction();
					break;

				default:
					throw new NotImplementedException();
			}
			
			if (Name.StartsWith("AI-")) // Artificial Intelligence
			{
				isAI = true;
				aiTimer = new System.Windows.Threading.DispatcherTimer();
				aiTimer.Tick += new EventHandler(aiLogic_Tick);
				aiTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
			}
        }
        
        
        public void MyTurn()
        {
			if (!isAI)
				return;

			aiCurrentUnit = 0;
			aiTimer.Start();
		}
		
		
		private void aiLogic_Tick(object sender, EventArgs e)
        {
			IUnit u = CurrentFaction.Units[aiCurrentUnit];

            IMap map = GameMaster.GM.CurrentGame.MapBoard;
            int remainingFav = 3; // Adviced tiles for next action (max 3)
            List<ITile> remainingFavList = new List<ITile>();

            foreach (ITile t in map.Tiles)
            {
                if (t != u.Position)
                {
                    // Attack tile
                    if (map.CanAttackTo(u.Position, t))
                    {
                        remainingFav--;
                        remainingFavList.Insert(0, t);
                    }
                    // Move tile
                    else if (map.CanMoveTo(u.Position, t))
                    {
                        if (map.IsFavorite(remainingFav, u.Position, t, false, t.IsOccupied()))
                        {
                            remainingFav--;
                            if (remainingFavList.Count > 1)
                                remainingFavList.Insert(1, t);
                            else
                                remainingFavList.Add(t);
                        }
                        else if (!t.IsOccupied())
                        {
                            remainingFavList.Add(t);
                        }
                        else if (remainingFav > 2)
                        {
                            remainingFavList.Add(t);
                        }
                    }
                }
            }

            // Choose a random tile from the 3 favorites ones
            if (remainingFavList.Count > 0)
            {
                int nbSelectableTiles = Math.Min(remainingFavList.Count, 3);
                int selected = random.Next(nbSelectableTiles);
                if (map.CanAttackTo(u.Position, remainingFavList[selected]))
                    u.Attack(remainingFavList[selected].GetBestDefUnit());
                else
                    u.Move(remainingFavList[selected]);
            }

            // End turn if every unit has moved
			aiCurrentUnit++;
			if (aiCurrentUnit >= CurrentFaction.Units.Count)
			{
				aiTimer.Stop();
				GameMaster.GM.CurrentGame.NextPlayer();
			}
		}
    }
}
