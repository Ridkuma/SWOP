using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;
using SmallWorld;

namespace SWAG
{
	/// <summary>
	/// Interaction logic for PlayerBox.xaml
	/// </summary>
	public partial class PlayerBox : ScatterViewItem
	{
		private static int nbCreatedPlayers = 0;
		private static Color factionIdleColor = Color.FromRgb(150, 150, 150);
		private static Color factionSelectedColor = Color.FromRgb(150, 150, 64);

		public int playerId;
		public bool isReady = false;
		public Color playerColor;
		public FactionName factionChosen;


		public PlayerBox()
		{
			InitializeComponent();

			playerId = nbCreatedPlayers;
			txtName.Text = "Player " + (playerId + 1);

			switch(nbCreatedPlayers)
			{
				case 0: playerColor = Color.FromRgb(200, 64, 64); break;
				case 1: playerColor = Color.FromRgb(64, 64, 200); break;
				case 2: playerColor = Color.FromRgb(64, 200, 64); break;
				case 3: playerColor = Color.FromRgb(200, 180, 64); break;
			}
			rectColor.Fill = new SolidColorBrush(playerColor);
			nbCreatedPlayers++;

			Random rnd = new Random();
			factionChosen = (rnd.Next(3) == 0) ? FactionName.Vikings : ((rnd.Next(2) == 0) ? FactionName.Gauls : FactionName.Dwarves);

			RefreshUI();
		}


		private void btnValidate_Click(object sender, RoutedEventArgs e)
		{
			startView.Visibility = Visibility.Hidden;
			inGameView.Visibility = System.Windows.Visibility.Visible;
			playerName.Content = txtName.Text;
			isReady = true;

			MainWindow.INSTANCE.NewGame();
		}

		/// <summary>
		/// End current player turn
		/// </summary>
		private void btnNextPlayer_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.INSTANCE.GM.CurrentGame.NextPlayer(); // Next turn
		}


		private void btnViking_Click(object sender, RoutedEventArgs e)
		{
			factionChosen = FactionName.Vikings;
			RefreshUI();
		}

		private void btnGaul_Click(object sender, RoutedEventArgs e)
		{
			factionChosen = FactionName.Gauls;
			RefreshUI();
		}

		private void btnDwarf_Click(object sender, RoutedEventArgs e)
		{
			factionChosen = FactionName.Dwarves;
			RefreshUI();
		}


		private void RefreshUI()
		{
			btnViking.Background = new SolidColorBrush((factionChosen == FactionName.Vikings) ? factionSelectedColor : factionIdleColor);
			btnGaul.Background = new SolidColorBrush((factionChosen == FactionName.Gauls) ? factionSelectedColor : factionIdleColor);
			btnDwarf.Background = new SolidColorBrush((factionChosen == FactionName.Dwarves) ? factionSelectedColor : factionIdleColor);

			imgViking.Visibility = (factionChosen == FactionName.Vikings) ? Visibility.Visible : Visibility.Hidden;
			imgGaul.Visibility = (factionChosen == FactionName.Gauls) ? Visibility.Visible : Visibility.Hidden;
			imgDwarf.Visibility = (factionChosen == FactionName.Dwarves) ? Visibility.Visible : Visibility.Hidden;
		}

	}
}
