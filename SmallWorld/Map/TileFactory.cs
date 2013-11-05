using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class TileFactory
    {
		public ITile CreateTile(int x, int y, int tileType)
		{
			TileType tile = (TileType) tileType;

			switch(tile)
			{
				case TileType.Field:
					return CreateFieldTile(x, y);
				case TileType.Mountain:
					return CreateMountainTile(x, y);
				case TileType.Desert:
					return CreateDesertTile(x, y);
				case TileType.Forest:
					return CreateForestTile(x, y);
				case TileType.Water:
					return CreateWaterTile(x, y);

				default: // (Replace by an error ?)
					return CreateFieldTile(x, y);
			}
		}

        FieldTile CreateFieldTile(int x, int y)
		{
			return new FieldTile(x, y);
		}

		MountainTile CreateMountainTile(int x, int y)
		{
			return new MountainTile(x, y);
		}

		DesertTile CreateDesertTile(int x, int y)
		{
			return new DesertTile(x, y);
		}

		ForestTile CreateForestTile(int x, int y)
		{
			return new ForestTile(x, y);
		}

		WaterTile CreateWaterTile(int x, int y)
		{
			return new WaterTile(x, y);
		}

    }

	public enum TileType
	{
		Field,
		Mountain,
		Desert,
		Forest,
		Water,
	}
}
