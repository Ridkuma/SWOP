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
    /// Interaction logic for MapSelector.xaml
    /// </summary>
    public partial class MapSelector : UserControl
    {

        private static Color mapIdleColor = Color.FromRgb(150, 150, 150);
        private static Color mapSelectedColor = Color.FromRgb(150, 150, 64);

        public BuilderMapStrategy mapChosen = BuilderMapStrategy.Small;

        public MapSelector()
        {
            InitializeComponent();
        }

        private void RefreshUI()
        {
            SolidColorBrush idleColor = new SolidColorBrush(mapIdleColor);
            SolidColorBrush selectedColor = new SolidColorBrush(mapSelectedColor);

            this.btnDemo.Background = idleColor;
            this.btnSmall.Background = idleColor;
            this.btnNormal.Background = idleColor;

            switch (this.mapChosen)
            {
                case BuilderMapStrategy.Demo:
                    this.btnDemo.Background = selectedColor;
                    break;

                case BuilderMapStrategy.Small:
                    this.btnSmall.Background = selectedColor;
                    break;

                case BuilderMapStrategy.Normal:
                    this.btnNormal.Background = selectedColor;
                    break;
            }
        }

        private void AnyButtonClick()
        {
            this.RefreshUI();
        }

        #region ButtonEvents

        private void btnDemo_Click(object sender, RoutedEventArgs e)
        {
            this.mapChosen = BuilderMapStrategy.Demo;
            this.AnyButtonClick();
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            this.mapChosen = BuilderMapStrategy.Small;
            this.AnyButtonClick();
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            this.mapChosen = BuilderMapStrategy.Normal;
            this.AnyButtonClick();
        }

        #endregion
 
    }
}
