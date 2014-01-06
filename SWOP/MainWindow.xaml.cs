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
		public List<FactionView> FactionsViews { get; protected set; }
		public UnitView ActiveUnitView { get; set; }
		public MediaPlayer MediaPlayer { get; set; }

        private static Random RAND = new Random();
        private static String PATH;

        private const int MIN_VOLUME = 20;
        private const int MAX_VOLUME = 80;

        public MainWindow()
        {
            InitializeComponent();
            this.ActiveUnitView = null;
            this.MediaPlayer = new MediaPlayer();
            INSTANCE = this;

            PATH = System.Environment.CurrentDirectory;
            PATH = PATH.Replace(@"\Debug", @"\SWOP\Resources");
            PATH = PATH.Replace(@"\Release", @"\SWOP\Resources");
        }

		/// <summary>
		/// Initialization of the game once UI loaded
		/// </summary>
        public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
			GM = new GameMaster();
            this.MediaPlayer.Volume = MAX_VOLUME;
            this.MediaPlayer.Open(new Uri(PATH + @"\musics\BGM2_Morro.mp3"));
            this.MediaPlayer.Play();
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
			btnNextPlayer.Visibility = Visibility.Visible;
			btnNextPlayer.Content = "Start Game !";
			
			if (MapView != null)
				MapView.MapViewGrid.Children.RemoveRange(0, MapView.MapViewGrid.Children.Count);
        }
        

        /// <summary>
        /// Play a random battle song
        /// </summary>
        public void RandomBattleSong()
        {
            Uri uri;
            switch (RAND.Next(3))
            {
                case 0 :
                    uri = new Uri(PATH + @"\musics\BGM1_FFTA.mp3");
                    break;

                case 1 :
                    uri = new Uri(PATH + @"\musics\BGM2_Morro.mp3");
                    break;

                case 2:
                    uri = new Uri(PATH + @"\musics\BGM3_Ray.mp3");
                    break;

                default :
                    uri = new Uri(PATH + @"\musics\BGM1_FFTA.mp3");
                    break;
            }
            
            this.MediaPlayer.Stop();
            this.MediaPlayer.Open(uri);
            this.MediaPlayer.Play();
        }


		/// <summary>
		/// Ask explicitely to refresh each UI elements (may be tmp and replaced by bindings)
		/// </summary>
		public void RefreshUI()
		{
			IGame g = GM.CurrentGame;

			// Header UI elements
			lblPlayer1Name.Content = g.Players[0].Name;
			lblPlayer2Name.Content = g.Players[1].Name;
			lblNbTurn.Content = "Turn " + g.CurrentTurn + "/" + g.MapBoard.TotalNbTurn;

			borderPlayer1.Visibility = (g.CurrentPlayerId == 0) ? Visibility.Visible : Visibility.Hidden;
			borderPlayer2.Visibility = (g.CurrentPlayerId == 1) ? Visibility.Visible : Visibility.Hidden;

			// Bottom UI elements
			if (ActiveUnitView != null)
			{
				unitName.Text = ActiveUnitView.Unit.Name;
                unitHp.Text = "HP : " + ActiveUnitView.Unit.Hp.ToString() + "/" + ActiveUnitView.Unit.HpMax.ToString();
				unitMvt.Text = "MVT : " + ActiveUnitView.Unit.Mvt.ToString();
				unitAtk.Text = "ATK : " + ActiveUnitView.Unit.Atk.ToString();
				unitDef.Text = "DEF : " + ActiveUnitView.Unit.Def.ToString();
				unitImg.Source = ActiveUnitView.sprite.Source;
				selectedUnit.Visibility = Visibility.Visible;
			}
			else
			{
				selectedUnit.Visibility = Visibility.Hidden;
			}

            // Remove dead units
            foreach (FactionView factionView in this.FactionsViews)
            {
                factionView.BuryOurDeads();
            }

			btnNextPlayer.Visibility = (g.CurrentPlayerIsMe) ? Visibility.Visible : Visibility.Hidden;
		}


		#region ButtonsEvents

        // Main menu -----------------------------------------------------------

        /// <summary>
        /// Open creation game window
        /// </summary>
        private void ButtonGameCreation_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Hidden;
            creationGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Load last game saved
        /// </summary>
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            GM.LoadGame();

            // Subscribe to Game events
            GM.CurrentGame.OnStartGame += OnStartGame;
            GM.CurrentGame.OnNextPlayer += OnNextPlayer;
            GM.CurrentGame.OnEndGame += OnEndGame;
            GM.CurrentGame.OnMoveUnit += OnMoveUnit;
            GM.CurrentGame.OnNewChatMessage += OnNewChatMessage;

            GM.FinishLoadGame();

            menuGrid.Visibility = Visibility.Hidden;
            creationGrid.Visibility = Visibility.Hidden;
            gameGrid.Visibility = Visibility.Visible;
        }

        // tmp
        private void ButtonJoinServer_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("MeTheTinyClient", FactionName.Gauls));

            NewGame(BuilderGameStrategy.Client, BuilderMapStrategy.Demo, listFaction);

            menuGrid.Visibility = Visibility.Hidden;
            creationGrid.Visibility = Visibility.Hidden;
            gameGrid.Visibility = Visibility.Visible;
        }



        // Game creation -----------------------------------------------------------

        /// <summary>
        /// Starts Multi Player game creation
        /// </summary>
        private void ButtonValidate_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("TheFox", FactionName.Vikings));
            listFaction.Add(new Tuple<string, FactionName>("Ablouin", FactionName.Dwarves));

            NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction); // tmp
            this.GM.CurrentGame.Start(); // Ask explicitely to launch game

            menuGrid.Visibility = Visibility.Hidden;
            creationGrid.Visibility = Visibility.Hidden;
            gameGrid.Visibility = Visibility.Visible;
        }

        // tmp
        private void ButtonLaunchServer_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("MeTheServer", FactionName.Vikings));

            NewGame(BuilderGameStrategy.Server, BuilderMapStrategy.Small, listFaction);

            menuGrid.Visibility = Visibility.Hidden;
            creationGrid.Visibility = Visibility.Hidden;
            gameGrid.Visibility = Visibility.Visible;
        }


        // Pause menu -----------------------------------------------------------

		/// <summary>
		/// Pause the game and display main menu
		/// </summary>
        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Hidden;
            mainMenu.Visibility = Visibility.Visible;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 15;
            mapGrid.Effect = blur;
            this.MediaPlayer.Volume = MIN_VOLUME;
        }

		/// <summary>
		/// Resume the game and hide main menu
		/// </summary>
        private void ButtonResume_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Visible;
            mainMenu.Visibility = Visibility.Hidden;
            mapGrid.Effect = null;
            this.MediaPlayer.Volume = MAX_VOLUME;
        }

        /// <summary>
        /// Save current game
        /// </summary>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            GM.SaveGame();
            ButtonResume_Click(sender, e);
        }

        /// <summary>
        /// Quit game
        /// </summary>
        private void ButtonGoMainMenu_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Visible;
            gameGrid.Visibility = Visibility.Hidden;
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

			NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Demo, listFaction);
        }



		/// <summary>
		/// End current player turn
		/// </summary>
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
				MapView = new MapView(GM.CurrentGame.MapBoard, mapGrid);

				FactionsViews = new List<FactionView>();
				foreach (Player p in GM.CurrentGame.Players)
				{
					FactionsViews.Add(new FactionView(p.CurrentFaction));
				}

                //this.RandomBattleSong();

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
				if (ActiveUnitView != null)
					ActiveUnitView.Unselect();

				// TODO : Temp, need to place this somewhere else
				foreach (TileView tView in this.MapView.TilesView.Values)
				{
					if (tView.Tile.IsOccupied())
						tView.DispatchArmy();
				}
				MapView.ParseNewSelectedTile();
				
				RefreshUI();
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
                //this.ActiveUnitView.Move();

                // TODO : tmp, could be better implemented than this 'a-la-bourrin' method
				foreach(FactionView fv in FactionsViews)
					foreach (UnitView uv in fv.UnitViews.Values)
					{
						uv.Move();
					}

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

				borderPlayer1.Visibility = Visibility.Hidden;
				borderPlayer2.Visibility = Visibility.Hidden;
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
