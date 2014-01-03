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
			foreach (ITile t in TilesView.Keys)
			{
				if (TilesView[t] != SelectedTileView)
				{
					if (SelectedTileView == null || !SelectedTileView.Tile.IsOccupied() || SelectedTileView.Tile.OccupyingUnits[0].Faction != MainWindow.INSTANCE.GM.CurrentGame.GetCurrentPlayer().CurrentFaction.Name)
						TilesView[t].SetAppearance(TileView.TileViewState.Idle);
					else if (Map.CanAttackTo(SelectedTileView.Tile, t))
						TilesView[t].SetAppearance(TileView.TileViewState.AttackReachable);
					else if (Map.CanMoveTo(SelectedTileView.Tile, t))
						TilesView[t].SetAppearance(TileView.TileViewState.MoveReachable);
					else
						TilesView[t].SetAppearance(TileView.TileViewState.Unreachable);
				}
			}
		}
    }
}
