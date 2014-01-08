using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
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

        private DispatcherTimer moveTimer;
        private float moveDirX, moveDirY;

        private const double MIN_VOLUME = 0.2;
        private const double MAX_VOLUME = 0.8;

        public MainWindow()
        {
            InitializeComponent();
            this.ActiveUnitView = null;
            this.MediaPlayer = new MediaPlayer();
            INSTANCE = this;

            PATH = System.Environment.CurrentDirectory;
            PATH = PATH.Replace(@"\Debug", @"\SWOP\Resources");
            PATH = PATH.Replace(@"\Release", @"\SWOP\Resources");

            moveTimer = new System.Windows.Threading.DispatcherTimer();
            moveTimer.Tick += new EventHandler(moveMap_Tick);
            moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }

		/// <summary>
		/// Initialization of the game once UI loaded
		/// </summary>
        public void MainWindow_Loaded(object sender, RoutedEventArgs e) {
			GM = new GameMaster();
            this.MediaPlayer.Volume = MAX_VOLUME;
            this.MediaPlayer.Open(new Uri(PATH + @"\musics\BGM0_Morro.mp3"));
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
            this.btnEndGame.Visibility = Visibility.Collapsed;
            this.btnNextPlayer.Visibility = Visibility.Visible;
            this.btnNextPlayer.Content = "Start Game !";
			
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
                    uri = new Uri(PATH + @"\musics\BGM1_AoE2.mp3");
                    break;

                case 1 :
                    uri = new Uri(PATH + @"\musics\BGM2_GW.mp3");
                    break;

                case 2:
                    uri = new Uri(PATH + @"\musics\BGM3_DS2.mp3");
                    break;

                default :
                    uri = new Uri(PATH + @"\musics\BGM0_Morro.mp3");
                    break;
            }
            
            this.MediaPlayer.Stop();
            this.MediaPlayer.Open(uri);
            this.MediaPlayer.Play();
        }

        /// <summary>
        /// Browse all logical children to find Player Creators and get their infos
        /// </summary>
        /// <returns>A list of tuples (PlayerName, FactionName)</returns>
        public List<Tuple<string, FactionName>> GetPlayersInfo(bool onlyFirst = false)
        {
            List<Tuple<string, FactionName>> playersInfos = new List<Tuple<string, FactionName>>();

            foreach (UIElement element in this.creationGrid.Children)
            {
                if (! (element is PlayerCreator))
                    continue;

                PlayerCreator playerCreator = (PlayerCreator) element;
                if (!playerCreator.isReady)
                    continue;

                playersInfos.Add(new Tuple<string, FactionName>(playerCreator.nameChosen, playerCreator.factionChosen));

                if (onlyFirst)
					break;
            }

            return playersInfos;
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
                unitHp.Text = ActiveUnitView.Unit.Hp.ToString() + " / " + ActiveUnitView.Unit.HpMax.ToString();
                unitMvt.Text = (ActiveUnitView.Unit.Mvt > 0) ? ActiveUnitView.Unit.Mvt.ToString() : "0";
				unitAtk.Text = ActiveUnitView.Unit.Atk.ToString();
				unitDef.Text = ActiveUnitView.Unit.Def.ToString();
				unitImg.Source = ActiveUnitView.sprite.Source;
                unitColor.Fill = new SolidColorBrush((g.CurrentPlayerId == 0) ? Color.FromRgb(44, 72, 195) : Color.FromRgb(166, 45, 26));
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

           	// Update player score : TMP ONLY TWO PLAYERS FOR NOW
			this.lblPlayer1Score.Content = this.GM.CurrentGame.Players[0].Score;
			this.lblPlayer2Score.Content = this.GM.CurrentGame.Players[1].Score;

			btnNextPlayer.Visibility = (g.CurrentPlayerIsMe) ? Visibility.Visible : Visibility.Hidden;
		}


		#region ButtonsEvents

        // Main menu -----------------------------------------------------------

        /// <summary>
        /// Open creation game window
        /// </summary>
        private void ButtonGameCreation_Click(object sender, RoutedEventArgs e)
        {
			playerCreator2.Visibility = Visibility.Visible;
			btnStartLocal.Visibility = Visibility.Visible;
			btnStartServer.Visibility = Visibility.Hidden;
			btnStartClient.Visibility = Visibility.Hidden;
			
            titleScreenGrid.Visibility = Visibility.Collapsed;
            creationGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Load last game saved
        /// </summary>
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            GM.LoadGame();

            if (GM.CurrentGame == null)
                return;

            // Subscribe to Game events
            GM.CurrentGame.OnStartGame += OnStartGame;
            GM.CurrentGame.OnNextPlayer += OnNextPlayer;
            GM.CurrentGame.OnEndGame += OnEndGame;
            GM.CurrentGame.OnMoveUnit += OnMoveUnit;
            GM.CurrentGame.OnNewChatMessage += OnNewChatMessage;

            GM.FinishLoadGame();

            menuGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Open creation game window
        /// </summary>
        private void ButtonServerCreation_Click(object sender, RoutedEventArgs e)
        {
			playerCreator2.Visibility = Visibility.Hidden;
			btnStartLocal.Visibility = Visibility.Hidden;
			btnStartServer.Visibility = Visibility.Visible;
			btnStartClient.Visibility = Visibility.Hidden;
			
            titleScreenGrid.Visibility = Visibility.Collapsed;
            creationGrid.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Open creation game window
        /// </summary>
        private void ButtonClientCreation_Click(object sender, RoutedEventArgs e)
        {
			playerCreator2.Visibility = Visibility.Hidden;
			btnStartLocal.Visibility = Visibility.Hidden;
			btnStartServer.Visibility = Visibility.Hidden;
			btnStartClient.Visibility = Visibility.Visible;
			
            titleScreenGrid.Visibility = Visibility.Collapsed;
            creationGrid.Visibility = Visibility.Visible;
        }


        // Game creation -----------------------------------------------------------

        /// <summary>
        /// Starts local game
        /// </summary>
        private void ButtonValidate_Click(object sender, RoutedEventArgs e)
        {
            NewGame(BuilderGameStrategy.Local, this.mapSelector.mapChosen, this.GetPlayersInfo());

            this.GM.CurrentGame.Start(); // Ask explicitely to launch game

            menuGrid.Visibility = Visibility.Collapsed;
            creationGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
            this.RandomBattleSong();
        }

        /// <summary>
        /// Starts server to host game
        /// </summary>
        private void ButtonLaunchServer_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("MeTheServer", FactionName.Vikings));

            NewGame(BuilderGameStrategy.Server, BuilderMapStrategy.Small, listFaction);

            menuGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;
            this.RandomBattleSong();
        }
        
        /// <summary>
        /// Join started server, as a client
        /// </summary>
        private void ButtonJoinServer_Click(object sender, RoutedEventArgs e)
        {
            // tmp
            List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
            listFaction.Add(new Tuple<string, FactionName>("MeTheTinyClient", FactionName.Gauls));

            NewGame(BuilderGameStrategy.Client, BuilderMapStrategy.Demo, listFaction);

            menuGrid.Visibility = Visibility.Collapsed;
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
            this.MediaPlayer.Stop();
            this.MediaPlayer.Open(new Uri(PATH + @"\musics\BGM0_Morro.mp3"));
            this.MediaPlayer.Play();
            menuGrid.Visibility = Visibility.Visible;
            titleScreenGrid.Visibility = Visibility.Visible;
            creationGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Collapsed;
            gameOverGrid.Visibility = Visibility.Collapsed;
            ButtonResume_Click(sender, e);
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
            if (GM.CurrentGame == null)
                return;

			if (GM.CurrentGame.CurrentTurn == 0)
				GM.CurrentGame.Start(); // Start game
			else
				GM.CurrentGame.NextPlayer(); // Next turn
		}

        /// <summary>
        /// Update End Game screen and go
        /// </summary>
        private void ButtonEndGame_Click(object sender, RoutedEventArgs e)
        {
            this.victorName.Text = GM.CurrentGame.Victor.Name;
            switch (GM.CurrentGame.Players.IndexOf(GM.CurrentGame.Victor))
            {
                case 0:
                    this.victorName.Foreground = new SolidColorBrush(Color.FromRgb(44, 72, 195));
                    break;

                case 1:
                    this.victorName.Foreground = new SolidColorBrush(Color.FromRgb(166, 45, 26));
                    break;
            }

            this.gameGrid.Visibility = Visibility.Collapsed;
            this.menuGrid.Visibility = Visibility.Visible;
            this.gameOverGrid.Visibility = Visibility.Visible;
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
                mapGrid.Margin = new Thickness(20, 20, 0, 0);

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
				lblNbTurn.Content = "Game Over !";

				this.borderPlayer1.Visibility = Visibility.Hidden;
				this.borderPlayer2.Visibility = Visibility.Hidden;
                this.btnEndGame.Visibility = Visibility.Visible;
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

        #region MoveMapHandlers

        private void rectMoveLeft_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            moveDirX = 2.5f;
            moveDirY = 0;
            moveTimer.Start();
        }

        private void rectMoveTop_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            moveDirX = 0;
            moveDirY = 2.5f;
            moveTimer.Start();
        }

        private void rectMoveRight_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            moveDirX = -2.5f;
            moveDirY = 0;
            moveTimer.Start();
        }

        private void rectMoveBottom_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            moveDirX = 0;
            moveDirY = -2.5f;
            moveTimer.Start();
        }

        private void rectMove_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            moveDirX = 0;
            moveDirY = 0f;
            moveTimer.Stop();
        }

        private void moveMap_Tick(object sender, EventArgs e)
        {
            if (MapView == null)
                return;

            if ((moveDirY == 0 && (mapGrid.Margin.Left > - MapView.Map.MapSize * 10 || moveDirX > 0) && (mapGrid.Margin.Left < 30 || moveDirX < 0))
                || (moveDirX == 0 && (mapGrid.Margin.Top > - MapView.Map.MapSize * 10 || moveDirY > 0) && (mapGrid.Margin.Top < 30 || moveDirY < 0)))
                mapGrid.Margin = new Thickness(mapGrid.Margin.Left + moveDirX, mapGrid.Margin.Top + moveDirY, 0, 0);
        }

        #endregion
    }
}
