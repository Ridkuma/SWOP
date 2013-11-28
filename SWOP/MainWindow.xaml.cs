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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmallWorld;

namespace SWOP
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow INSTANCE;

        public GameMaster GM { get; protected set; }
        public MapView MapView { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();
            INSTANCE = this;
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string,FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("Ablouin", FactionName.Dwarves));

            GM = new GameMaster();
            GM.NewGame("small", listFaction);

			GM.CurrentGame.OnNextPlayer += OnNextPlayer;

            MapView = new MapView(GM.CurrentGame.MapBoard, mapGrid);

            DwarvesUnit dUnit = new DwarvesUnit("Gimli");
            GaulsUnit gUnit = new GaulsUnit("Agecanonix");
            dUnit.Position = GM.CurrentGame.MapBoard.GetStartPosition(0);
            gUnit.Position = GM.CurrentGame.MapBoard.GetStartPosition(1);
            UnitView dUnitView = new UnitView(dUnit);
            UnitView gUnitView = new UnitView(gUnit);
            MapView.TilesView[dUnit.Position].grid.Children.Add(dUnitView);
            MapView.TilesView[gUnit.Position].grid.Children.Add(gUnitView);

            player1Name.Content = GM.CurrentGame.Players[0].Name;
            player2Name.Content = GM.CurrentGame.Players[1].Name;

        }

		#region ButtonsEvents

		// Pause the game and display main menu
        // -------------------------------------------------
        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = System.Windows.Visibility.Visible;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 15;
            mapGrid.Effect = blur;
        }


        // Resume the game and hide main menu
        // -------------------------------------------------
        private void ButtonResume_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = System.Windows.Visibility.Hidden;
            mapGrid.Effect = null;
        }


        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("Ablouin", FactionName.Dwarves));

            GM.NewGame("normal", listFaction);

            MapView.MapViewGrid.Children.RemoveRange(0, MapView.MapViewGrid.Children.Count);
            MapView = new MapView(GM.CurrentGame.MapBoard, mapGrid);
        }



        // End current player turn
        // -------------------------------------------------
		private void ButtonNextPlayer_Click(object sender, RoutedEventArgs e)
		{
			GM.CurrentGame.NextPlayer();
		}

		#endregion


		#region EventsHandlers

		void OnNextPlayer(object sender, EventArgs e)
		{
			Console.WriteLine("[LOG] Next player turn"); // tmp
		}

		#endregion
	}
}
