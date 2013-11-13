using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SmallWorld;

namespace SWOP {
	public class MapView {
		private Map map;
		public Grid mapViewGrid; // tmp pass to private
		public Dictionary<ITile, TileView> tilesView;

		public MapView(SmallWorld.Map _map, Grid _mapViewGrid) {
			map = _map;
			mapViewGrid = _mapViewGrid;

			tilesView = new Dictionary<ITile,TileView>();

			for (int y = 0; y < map.MapSize; y++)
			{
				for (int x = 0; x < map.MapSize; x++)
				{
					TileView tile = new TileView(map.Tiles[x, y]);
					mapViewGrid.Children.Add(tile);
					tilesView.Add(map.Tiles[x, y], tile);
				}
			}

			// tmp
			int[] startCoord = map.GetStartPosition(0);
			Console.WriteLine("[LOG] Player 0 start coords = " + startCoord[0] + ", " + startCoord[1]);
			startCoord = map.GetStartPosition(1);
			Console.WriteLine("[LOG] Player 1 start coords = " + startCoord[0] + ", " + startCoord[1]);
		}
	}
}
