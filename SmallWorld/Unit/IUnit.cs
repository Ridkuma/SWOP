using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWorld
{
    public interface IUnit
    {
        ITile Position { get; } // Tile
        int Atk { get; } // Attack Points
        int Def { get; } // Defense Points
        int Hp { get; set; } // Health Points // TODO : Would be cleaner to remove the setter ("friend"-like relationship IUnit/IGame ?)
        int HpMax { get; } // Maximum Heath Points
        int Mvt { get; } // Movement Points
        string Name { get; } // Unit name
        UnitState State { get; } // Unit State
        FactionName Faction { get; } // Unit's Faction Name

        bool CheckMove(ITile destination);
		void Move(ITile destination);
		void RealMove(ITile destination);
        bool CheckAttack(IUnit enemy);
        void Attack(IUnit enemy);
        void RealAttack(IUnit enemy);
        void ChangeState(UnitState targetState);
        void EndTurn();
    }

    public enum UnitState
    {
        Idle,
        Selected,
        Move,
        Attacking,
        Defending,
        Dead
    }
}
