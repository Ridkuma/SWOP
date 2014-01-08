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
    /// Logique d'interaction pour PlayerCreator.xaml
    /// </summary>
    public partial class PlayerCreator : UserControl
    {
        private static Color factionIdleColor = Color.FromRgb(150, 150, 150);
        private static Color factionSelectedColor = Color.FromRgb(150, 150, 64);

        public bool isReady = false;
        public Color playerColor;
        public FactionName factionChosen;
        public bool aiPlayer = false;

        public PlayerCreator()
        {
            InitializeComponent();
        }

        private void RefreshUI()
        {
            SolidColorBrush idleColor = new SolidColorBrush(factionIdleColor);
            SolidColorBrush selectedColor = new SolidColorBrush(factionSelectedColor);

            this.btnViking.Background = idleColor;
            this.btnGaul.Background = idleColor;
            this.btnDwarf.Background = idleColor;

            switch (factionChosen)
            {
                case FactionName.Vikings:
                    this.btnViking.Background = selectedColor;
                    break;

                case FactionName.Gauls:
                    this.btnGaul.Background = selectedColor;
                    break;

                case FactionName.Dwarves:
                    this.btnDwarf.Background = selectedColor;
                    break;
            }
        }

        private void AnyButtonClick()
        {
            this.isReady = true;
            this.RefreshUI();
        }

        #region ButtonEvents

        private void btnViking_Click(object sender, RoutedEventArgs e)
        {
            this.factionChosen = FactionName.Vikings;
            this.AnyButtonClick();
        }

        private void btnGaul_Click(object sender, RoutedEventArgs e)
        {
            this.factionChosen = FactionName.Gauls;
            this.AnyButtonClick();
        }

        private void btnDwarf_Click(object sender, RoutedEventArgs e)
        {
            this.factionChosen = FactionName.Dwarves;
            this.AnyButtonClick();
        }

        private void HandleAI(object sender, RoutedEventArgs e)
        {
            this.aiPlayer = (!this.aiPlayer) ? true : false;
        }

        #endregion
    }
}
