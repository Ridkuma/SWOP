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
        public enum TileViewState
        {
            Idle,
            Selected,
			MoveReachable,
			AttackReachable,
			Unreachable,
        }

        public ITile Tile { get; private set; }

        private TileViewState currentState;

        static Random random = new Random();
        

        public TileView(ITile t) {
            InitializeComponent();

            Tile = t;
        }


		/// <summary>
		/// Init position of the tile
		/// </summary>
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


		/// <summary>
		/// Change view of the tile
		/// </summary>
		/// <param name="newState"></param>
        public void SetAppearance(TileViewState newState)
        {
            if (newState == currentState)
                return;

            switch(newState)
            {
                case TileViewState.Idle:
                    hexagonPath.Opacity = 1;
                    break;
                case TileViewState.Selected:
					bgPath.Fill = new SolidColorBrush(Color.FromRgb(222,222,222));
                    hexagonPath.Opacity = 0.6;
                    break;
				case TileViewState.MoveReachable:
					bgPath.Fill = new SolidColorBrush(Color.FromRgb(100,255,100));
					hexagonPath.Opacity = 0.7;
					break;
				case TileViewState.AttackReachable:
					bgPath.Fill = new SolidColorBrush(Color.FromRgb(255,64,32));
					hexagonPath.Opacity = 0.6;
					break;
				case TileViewState.Unreachable:
					bgPath.Fill = new SolidColorBrush(Color.FromRgb(0,0,0));
					hexagonPath.Opacity = 0.6;
					break;
                default:
                    throw new NotImplementedException();
            }

            currentState = newState;
        }


        public void SetGround() {
            string brushPath = "BrushImg"; // set to "Brush" or "BrushImg"

            switch (Tile.Type)
            {
                case TileType.Field:
                    hexagonPath.Fill = (Brush) Resources[brushPath + "Field"];
                    break;
                case TileType.Mountain:
                    hexagonPath.Fill = (Brush) Resources[brushPath + "Mountain"];
                    break;
                case TileType.Desert:
                    hexagonPath.Fill = (Brush) Resources[brushPath + "Desert"];
                    break;
                case TileType.Forest:
                    hexagonPath.Fill = (Brush) Resources[brushPath + "Forest"];
                    break;
                case TileType.Water:
                    hexagonPath.Fill = (Brush) Resources[brushPath + "Water"];
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
				int randMarginX = random.Next(-10, 15);
				int randMarginY = random.Next(-20, 15);

                uView.Margin = new Thickness(randMarginX, randMarginY, - randMarginX, - randMarginY);
            }
        }

        #region Events

		/// <summary>
		/// Player left-clik on the tile => select a unit on it
		/// </summary>
        private void Tile_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            MapView mapView = MainWindow.INSTANCE.MapView;
			if (mapView.SelectedTileView != this)
			{
				if (mapView.SelectedTileView != null)
					mapView.SelectedTileView.SetAppearance(TileViewState.Idle);

				mapView.SelectedTileView = this;
				SetAppearance(TileViewState.Selected);
			}

			// Unit selection
			if (this.Tile.IsOccupied())
			{
				IEnumerable<UnitView> uViews = this.grid.Children.OfType<UnitView>();
				if (uViews.Count() <= 1)
				{
					uViews.ElementAt(0).Select();
				}
				else
				{
					int idToSelect = 0;
					for (int i = 0; i < uViews.Count(); i++)
					{
						if (uViews.ElementAt(i).Unit.State == UnitState.Selected)
						{
							idToSelect = (i + 1) % uViews.Count();
							break;
						}
					}
					uViews.ElementAt(idToSelect).Select();
				}
			}
			else if (MainWindow.INSTANCE.ActiveUnitView != null)
			{
				MainWindow.INSTANCE.ActiveUnitView.Unselect();
			}

			mapView.ParseNewSelectedTile();
        }

		/// <summary>
		/// Player right-clik => move the selected unit, or attack
		/// </summary>
        private void Tile_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            if (MainWindow.INSTANCE.ActiveUnitView != null)
            {
                // Try to move
                MainWindow.INSTANCE.ActiveUnitView.Unit.Move(this.Tile);

                // Try to attack
                IUnit bestDefUnit = this.Tile.GetBestDefUnit();
                if (bestDefUnit != null)
                {
                    Console.WriteLine("Defender : " + bestDefUnit.Name);
                    MainWindow.INSTANCE.ActiveUnitView.Unit.Attack(bestDefUnit);
                    MainWindow.INSTANCE.RefreshUI();
                }

                MapView mapView = MainWindow.INSTANCE.MapView;
                if (mapView.SelectedTileView != null && mapView.Map.CanMoveTo(mapView.SelectedTileView.Tile, this.Tile))
                {
                    mapView.SelectedTileView.SetAppearance(TileViewState.Idle);
                    mapView.SelectedTileView = null;
					mapView.ParseNewSelectedTile();
				}
            }
        }

		/// <summary>
		/// Mouse over the tile
		/// </summary>
        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
			hexagonPath.Opacity = hexagonPath.Opacity - 0.1;

            // Display ToHit chance from Active Unit to Best Unit on Tile
            if (MainWindow.INSTANCE.ActiveUnitView != null 
                && this.Tile.IsOccupied() 
                && MainWindow.INSTANCE.ActiveUnitView.Unit.CheckAttack(this.Tile.OccupyingUnits[0]))
            {
                IUnit bestUnit = this.Tile.GetBestDefUnit();
                double toHit = MainWindow.INSTANCE.GM.CurrentGame.ToHitChance(MainWindow.INSTANCE.ActiveUnitView.Unit, bestUnit);
                this.hitChance.Text = toHit.ToString() + "%";
                this.hitChance.Visibility = Visibility.Visible;
            }
        }


		/// <summary>
		/// Mouse quit 'overred' tile
		/// </summary>
        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
			hexagonPath.Opacity = hexagonPath.Opacity + 0.1;
            this.hitChance.Visibility = Visibility.Collapsed;
        }

        #endregion

    }
}
