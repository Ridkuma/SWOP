using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public class Map
	{
		private int	mapSize;
		public int MapSize
		{
			get
			{
				return mapSize;
			}
		}

		private ITile[,] tiles;
		public ITile[,] Tiles
		{
			get
			{
				return tiles;
			}
		}


		public Map(int size)
		{
			mapSize = size;
			tiles = new ITile[mapSize, mapSize];

			TileFactory tileFactory = new TileFactory();
			AlgoMap algo = new AlgoMap();
			algo.BuildMap(mapSize);

			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
					tiles[x, y] = tileFactory.CreateTile(x, y, algo.GetTileType(x, y));

					// Square map representation
					if (x > 0)
					{
						tiles[x, y].AddAdjacentTile(tiles[x - 1, y]);
						tiles[x - 1, y].AddAdjacentTile(tiles[x, y]);
					}
					if (y > 0)
					{
						tiles[x, y].AddAdjacentTile(tiles[x, y - 1]);
						tiles[x, y - 1].AddAdjacentTile(tiles[x, y]);
					}
					// Hexagonal map representation (just add top-left (odd) or top-right (even) corner tile)
					if (y % 2 == 0) // Odd line
					{
						if (x > 0 && y > 0)
						{
							tiles[x, y].AddAdjacentTile(tiles[x - 1, y - 1]);
							tiles[x - 1, y - 1].AddAdjacentTile(tiles[x, y]);
						}
					}
					else // Even line
					{
						if (x < mapSize - 1 && y > 0)
						{
							tiles[x, y].AddAdjacentTile(tiles[x + 1, y - 1]);
							tiles[x + 1, y - 1].AddAdjacentTile(tiles[x, y]);
						}
					}
				}
			}

			Console.WriteLine("[Log] MapBoard created");
		}

	}
}
