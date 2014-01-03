using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SmallWorld;

namespace SWOP {
    public class MapView {
        public IMap Map { get; protected set; }
        public Grid MapViewGrid { get; protected set; }
		public Dictionary<ITile, TileView> TilesView { get; protected set; }
		public TileView SelectedTileView { get; set; }

		/// <summary>
		/// Constructor to the view of the map
		/// </summary>
		/// <param name="_map">Reference to the Map model</param>
		/// <param name="_mapViewGrid">WPF Grid element</param>
        public MapView(SmallWorld.IMap _map, Grid _mapViewGrid) {
            Map = _map;
            MapViewGrid = _mapViewGrid;

            TilesView = new Dictionary<ITile,TileView>();

            for (int y = 0; y < Map.MapSize; y++)
            {
                for (int x = 0; x < Map.MapSize; x++)
                {
                    TileView tile = new TileView(Map.Tiles[x, y]);
                    MapViewGrid.Children.Add(tile);
                    TilesView.Add(Map.Tiles[x, y], tile);
                }
            }
        }

		public void ParseNewSelectedTile()
		{
			int remainingFav = 3; // Adviced tiles for next action (max 3)
			List<TileView> remainingFavList = new List<TileView>();

			foreach (ITile t in TilesView.Keys)
			{
				if (TilesView[t] != SelectedTileView)
				{
					// Reset tile
					if (SelectedTileView == null || !SelectedTileView.Tile.IsOccupied() || SelectedTileView.Tile.OccupyingUnits[0].Faction != MainWindow.INSTANCE.GM.CurrentGame.GetCurrentPlayer().CurrentFaction.Name)
					{
						TilesView[t].SetAppearance(TileView.TileViewState.Idle);
					}
					// Attack tile
					else if (Map.CanAttackTo(SelectedTileView.Tile, t))
					{
						if (Map.IsFavorite(remainingFav, SelectedTileView.Tile, t, true, false))
						{
							TilesView[t].SetAppearance(TileView.TileViewState.AttackReachableFavorite);
							remainingFav--;
						}
						else
						{
							TilesView[t].SetAppearance(TileView.TileViewState.AttackReachable);
						}
					}
					// Move tile
					else if (Map.CanMoveTo(SelectedTileView.Tile, t))
					{
						TilesView[t].SetAppearance(TileView.TileViewState.MoveReachable);
						if (Map.IsFavorite(remainingFav, SelectedTileView.Tile, t, false, t.IsOccupied()))
						{
							remainingFavList.Insert(0, TilesView[t]); // Insert at start in priority
						}
						else if (remainingFav > 0)
						{
							remainingFavList.Add(TilesView[t]);
						}
					}
					// Unreachable tile
					else
					{
						TilesView[t].SetAppearance(TileView.TileViewState.Unreachable);
					}
				}
			}

			for (int i = 0; i < remainingFav; i++)
			{
				if (i < remainingFavList.Count)
					remainingFavList[i].SetAppearance(TileView.TileViewState.MoveReachableFavorite);
			}
		}
    }
}
