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
        protected enum TileViewState
        {
            Idle,
            Over,
            Selected,
            SelectedOver
        }
        private TileViewState currentState;
        private ITile tile;
        

        public TileView(ITile t) {
            InitializeComponent();

            tile = t;
        }


        private void OnTileLoaded(object sender, RoutedEventArgs e)
        {
            // Set position (hexagon disposition)
            TranslateTransform trTns = new TranslateTransform(tile.X * 60 + ((tile.Y % 2 == 0) ? 0 : 30), tile.Y * 50);
            TransformGroup trGrp = new TransformGroup();
            trGrp.Children.Add(trTns);

            grid.RenderTransform = trGrp;

            currentState = TileViewState.Idle;
            SetGround();
        }


        protected void SetAppearance(TileViewState newState)
        {
            if (newState == currentState)
                return;

            switch(newState)
            {
                case TileViewState.Idle:
                    hexagon.Opacity = 1;
                    break;
                case TileViewState.Over:
                    hexagon.Opacity = 0.8;
                    break;
                case TileViewState.Selected:
                    hexagon.Opacity = 0.4;
                    break;
                case TileViewState.SelectedOver:
                    hexagon.Opacity = 0.2;
                    break;
                default:
                    throw new NotImplementedException();
            }

            currentState = newState;
        }


        public void SetGround() {
            string brushPath = "Brush"; // set to "Brush" or "BrushImg"

            switch (tile.Type)
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

        
        private void Tile_MouseButtonDown(object sender, MouseEventArgs e)
        {
            MapView mapView = MainWindow.INSTANCE.MapView;
            if (mapView.Map.SelectedTile == tile)
                return;

            if (mapView.Map.SelectedTile != null)
                mapView.TilesView[mapView.Map.SelectedTile].SetAppearance(TileViewState.Idle);

            mapView.Map.SelectedTile = tile;
            SetAppearance(TileViewState.Selected);
        }


        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.Map.SelectedTile == tile)
                SetAppearance(TileViewState.SelectedOver);
            else
                SetAppearance(TileViewState.Over);
        }


        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.Map.SelectedTile == tile)
                SetAppearance(TileViewState.Selected);
            else
                SetAppearance(TileViewState.Idle);

            /*foreach (ITile t in Tile.AdjacentsTiles)
            {
                MainWindow.INSTANCE.MapView.TilesView[t].Show();
            }*/
        }
    }
}
