using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Unit : IUnit
    {
        public ITile Position { get; set; }
        public int Atk { get; protected set; }
        public int Def { get; protected set; }
        public int Hp { get; protected set; }
        public int Mvt { get; protected set; }
        public string Name { get; protected set; }
        public UnitState State { get; set; }
		public FactionName Faction { get; protected set; }


        public Unit(string name)
        {
            this.Atk = 2;
            this.Def = 1;
            this.Hp = 2;
            this.Mvt = 1;
            this.Name = name;
            this.State = UnitState.Idle;
        }

        // DEFAULT MOVE : Consider this "pseudo code" for now
        // A Unit can only move one tile per round
        public virtual void Move(ITile destination)
        {
            // Minimum verifications before allowing a move,
            // every Unit extension should add up their own limitations
            bool possibleMove = (this.Mvt > 0)
                && (!destination.IsOccupied());

            if (!possibleMove)
                return;

            this.Position.UnitLeave(this);
            this.Position = destination;
            destination.UnitEnter(this);
        }

        // Attacking is the same for every faction
        public void Attack(IUnit enemy)
        {
            // TODO
        }


        public void ChangeState(UnitState targetState)
        {
            switch (this.State)
            {
                case UnitState.Idle :
                    switch (targetState)
                    {
                        case UnitState.Selected :
                            this.State = targetState;
                            break;

                        case UnitState.Defending :
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Selected :
                    switch (targetState)
                    {
                        case UnitState.Move :
                            this.State = targetState;
                            break;

                        case UnitState.Attacking :
                            this.State = targetState;
                            break;

                        case UnitState.Idle :
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Move:
                    switch (targetState)
                    {
                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                    }
                    break;

                case UnitState.Attacking:
                    switch (targetState)
                    {
                        case UnitState.Move:
                            this.State = targetState;
                            break;

                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                        case UnitState.Dead:
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Defending:
                    switch (targetState)
                    {
                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                        case UnitState.Dead:
                            this.State = targetState;
                            break;
                    }
                    break;

            }
        }

    }

    public class VikingsUnit : Unit
    {
        public VikingsUnit(string name) 
            : base(name)
        {
        }

        public FactionName Faction
        {
            get { return FactionName.Vikings; }
        }

        public override void Move(ITile destination)
        {
            if (!destination.IsAdjacent(this.Position))
                return;
            base.Move(destination);
            this.Mvt--;
        }

    }

    public class DwarvesUnit : Unit
    {
        public DwarvesUnit(string name) 
            : base(name)
        {
        }

        public FactionName Faction
        {
            get { return FactionName.Dwarves; }
        }

        // A Dwarf can move from a Mountain to any another Mountain
        // Cannot cross Water
        public override void Move(ITile destination)
        {
            bool mountainTravel = !destination.IsAdjacent(this.Position)
                && destination.Type != TileType.Mountain
                && this.Position.Type != TileType.Mountain;

            if (!mountainTravel || destination.Type == TileType.Water)
                return;
            base.Move(destination);
            this.Mvt--;
        }
    }

    public class GaulsUnit : Unit
    {
        public GaulsUnit(string name) 
            : base(name)
        {
            this.Mvt = 2;
        }

        public FactionName Faction
        {
            get { return FactionName.Gauls; }
        }

        public void Move(ITile destination)
        {
            if (!destination.IsAdjacent(this.Position) || destination.Type == TileType.Water)
                return;
            base.Move(destination);
            if (destination.Type == TileType.Field)
                this.Mvt -= 1;
            else
                this.Mvt -= 2;
        }
    }
}
