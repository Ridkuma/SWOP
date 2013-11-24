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
            this.SetPosition();
            this.SetFactionView();
        }

        public void SetFactionView()
        {
            // Cannot switch on type with C# !
            // So this one is gonna burn eyes
            switch(this.Unit.GetType().Name)
            {
                case "VikingsUnit":
                    unit.Fill = (SolidColorBrush) Resources["VikingsColor"];
                    break;

                case "GaulsUnit":
                    unit.Fill = (SolidColorBrush) Resources["GaulsColor"];
                    break;

                case "DwarvesUnit":
                    unit.Fill = (SolidColorBrush) Resources["DwarvesColor"];
                    break;
            }
        }

        public void SetPosition()
        {
            int X = this.Unit.Position.X;
            int Y = this.Unit.Position.Y;
            TranslateTransform trTns = new TranslateTransform(X * 60 + ((Y % 2 == 0) ? 0 : 30), Y * 50);
            TransformGroup trGrp = new TransformGroup();
            trGrp.Children.Add(trTns);

            grid.RenderTransform = trGrp;
        }

    }
}
