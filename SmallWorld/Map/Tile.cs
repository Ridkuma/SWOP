using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Interface to access Tile properties
    /// </summary>
    public interface ITile
    {
        int X { get; }
        int Y { get; }
        TileType Type { get; }
        List<ITile> AdjacentsTiles { get; }
        List<IUnit> OccupyingUnits { get; }
        BonusType Bonus { get; }

        void AddAdjacentTile(ITile adjacentTile);
        bool IsAdjacent(ITile comparedTile);
		bool IsOccupied();
		bool IsOccupiedByFriend(IUnit comparedUnit);
		bool IsOccupiedByEnnemy(IUnit comparedUnit);
        void UnitEnter(IUnit unit);
        void UnitLeave(IUnit unit);
        IUnit GetBestDefUnit();
    }


    /// <summary>
    /// Private implementation of Tile
    /// </summary>
    [Serializable]
    public class Tile : ITile
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public virtual TileType Type
        {
            get { throw new NotImplementedException(); }
        }
        public List<ITile> AdjacentsTiles { get; protected set; }
        public List<IUnit> OccupyingUnits { get; protected set; }
        public BonusType Bonus { get; protected set; }


        protected Tile(int posX, int posY)
        {
            X = posX;
            Y = posY;

            AdjacentsTiles = new List<ITile>();
            OccupyingUnits = new List<IUnit>();
            
            InitBonus();
        }


        public void AddAdjacentTile(ITile adjTile)
        {
            if (AdjacentsTiles.Contains(adjTile))
                return;
            AdjacentsTiles.Add(adjTile);
        }


        public bool IsAdjacent(ITile comparedTile)
        {
            return AdjacentsTiles.Contains(comparedTile);
        }


        public bool IsOccupied()
        {
            return (OccupyingUnits.Count > 0);
        }


		public bool IsOccupiedByFriend(IUnit comparedUnit)
		{
			if (!IsOccupied())
				return false;

			foreach (Player p in GameMaster.GM.CurrentGame.Players)
				if (p.CurrentFaction.Units.Contains(comparedUnit))
					foreach (IUnit u in OccupyingUnits)
						if (!p.CurrentFaction.Units.Contains(u))
							return false;

			return true;
		}


		public bool IsOccupiedByEnnemy(IUnit comparedUnit)
		{
			return (IsOccupied() && !IsOccupiedByFriend(comparedUnit));
		}


        public void UnitEnter(IUnit unit)
        {
            if (!OccupyingUnits.Contains(unit))
                OccupyingUnits.Add(unit);
            else
                Console.WriteLine("[Warning] Unit trying to enter on a tile already occupied");
        }
		

        public void UnitLeave(IUnit unit)
        {
            if (OccupyingUnits.Contains(unit))
                OccupyingUnits.Remove(unit);
            else
                Console.WriteLine("[Warning] Unit trying to quit a tile not currently occupied");
        }

        public IUnit GetBestDefUnit()
        {
            int maxDef = -1;
            IUnit bestDefUnit = null;
            foreach (IUnit unit in this.OccupyingUnits)
            {
                if (unit.Def > maxDef)
                {
                    bestDefUnit = unit;
                    maxDef = unit.Def;
                }
            }

            return bestDefUnit;
        }
        
        
        private void InitBonus()
        {
            Bonus = BonusType.None;

            Random rnd = new Random();
            if (rnd.Next(10) > 7)
            {
                Bonus = (BonusType) rnd.Next(Enum.GetValues(typeof(BonusType)).Length);
            }
        }
    }


    [Serializable]
    public class FieldTile : Tile
    {
        public override TileType Type
        {
            get { return TileType.Field; }
        }

        public FieldTile(int x, int y) 
            : base(x, y)
        {
        }
    }

    [Serializable]
    public class MountainTile : Tile
    {
        public override TileType Type
        {
            get { return TileType.Mountain; }
        }

        public MountainTile(int x, int y) 
            : base(x, y)
        {
        }
    }

    [Serializable]
    public class DesertTile : Tile
    {
        public override TileType Type
        {
            get { return TileType.Desert; }
        }

        public DesertTile(int x, int y) 
            : base(x, y)
        {
        }
    }

    [Serializable]
    public class ForestTile : Tile
    {
        public override TileType Type
        {
            get { return TileType.Forest; }
        }

        public ForestTile(int x, int y) 
            : base(x, y)
        {
        }
    }

    [Serializable]
    public class WaterTile : Tile
    {
        public override TileType Type
        {
            get { return TileType.Water; }
        }

        public WaterTile(int x, int y) 
            : base(x, y)
        {
        }
    }
    
    
    public enum BonusType {
        None,
        DoublePoint,
        DoubleAtkDef,
        NoPoint,
    }
}
