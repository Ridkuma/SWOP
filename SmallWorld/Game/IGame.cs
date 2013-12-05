using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
    {
		IMap MapBoard { get; }
		List<Player> Players { get; }
		int CurrentPlayerId { get; }
		int CurrentTurn { get; }

        Player GetCurrentPlayer();
		void AddNewPlayer(Player player);
		void Start();
        void NextPlayer();
        void Save();
		void End();

		event EventHandler<EventArgs> OnStartGame;
		event EventHandler<EventArgs> OnNextPlayer;
		event EventHandler<EventArgs> OnEndGame;
		event EventHandler<StringEventArgs> OnNewChatMessage;

		void OnRaiseNewChatMessage(string text);

    }

	public class StringEventArgs : EventArgs
	{
		public string Text { get; protected set; }

		public StringEventArgs(string text)
		{
			Text = text;
		}
	}
}
