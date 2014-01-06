using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Player
    {
		public string Name { get; protected set; }
		public IFaction CurrentFaction { get; protected set; }
		public int Score { get; set; }


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
        }
    }
}
