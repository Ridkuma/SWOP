﻿using System;
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
		
		private ITile tile;
		private TileType type;


		public TileView(ITile t) {
			InitializeComponent();

			tile = t;
			type = tile.Type;
		}


		private void OnTileLoaded(object sender, RoutedEventArgs e)
		{
			// Set position (hexagon disposition)
			TranslateTransform trTns = new TranslateTransform(tile.X * 60 + ((tile.Y % 2 == 0) ? 0 : 30), tile.Y * 50);
			TransformGroup trGrp = new TransformGroup();
			trGrp.Children.Add(trTns);

			grid.RenderTransform = trGrp;

			SetGround();
		}


		public void SetGround() {
			string brushPath = "BrushImg"; // set to "Brush" or "BrushImg"

			switch (type)
			{
				case TileType.Field:
					hexagon.Fill = (Brush) Resources[brushPath + "Field"];
					break;
				case TileType.Mountain:
					hexagon.Fill = (Brush) Resources[brushPath + "Moutain"];
					break;
				case TileType.Desert:
					hexagon.Fill = (Brush) Resources[brushPath + "Desert"];
					break;
				case TileType.Forest:
					hexagon.Fill = (Brush) Resources[brushPath + "Forest"];
					break;
				case TileType.Water:
					hexagon.Fill = (Brush) Resources[brushPath + "Water"];
					break;
			}
		}

		// tmp
		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			hexagon.Opacity = 0.2;

			foreach (ITile t in tile.AdjacentsTiles)
			{
				MainWindow.INSTANCE.mapView.tilesView[t].Hide();
			}
		}

		private void Hide()
		{
			hexagon.Opacity = 0.7;
		}

		// tmp
		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			hexagon.Opacity = 1;

			foreach (ITile t in tile.AdjacentsTiles)
			{
				MainWindow.INSTANCE.mapView.tilesView[t].Show();
			}
		}

		private void Show()
		{
			hexagon.Opacity = 1;
		}
	}
}
