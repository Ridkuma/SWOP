using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SmallWorld;

namespace SWOP {
	public class MapView {
		public Map Map { get; protected set; }
        public Grid MapViewGrid { get; protected set; }
        public Dictionary<ITile, TileView> TilesView { get; protected set; }

		public MapView(SmallWorld.Map _map, Grid _mapViewGrid) {
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

			// tmp
			int[] startCoord = Map.GetStartPosition(0);
			Console.WriteLine("[LOG] Player 0 start coords = " + startCoord[0] + ", " + startCoord[1]);
			startCoord = Map.GetStartPosition(1);
			Console.WriteLine("[LOG] Player 1 start coords = " + startCoord[0] + ", " + startCoord[1]);
		}
	}
}
