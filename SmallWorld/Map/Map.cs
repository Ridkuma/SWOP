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
		ITile SelectedTile { get; set; }

		int GetRandomSeed();
		ITile GetStartPosition(int playerId);
	}


	public class Map : IMap
	{
		private AlgoMap algo;

		public int MapSize { get; protected set; }
		public int TotalNbTurn { get; protected set; }
		public int TotalNbUnits { get; protected set; }

		public ITile[,] Tiles { get; protected set; }
		public ITile SelectedTile { get; set; }

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
		public ITile GetStartPosition(int playerId)
		{
			return Tiles[algo.GetStartTileX(playerId), algo.GetStartTileY(playerId)];
		}
	}
}
