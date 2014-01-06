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
		int RandomSeed { get; }

        void RefreshAlgoMap();
		ITile GetStartTile(int playerId);
		int GetTileId(ITile tile);
		ITile GetTileFromId(int id);

		bool CanMoveTo(ITile source, ITile destination);
		bool CanAttackTo(ITile source, ITile destination);
		bool IsFavorite(int remainingFav, ITile source, ITile destination, bool canAttack, bool isOccupied);
	}

    [Serializable]
	public class Map : IMap
	{
        [NonSerialized] private AlgoMap algo = null;

		public int MapSize { get; protected set; }
		public int TotalNbTurn { get; protected set; }
		public int TotalNbUnits { get; protected set; }
        public int RandomSeed { get; protected set; }

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

            RandomSeed = (randomSeed == 0) ? algo.GetRandomSeed() : randomSeed;

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
		}

        /// <summary>
        /// Call initialization of the algo after load game
        /// </summary>
        public void RefreshAlgoMap()
        {
            algo = new AlgoMap();
            algo.BuildMap(MapSize, RandomSeed);
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

		/// <summary>
		/// Can go from a source to a destination tile
		/// </summary>
		public bool CanMoveTo(ITile source, ITile destination)
		{
            int idToSelect = 0;
			for (int i = 0; i < source.OccupyingUnits.Count; i++)
				if (source.OccupyingUnits[i].State == UnitState.Selected)
				{
					idToSelect = i;	break;
				}
			return algo.CanMoveTo(source.X, source.Y, destination.X, destination.Y, ((source.IsOccupied() && source.OccupyingUnits[idToSelect].CheckMove(destination)) ? (int) source.OccupyingUnits[idToSelect].Faction : -1));
		}

		/// <summary>
		/// Can attack from a source to a destination tile
		/// </summary>
		public bool CanAttackTo(ITile source, ITile destination)
		{
			int idToSelect = 0;
			for (int i = 0; i < source.OccupyingUnits.Count; i++)
				if (source.OccupyingUnits[i].State == UnitState.Selected)
				{
					idToSelect = i;
					break;
				}
			return algo.CanAttackTo(source.X, source.Y, destination.X, destination.Y, ((source.IsOccupied() && destination.IsOccupied() && source.OccupyingUnits[idToSelect].CheckAttack(destination.OccupyingUnits[0])) ? (int) source.OccupyingUnits[idToSelect].Faction : -1), (destination.IsOccupied() ? (int) destination.OccupyingUnits[0].Faction : -1));
		}


		public bool IsFavorite(int remainingFav, ITile source, ITile destination, bool canAttack, bool isOccupied)
		{
			if (remainingFav <= 0)
				return false;

			return algo.IsFavorite(source.X, source.Y, destination.X, destination.Y, canAttack, isOccupied);
		}
	}
}
