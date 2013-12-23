using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using SmallWorld;

namespace SWOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow INSTANCE;

        public GameMaster GM { get; protected set; }
        public MapView MapView { get; protected set; }
        public UnitView ActiveUnitView { get; set; }
        public MediaPlayer MediaPlayer { get; set; }

        private static Random RAND = new Random();

        private const int MIN_VOLUME = 20;
        private const int MAX_VOLUME = 80;

        public MainWindow()
        {
            InitializeComponent();
            this.ActiveUnitView = null;
            this.MediaPlayer = new MediaPlayer();
            INSTANCE = this;
        }

		/// <summary>
		/// Initialization of the game once UI loaded
		/// </summary>
        public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
			GM = new GameMaster();

			// tmp
			List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
			listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
			listFaction.Add(new Tuple<string, FactionName>("Ablouin", FactionName.Dwarves));

			NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Small, listFaction); // tmp
			GM.CurrentGame.Start(); // Ask explicitely to launch game
		}


		/// <summary>
		/// Application exit
		/// </summary>
		private void MainWindow_Closed(object sender, EventArgs e)
		{
			GM.DestroyGame();
		}



		/// <summary>
		/// Creation of a new game
		/// </summary>
		/// <param name="gameStrategy"></param>
		/// <param name="mapStrategy"></param>
		public void NewGame(BuilderGameStrategy gameStrategy, BuilderMapStrategy mapStrategy, List<Tuple<string, FactionName>> listFaction)
		{
            GM.NewGame(gameStrategy, mapStrategy, listFaction);

			// Subscribe to Game events
			GM.CurrentGame.OnStartGame += OnStartGame;
			GM.CurrentGame.OnNextPlayer += OnNextPlayer;
			GM.CurrentGame.OnEndGame += OnEndGame;
            GM.CurrentGame.OnMoveUnit += OnMoveUnit;
			GM.CurrentGame.OnNewChatMessage += OnNewChatMessage;

            // Subscribe to Media event
            this.MediaPlayer.MediaEnded += OnMediaEnded;

			// GUI
			btnNextPlayer.Visibility = System.Windows.Visibility.Visible;
			btnNextPlayer.Content = "Start Game !";
			
				if (MapView != null)
					MapView.MapViewGrid.Children.RemoveRange(0, MapView.MapViewGrid.Children.Count);
        }
        

        /// <summary>
        /// Play a random battle song
        /// </summary>
        public void RandomBattleSong()
        {
            String path = System.Environment.CurrentDirectory;
            path = path.Replace(@"\Debug", @"\SWOP\Resources");
            path = path.Replace(@"\Release", @"\SWOP\Resources");
            Uri uri;

            switch (RAND.Next(3))
            {
                case 0 :
                    uri = new Uri(path + @"\musics\BGM1_FFTA.mp3");
                    break;

                case 1 :
                    uri = new Uri(path + @"\musics\BGM2_Morro.mp3");
                    break;

                case 2:
                    uri = new Uri(path + @"\musics\BGM3_Ray.mp3");
                    break;

                default :
                    uri = new Uri(path + @"\musics\BGM1_FFTA.mp3");
                    break;
            }
            
            this.MediaPlayer.Stop();
            this.MediaPlayer.Open(uri);
            this.MediaPlayer.Volume = MAX_VOLUME;
            this.MediaPlayer.Play();
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
            this.MediaPlayer.Volume = MIN_VOLUME;
        }


        // Resume the game and hide main menu
        // -------------------------------------------------
        private void ButtonResume_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = System.Windows.Visibility.Hidden;
            mapGrid.Effect = null;
            this.MediaPlayer.Volume = MAX_VOLUME;
        }

        /// <summary>
        /// Mutes BGM
        /// </summary>
        private void ButtonMuteBGM_Click(object sender, RoutedEventArgs e)
        {
            this.MediaPlayer.Stop();
            this.muteBgmBtn.Visibility = Visibility.Hidden;
            this.playBgmBtn.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Resumes BGM
        /// </summary>
        private void ButtonPlayBGM_Click(object sender, RoutedEventArgs e)
        {
            this.RandomBattleSong();
            this.muteBgmBtn.Visibility = Visibility.Visible;
            this.playBgmBtn.Visibility = Visibility.Hidden;
        }

		// tmp
        private void ButtonReload_Click(object sender, RoutedEventArgs e)
		{
			// tmp
			List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
			listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
			listFaction.Add(new Tuple<string, FactionName>("Ablouin", FactionName.Dwarves));

			NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Normal, listFaction);
        }


		// tmp
		private void ButtonLaunchServer_Click(object sender, RoutedEventArgs e)
		{
			// tmp
			List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
			listFaction.Add(new Tuple<string, FactionName>("MeTheServer", FactionName.Vikings));

			NewGame(BuilderGameStrategy.Server, BuilderMapStrategy.Small, listFaction);
		}


		// tmp
		private void ButtonJoinServer_Click(object sender, RoutedEventArgs e)
		{
			// tmp
			List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
			listFaction.Add(new Tuple<string, FactionName>("MeTheTinyClient", FactionName.Gauls));

			NewGame(BuilderGameStrategy.Client, BuilderMapStrategy.Demo, listFaction);
		}


        // End current player turn
        // -------------------------------------------------
		private void ButtonNextPlayer_Click(object sender, RoutedEventArgs e)
		{
			if (GM.CurrentGame.CurrentTurn == 0)
				GM.CurrentGame.Start(); // Start game
			else
				GM.CurrentGame.NextPlayer(); // Next turn
		}

		#endregion



		#region EventsHandlers

		private delegate void OnModifyWPFCallback(); // Used for calls from other thread

		/// <summary>
		/// Event recevied when game officially start
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStartGame(object sender, EventArgs e)
		{
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate()
			{
				lblPlayer1Name.Content = GM.CurrentGame.Players[0].Name;
				lblPlayer2Name.Content = GM.CurrentGame.Players[1].Name;

				MapView = new MapView(GM.CurrentGame.MapBoard, mapGrid);

				foreach (Player p in GM.CurrentGame.Players)
				{
					new FactionView(p.CurrentFaction);
				}

                this.RandomBattleSong();

				OnNextPlayer(this, e); // Init game info in the UI
				btnNextPlayer.Content = "End my turn";
			});
		}

		/// <summary>
		/// Event received when it's next effective player turn
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnNextPlayer(object sender, EventArgs e)
		{
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate()
			{
				IGame g = GM.CurrentGame;
				lblNbTurn.Content = "Turn " + g.CurrentTurn + "/" + g.MapBoard.TotalNbTurn;

				borderPlayer1.Visibility = (g.CurrentPlayerId == 0) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
				borderPlayer2.Visibility = (g.CurrentPlayerId == 1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

				btnNextPlayer.Visibility = (g.CurrentPlayerIsMe) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

				if (this.ActiveUnitView != null)
				{
					this.ActiveUnitView.Unit.ChangeState(UnitState.Idle);
					this.ActiveUnitView.UpdateAppearance();
					this.ActiveUnitView = null;
                    this.selectedUnit.Visibility = Visibility.Hidden;
				}

				// TODO : Temp, need to place this somewhere else
				foreach (TileView tView in this.MapView.TilesView.Values)
				{
					if (tView.Tile.IsOccupied())
						tView.DispatchArmy();
				}

			});
		}

        /// <summary>
        /// Event received when a unit moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveUnit(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((OnModifyWPFCallback)delegate()
            {
                this.ActiveUnitView.Move();

                // TODO : Temp, need to place this somewhere else
                /*
                foreach (TileView tView in this.MapView.TilesView.Values)
                {
                    if (tView.Tile.IsOccupied())
                        tView.DispatchArmy();
                } */

            });
        }

		/// <summary>
		/// Event received when game is over
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnEndGame(object sender, EventArgs e)
		{
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate()
			{
                this.MediaPlayer.Stop();

				IGame g = GM.CurrentGame;
				lblNbTurn.Content = "Game Over !";

				borderPlayer1.Visibility = System.Windows.Visibility.Hidden;
				borderPlayer2.Visibility = System.Windows.Visibility.Hidden;
			});
		}

		/// <summary>
		/// Event received when a chat message is received
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnNewChatMessage(object sender, StringEventArgs e)
		{
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate ()
			{
				textChat.Text += "\n" + e.Text;
			});
		}

        /// <summary>
        /// When BGM ends, play another
        /// </summary>
        private void OnMediaEnded(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((OnModifyWPFCallback)delegate()
            {
                this.RandomBattleSong();
            });
        }

		#endregion


	}
}
