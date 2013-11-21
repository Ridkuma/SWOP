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
		public int Score { get; protected set; }


        public Player(string name, FactionName faction)
        {
			Name = name;
			switch (faction)
			{
				case FactionName.Dwarves:
					Faction = new DwarvesFaction();
					break;

				case FactionName.Gauls:
					Faction = new GaulsFaction();
					break;

				case FactionName.Vikings:
					Faction = new VikingsFaction();
					break;

				default:
					throw new NotImplementedException();
			}
        }
    }
}
