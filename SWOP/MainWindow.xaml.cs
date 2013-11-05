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
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static MainWindow INSTANCE;

		public GameMaster GM;
		public MapView mapView;

		public MainWindow()
		{
			InitializeComponent();
			INSTANCE = this;
		}

		public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
			GM = new GameMaster();
			GM.NewGame("small");

			mapView = new MapView(GM.CurrentMap, mapGrid);
		}
	}
}
