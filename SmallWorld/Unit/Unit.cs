using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Unit : IUnit
    {
        protected ITile Position { get; set; }
        protected int Atk { get; set; }
        protected int Def { get; set; }
        protected int Hp { get; set; }
        protected int Mvt { get; set; }
        protected string Name { get; set; }

        public Unit(string name)
        {
            this.Atk = 2;
            this.Def = 1;
            this.Hp = 2;
            this.Mvt = 1;
            this.Name = name;
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
                
            this.Position.OccupyingUnit = null;
            this.Position = destination;
            destination.OccupyingUnit = this;
        }

        // Attacking is the same for every faction
        public void Attack(IUnit enemy)
        {
            // TODO
        }
    }

    public class VikingsUnit : Unit
    {
        public VikingsUnit(string name) 
            : base(name)
        {
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
