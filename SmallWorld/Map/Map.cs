using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
	public interface IMap
	{
		int MapSize { get; }
		int TotalNbTurn { get; }
		int TotalNbUnits { get; }
		ITile[,] Tiles { get; }

		int GetRandomSeed();
		ITile GetStartTile(int playerId);
		int GetTileId(ITile tile);
		ITile GetTileFromId(int id);
	}


	public class Map : IMap
	{
		private AlgoMap algo;

		public int MapSize { get; protected set; }
		public int TotalNbTurn { get; protected set; }
		public int TotalNbUnits { get; protected set; }

		public ITile[,] Tiles { get; protected set; }

		/// <summary>
		/// Map constructor
		/// </summary>
		public Map(int size, int nbTurn, int nbUnits, int randomSeed)
		{
			MapSize = size;
			TotalNbTurn = nbTurn;
			TotalNbUnits = nbUnits;
			Tiles = new ITile[MapSize, MapSize];

			TileFactory tileFactory = new TileFactory();
			algo = new AlgoMap();
			algo.BuildMap(MapSize, randomSeed);

			for (int y = 0; y < MapSize; y++)
			{
				for (int x = 0; x < MapSize; x++)
				{
					Tiles[x, y] = tileFactory.CreateTile(x, y, algo.GetTileType(x, y));

					// Square map representation
					if (x > 0)
					{
						Tiles[x, y].AddAdjacentTile(Tiles[x - 1, y]);
						Tiles[x - 1, y].AddAdjacentTile(Tiles[x, y]);
					}
					if (y > 0)
					{
						Tiles[x, y].AddAdjacentTile(Tiles[x, y - 1]);
						Tiles[x, y - 1].AddAdjacentTile(Tiles[x, y]);
					}
					// Hexagonal map representation (just add top-left (odd) or top-right (even) corner tile)
					if (y % 2 == 0) // Odd line
					{
						if (x > 0 && y > 0)
						{
							Tiles[x, y].AddAdjacentTile(Tiles[x - 1, y - 1]);
							Tiles[x - 1, y - 1].AddAdjacentTile(Tiles[x, y]);
						}
					}
					else // Even line
					{
						if (x < MapSize - 1 && y > 0)
						{
							Tiles[x, y].AddAdjacentTile(Tiles[x + 1, y - 1]);
							Tiles[x + 1, y - 1].AddAdjacentTile(Tiles[x, y]);
						}
					}
				}
			}

			Console.WriteLine("[Log] MapBoard created");
		}


		/// <summary>
		/// Return the seed used to generate the map
		/// </summary>
		public int GetRandomSeed()
		{
			return algo.GetRandomSeed();
		}

		/// <summary>
		/// Return start coords of a specific player
		/// </summary>
		public ITile GetStartTile(int playerId)
		{
			return Tiles[algo.GetStartTileX(playerId), algo.GetStartTileY(playerId)];
		}

		/// <summary>
		/// Get unique index of a tile (equivalent of the position put in a single int)
		/// </summary>
		public int GetTileId(ITile tile)
		{
			return (tile.X * MapSize) + tile.Y;
		}

		/// <summary>
		/// Get unique index of a tile (equivalent of the position put in a single int)
		/// </summary>
		public ITile GetTileFromId(int id)
		{
			// Index out of limits
			if (id < 0 || id >= Tiles.Length)
				throw new NotSupportedException();

			return Tiles[(id / MapSize), (id % MapSize)];
		}
	}
}
