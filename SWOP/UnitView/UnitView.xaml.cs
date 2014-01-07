using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmallWorld;

namespace SWOP
{
    /// <summary>
    /// Logique d'interaction pour UnitView.xaml
    /// </summary>
    public partial class UnitView : UserControl
    {
        public IUnit Unit { get; private set; }
        public TileView ParentTile { get; private set; }
        
        public UnitView(IUnit u)
        {
            InitializeComponent();
            this.Unit = u;
            this.ParentTile = MainWindow.INSTANCE.MapView.TilesView[this.Unit.Position];
        }

        private void OnUnitLoaded(object sender, RoutedEventArgs e)
        {
           this.SetAppearance();
        }

        /// <summary>
        /// Sets appearance depending on Faction
        /// (and UnitType in the future ?)
        /// </summary>
        public void SetAppearance()
        {
            switch(this.Unit.Faction)
            {
                case FactionName.Vikings:
                    this.sprite.Source = (BitmapImage) Resources["VikingsImg"];
                    break;

                case FactionName.Gauls:
                    this.sprite.Source = (BitmapImage) Resources["GaulsImg"];
                    break;

                case FactionName.Dwarves:
                    this.sprite.Source = (BitmapImage) Resources["DwarvesImg"];
                    break;
            }
        }

        /// <summary>
        /// Updates appearance if unit selected/idle
        /// Shows SelectionRectangle and toggles animation
        /// </summary>
        public void UpdateAppearance()
        {
            this.selectedSquare.Stroke = new SolidColorBrush((MainWindow.INSTANCE.GM.CurrentGame.CurrentPlayerId == 0) ? Color.FromRgb(44, 72, 195) : Color.FromRgb(166, 45, 26));
            Storyboard rectangleOpacityAnim = (Storyboard) this.grid.FindResource("rectangleOpacity");

            if (this.Unit.State == UnitState.Selected)
            {
                this.selectedSquare.Visibility = Visibility.Visible;
                rectangleOpacityAnim.Begin(this);
            }
            else
            {
                this.selectedSquare.Visibility = Visibility.Hidden;
                rectangleOpacityAnim.Stop(this);
            }
            
        }

        /// <summary>
        /// UI update on Unit selection
        /// </summary>
        public void Select()
        {
            if (this.Unit.Faction != MainWindow.INSTANCE.GM.CurrentGame.GetCurrentPlayer().CurrentFaction.Name)
                return;

            if (MainWindow.INSTANCE.ActiveUnitView != null)
            {
				MainWindow.INSTANCE.ActiveUnitView.Unselect();
            }
			MainWindow.INSTANCE.ActiveUnitView = this;

            this.Unit.ChangeState(UnitState.Selected);
            this.UpdateAppearance();

			MainWindow.INSTANCE.RefreshUI();
        }

		
        /// <summary>
        /// UI update on Unit selection
        /// </summary>
		public void Unselect()
		{
			MainWindow.INSTANCE.ActiveUnitView.Unit.ChangeState(UnitState.Idle);
			MainWindow.INSTANCE.ActiveUnitView.UpdateAppearance();
			MainWindow.INSTANCE.ActiveUnitView = null;

			MainWindow.INSTANCE.RefreshUI();
		}

        /// <summary>
        /// UI update on Unit move
        /// </summary>
        public void Move()
        {
            TileView newTileView = MainWindow.INSTANCE.MapView.TilesView[this.Unit.Position];
			if (this.ParentTile == newTileView)
				return;

            this.ParentTile.grid.Children.Remove(this);
            newTileView.grid.Children.Add(this);
            this.ParentTile = newTileView;

			// If only one unit on tile => replace it in the center
			if (this.ParentTile.Tile.OccupyingUnits.Count <= 1)
				this.Margin = new Thickness(0);
        }
    }
}
