using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Player
    {
        public string Name
        {
            get;
            protected set;
        }

        public IFaction Faction
        {
            get;
            protected set;
        }

        public int Score
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Player(string name, FactionName faction)
        {
            throw new System.NotImplementedException();
        }
    }
}
