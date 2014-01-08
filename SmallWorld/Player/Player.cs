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
		private DispatcherTimer aiTimer;
		private int aiCurrentUnit;


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
			
			if (Name.StartsWith("ai")) // Artificial Intelligence
			{
				isAI = true;
				aiTimer = new System.Windows.Threading.DispatcherTimer();
				aiTimer.Tick += new EventHandler(aiLogic_Tick);
				aiTimer.Interval = new TimeSpan(0, 0, 0, 1);
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
			
			
			aiCurrentUnit++;
			if (aiCurrentUnit >= CurrentFaction.Units.Count)
			{
				aiTimer.Stop();
				GameMaster.GM.CurrentGame.NextPlayer();
			}
		}
    }
}
