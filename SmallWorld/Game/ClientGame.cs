using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SmallWorld
{
	class ClientGame : LocalGame
	{
		private UdpClient client;
		private bool isRunning;
		private Thread listenerThread;
		private int localPlayerId;


		/// <summary>
		/// Constructor
		/// </summary>
		public ClientGame(IMap map, List<Player> players)
			: base(map, players)
		{
			client = new UdpClient();
			client.Connect("10.131.42.139", 1523);
			isRunning = true;

			// Launch the listening thread
			listenerThread = new Thread(new ThreadStart(Listening));
			listenerThread.Start();
		}

		public override void Start()
		{
			NetworkData tmpCoucou = new NetworkData(0, NetworkCommand.Login, "coucou !!!");
			byte[] tmpB = tmpCoucou.ToByte();
			client.Send(tmpB, tmpB.Length);
			NetworkData tmpPouet = new NetworkData(0, NetworkCommand.Login, "pouet !!!");
			tmpB = tmpPouet.ToByte();
			client.Send(tmpB, tmpB.Length);

			base.Start();
		}

		public override void NextPlayer()
		{
			throw new NotImplementedException();
		}

		public override void Save()
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
			isRunning = false;
			client.Close();
			listenerThread.Join();

			base.End();
		}


		/*private void btnEnvoyer_Click(object sender, EventArgs e)
		{
			byte[] data = Encoding.Default.GetBytes(txtMessage.Text);
			client.Send(data, data.Length);
		}*/


		/// <summary>
		/// Active waiting listening for incoming packet
		/// </summary>
		private void Listening()
		{
			UdpClient listener = null; // Listening socket

			// Secure creation of the socket
			try
			{
				listener = new UdpClient(5053);
			}
			catch
			{
				Console.WriteLine("[ERROR] Can't connect to UDP port 5053. Check your network settings.");
				return;
			}

			listener.Client.ReceiveTimeout = 1000;

			// Perpetual waiting for packet
			while (isRunning)
			{
				try
				{
					IPEndPoint ip = null;
					byte[] dataBytes = listener.Receive(ref ip);
					NetworkData packet = new NetworkData(ip, dataBytes);
				}
				catch
				{
				}
			}

			listener.Close();
		}
	}
}
