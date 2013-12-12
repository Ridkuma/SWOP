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

        public ITile Tile { get; private set; }

        private TileViewState currentState;

        static Random random = new Random();
        

        public TileView(ITile t) {
            InitializeComponent();

            Tile = t;
        }


        private void OnTileLoaded(object sender, RoutedEventArgs e)
        {
            // Set position (hexagon disposition)
            TranslateTransform trTns = new TranslateTransform(Tile.X * 60 + ((Tile.Y % 2 == 0) ? 0 : 30), Tile.Y * 50);
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

            switch (Tile.Type)
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

        /// <summary>
        /// Randomize aspect of multiple Units on the same Tile
        /// </summary>
        public void DispatchArmy()
        {
            IEnumerable<UnitView> uViews = this.grid.Children.OfType<UnitView>();

            if (uViews.Count() < 2)
                return;

            foreach (UnitView uView in uViews)
            {
                int randMargin = random.Next(10, 30);
                int randMarginDir = random.Next(3);

                switch (randMarginDir)
                {
                    case 0 :
                        uView.Margin = new Thickness(randMargin, 0, 0, 0);
                        break;

                    case 1:
                        uView.Margin = new Thickness(0, randMargin, 0, 0);
                        break;

                    case 2:
                        uView.Margin = new Thickness(0, 0, randMargin, 0);
                        break;

                    case 3:
                        uView.Margin = new Thickness(0, 0, 0, randMargin);
                        break;
                }
            }
        }

        #region Events

        private void Tile_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            MapView mapView = MainWindow.INSTANCE.MapView;
            if (mapView.Map.SelectedTile == Tile)
                return;

            if (mapView.Map.SelectedTile != null)
                mapView.TilesView[mapView.Map.SelectedTile].SetAppearance(TileViewState.Idle);

            mapView.Map.SelectedTile = Tile;
            SetAppearance(TileViewState.Selected);

            if (!this.Tile.IsOccupied())
                return;

            IEnumerable<UnitView> uViews = this.grid.Children.OfType<UnitView>();

            // if (uViews.Count() < 2)
             uViews.ElementAt(0).Select();


        }

        private void Tile_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.ActiveUnitView != null)
            {
                MainWindow.INSTANCE.ActiveUnitView.Unit.Move(this.Tile);
                MainWindow.INSTANCE.ActiveUnitView.Margin = new Thickness(0);

                MapView mapView = MainWindow.INSTANCE.MapView;
                if (mapView.Map.SelectedTile != null)
                {
                    mapView.TilesView[mapView.Map.SelectedTile].SetAppearance(TileViewState.Idle);
                    mapView.Map.SelectedTile = null;
                }
            }
        }


        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.Map.SelectedTile == Tile)
                SetAppearance(TileViewState.SelectedOver);
            else
                SetAppearance(TileViewState.Over);
        }


        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.MapView.Map.SelectedTile == Tile)
                SetAppearance(TileViewState.Selected);
            else
                SetAppearance(TileViewState.Idle);

            /*foreach (ITile t in Tile.AdjacentsTiles)
            {
                MainWindow.INSTANCE.MapView.TilesView[t].Show();
            }*/
        }

        #endregion

    }
}
