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
        void UnitEnter(IUnit unit);
        void UnitLeave(IUnit unit);
    }


    /// <summary>
    /// Private implementation of Tile
    /// </summary>
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
