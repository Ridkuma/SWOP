using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmallWorld;

namespace SWOP {

	/// <summary>
	/// Logique d'interaction pour Tile.xaml
	/// </summary>
	public partial class TileView : UserControl {
		//private static Random rnd;

		private ITile tile;
		private TileType type;



		/*public Tile() {
			InitializeComponent();

			if (rnd == null)
				rnd = new Random();
			m_type = (TileType) Enum.GetValues(typeof(TileType)).GetValue( rnd.Next(Enum.GetNames(typeof(TileType)).Length) );

			SetGround();
		}*/

		public TileView(ITile t) {
			InitializeComponent();

			tile = t;
			type = tile.Type;

			SetGround();
		}


		public void SetGround() {
			switch (type)
			{
				case TileType.Field:
					d_ground.Fill = (Brush) Resources["BrushField"];
					break;
				case TileType.Mountain:
					d_ground.Fill = (Brush) Resources["BrushMoutain"];
					break;
				case TileType.Desert:
					d_ground.Fill = (Brush)Resources["BrushDesert"];
					break;
				case TileType.Forest:
					d_ground.Fill = (Brush)Resources["BrushForest"];
					break;
				case TileType.Water:
					d_ground.Fill = (Brush) Resources["BrushWater"];
					break;
			}
		}

		// tmp
		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			foreach (ITile t in tile.AdjacentsTiles)
			{
				MainWindow.INSTANCE.mapView.tilesView[t].Hide();
			}
		}

		private void Hide()
		{
			d_ground.Visibility = System.Windows.Visibility.Hidden;
		}

		// tmp
		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			foreach (ITile t in tile.AdjacentsTiles)
			{
				MainWindow.INSTANCE.mapView.tilesView[t].Show();
			}
		}

		private void Show()
		{
			d_ground.Visibility = System.Windows.Visibility.Visible;
		}
	}
}
