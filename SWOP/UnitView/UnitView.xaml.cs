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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmallWorld;

namespace SWOP
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UnitView : UserControl
    {
        public IUnit Unit { get; private set; }

        
        public UnitView(IUnit u)
        {
            InitializeComponent();
            this.Unit = u;
        }

        private void OnUnitLoaded(object sender, RoutedEventArgs e)
        {
           this.SetAppearance();
        }

        public void SetAppearance()
        {
            switch(this.Unit.Faction)
            {
                case FactionName.Vikings:
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["VikingsColor"];
                    this.sprite.Source = (BitmapImage) Resources["VikingsImg"];
                    break;

                case FactionName.Gauls:
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["GaulsColor"];
                    this.sprite.Source = (BitmapImage) Resources["GaulsImg"];
                    break;

                case FactionName.Dwarves:
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["DwarvesColor"];
                    this.sprite.Source = (BitmapImage) Resources["DwarvesImg"];
                    break;
            }
        }

        public void UpdateAppearance()
        {
            this.selectedSquare.Visibility = (this.Unit.State == UnitState.Selected) ? Visibility.Visible : Visibility.Hidden;
        }

        public void Move(TileView tileView)
        {
            ITile prevPos = this.Unit.Position;
            if (this.Unit.Move(tileView.Tile))
            {
                MainWindow.INSTANCE.MapView.TilesView[prevPos].grid.Children.Remove(this);
                MainWindow.INSTANCE.MapView.TilesView[this.Unit.Position].grid.Children.Add(this);
            }
        }

        private void Unit_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Unit.Faction != MainWindow.INSTANCE.GM.CurrentGame.GetCurrentPlayer().CurrentFaction.Name)
                return;

            if (MainWindow.INSTANCE.ActiveUnitView != null)
            {
                MainWindow.INSTANCE.ActiveUnitView.Unit.ChangeState(UnitState.Idle);
                MainWindow.INSTANCE.ActiveUnitView.UpdateAppearance();
            }
            MainWindow.INSTANCE.ActiveUnitView = this;

            this.Unit.ChangeState(UnitState.Selected);
            this.UpdateAppearance();
        }
    }
}
