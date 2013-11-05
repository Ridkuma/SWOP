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
		private UniformGrid mapViewGrid;
		public Dictionary<ITile, TileView> tilesView;

		public MapView(SmallWorld.Map _map, UniformGrid _mapViewGrid) {
			map = _map;
			mapViewGrid = _mapViewGrid;

			mapViewGrid.Columns = map.MapSize;
			mapViewGrid.Rows = map.MapSize;
			mapViewGrid.Width = 50 * map.MapSize;
			mapViewGrid.Height = 50 * map.MapSize;

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

		}
	}
}
