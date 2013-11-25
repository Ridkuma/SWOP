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
        private IUnit Unit { get; set; }
        
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
            // Cannot switch on type with C# !
            // So this one is gonna burn eyes
            switch(this.Unit.GetType().Name)
            {
                case "VikingsUnit":
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["VikingsColor"];
                    this.sprite.Source = (BitmapImage) Resources["VikingsImg"];
                    break;

                case "GaulsUnit":
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["GaulsColor"];
                    this.sprite.Source = (BitmapImage) Resources["GaulsImg"];
                    break;

                case "DwarvesUnit":
                    this.selectedSquare.Stroke = (SolidColorBrush) Resources["DwarvesColor"];
                    this.sprite.Source = (BitmapImage) Resources["DwarvesImg"];
                    break;
            }
        }
    }
}
