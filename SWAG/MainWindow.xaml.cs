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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using SmallWorld;

namespace SWAG
{
	/// <summary>
	/// Interaction logic for SurfaceWindow1.xaml
	/// </summary>
	public partial class MainWindow : SurfaceWindow
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


		/// <summary>
		/// Default constructor.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			INSTANCE = this;
			GM = new GameMaster();

			// Add handlers for window availability events
			AddWindowAvailabilityHandlers();

			for (int i = 0; i < 4; i++)
			{
				playerScatterView.Items.Add(new PlayerBox());
			}
		}


		/// <summary>
		/// Creation of a new game
		/// </summary>
		/// <param name="gameStrategy"></param>
		/// <param name="mapStrategy"></param>
		public void NewGame()
		{
			foreach (object pb in playerScatterView.Items)
			{
				if (!((PlayerBox)pb).isReady)
					return;
			}

			List<Tuple<string, FactionName>> listFaction = new List<Tuple<string, FactionName>>();
			foreach (object obj in playerScatterView.Items)
			{
				PlayerBox pb = obj as PlayerBox;
				listFaction.Add(new Tuple<string, FactionName>(pb.txtName.Text, pb.factionChosen));
			}
			GM.NewGame(BuilderGameStrategy.Local, BuilderMapStrategy.Normal, listFaction);

			// Subscribe to Game events
			GM.CurrentGame.OnStartGame += OnStartGame;
			GM.CurrentGame.OnNextPlayer += OnNextPlayer;
			GM.CurrentGame.OnEndGame += OnEndGame;
			GM.CurrentGame.OnMoveUnit += OnMoveUnit;

			// Subscribe to Media event
			//this.MediaPlayer.MediaEnded += OnMediaEnded;

			if (MapView != null)
				MapView.MapViewGrid.Children.RemoveRange(0, MapView.MapViewGrid.Children.Count);
			
			GM.CurrentGame.Start();
		}


		/// <summary>
		/// Play a random battle song
		/// </summary>
		public void RandomBattleSong()
		{
			Uri uri;
			switch (RAND.Next(3))
			{
				case 0:
					uri = new Uri(PATH + @"\musics\BGM1_FFTA.mp3");
					break;

				case 1:
					uri = new Uri(PATH + @"\musics\BGM2_Morro.mp3");
					break;

				case 2:
					uri = new Uri(PATH + @"\musics\BGM3_Ray.mp3");
					break;

				default:
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

		}




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

				//RefreshUI();
				foreach (object pb in playerScatterView.Items)
				{
					((PlayerBox) pb).myTurnView.Visibility = (GM.CurrentGame.CurrentPlayerId == ((PlayerBox)pb).playerId) ? Visibility.Visible : Visibility.Hidden;
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
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate()
			{
				//this.ActiveUnitView.Move();

				// TODO : tmp, could be better implemented than this 'a-la-bourrin' method
				foreach (FactionView fv in FactionsViews)
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
				//this.MediaPlayer.Stop();

				IGame g = GM.CurrentGame;
				foreach (object pb in playerScatterView.Items)
				{
					((PlayerBox) pb).inGameView.Visibility = Visibility.Hidden;
					((PlayerBox) pb).myTurnView.Visibility = Visibility.Hidden;
					((PlayerBox) pb).endView.Visibility = Visibility.Visible;
				}
			});
		}


		/// <summary>
		/// When BGM ends, play another
		/// </summary>
		private void OnMediaEnded(object sender, EventArgs e)
		{
			this.Dispatcher.Invoke((OnModifyWPFCallback) delegate()
			{
				this.RandomBattleSong();
			});
		}

		#endregion


		#region SurfaceStuff

		/// <summary>
		/// Occurs when the window is about to close. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			
			// Remove handlers for window availability events
			RemoveWindowAvailabilityHandlers();
		}

		/// <summary>
		/// Adds handlers for window availability events.
		/// </summary>
		private void AddWindowAvailabilityHandlers()
		{
			// Subscribe to surface window availability events
			ApplicationServices.WindowInteractive += OnWindowInteractive;
			ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
			ApplicationServices.WindowUnavailable += OnWindowUnavailable;
		}

		/// <summary>
		/// Removes handlers for window availability events.
		/// </summary>
		private void RemoveWindowAvailabilityHandlers()
		{
			// Unsubscribe from surface window availability events
			ApplicationServices.WindowInteractive -= OnWindowInteractive;
			ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
			ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
		}

		/// <summary>
		/// This is called when the user can interact with the application's window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowInteractive(object sender, EventArgs e)
		{
			//TODO: enable audio, animations here
		}

		/// <summary>
		/// This is called when the user can see but not interact with the application's window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowNoninteractive(object sender, EventArgs e)
		{
			//TODO: Disable audio here if it is enabled

			//TODO: optionally enable animations here
		}

		/// <summary>
		/// This is called when the application's window is not visible or interactive.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowUnavailable(object sender, EventArgs e)
		{
			//TODO: disable audio, animations here
		}

		#endregion
	}
}