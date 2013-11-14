using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWorld
{
    interface IUnit
    {
        ITile Position { get; set; } // A position code, or a tile ?
        int Atk { get; set; } // Attack Points
        int Def { get; set; } // Defense Points
        int Hp { get; set; } // Health Points
        int Mvt { get; set; } // Movement Points
        string Name { get; set; } // Unit name

        void Move(ITile destination);
        void Attack(IUnit enemy);
    }
}
