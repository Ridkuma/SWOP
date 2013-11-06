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
		int X
		{
			get;
		}
		int Y
		{
			get;
		}
		TileType Type
		{
			get;
		}
		List<ITile> AdjacentsTiles
		{
			get;
		}
		Unit OccupyingUnit
		{
			get;
			set;
		}

		void AddAdjacentTile(ITile adjacentTile);
		bool IsAdjacent(ITile comparedTile);
		bool IsOccupied();
	}


	/// <summary>
	/// Private implementation of Tile
	/// </summary>
	public class Tile : ITile
	{
		protected int x;
		public int X
		{
			get
			{
				return x;
			}
		}

		protected int y;
		public int Y
		{
			get
			{
				return y;
			}
		}

		public virtual TileType Type
		{
			get
			{
				return TileType.Field; // (Replace by an error ?)
			}
		}

		private List<ITile> adjacentsTiles;
		public List<ITile> AdjacentsTiles
		{
			get
			{
				return adjacentsTiles;
			}
		}

		protected Unit occupyingUnit;
		public Unit OccupyingUnit
		{
			get
			{
				return occupyingUnit;
			}
			set
			{
				occupyingUnit = value;
			}
		}


		protected Tile(int posX, int posY)
		{
			x = posX;
			y = posY;

			adjacentsTiles = new List<ITile>();
		}


		public void AddAdjacentTile(ITile adjTile)
		{
			if (adjacentsTiles.Contains(adjTile))
				return;
			adjacentsTiles.Add(adjTile);
		}


		public bool IsAdjacent(ITile comparedTile)
		{
			return adjacentsTiles.Contains(comparedTile);
		}


		public bool IsOccupied()
		{
			return (OccupyingUnit != null);
		}
	}


	public class FieldTile : Tile
	{
		public override TileType Type
		{
			get
			{
				return TileType.Field;
			}
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
			get
			{
				return TileType.Mountain;
			}
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
			get
			{
				return TileType.Desert;
			}
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
			get
			{
				return TileType.Forest;
			}
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
			get
			{
				return TileType.Water;
			}
		}

		public WaterTile(int x, int y) 
            : base(x, y)
        {
        }
    }
}
