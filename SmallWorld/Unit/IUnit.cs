using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWorld
{
    public interface IUnit
    {
        ITile Position { get; } // A position code, or a tile ?
        int Atk { get; } // Attack Points
        int Def { get; } // Defense Points
        int Hp { get; } // Health Points
        int Mvt { get; } // Movement Points
        string Name { get; } // Unit name
        UnitState State { get; } // Unit State
        FactionName Faction { get; } // Unit's Faction Name

        bool CheckMove(ITile destination);
        void Move(ITile destination);
        void Attack(IUnit enemy);
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
