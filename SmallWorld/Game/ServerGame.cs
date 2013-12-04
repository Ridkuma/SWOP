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
	class ServerGame : LocalGame
	{
		private UdpClient		broadcaster;
		private bool			isRunning;
		private Thread			listenerThread;
		private int				myPlayerId; // Local player Id


		/// <summary>
		/// Constructor
		/// </summary>
		public ServerGame(IMap map, List<Player> players)
			: base(map, players)
		{
			broadcaster = new UdpClient();
			broadcaster.EnableBroadcast = true;
			broadcaster.Connect(new IPEndPoint(IPAddress.Broadcast, 5053));

			isRunning = true;

			// Start the listening thread
			listenerThread = new Thread(new ThreadStart(Listening));
			listenerThread.Start();
		}

		public override void Start()
		{
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
			
			if (listenerThread != null && listenerThread.ThreadState == ThreadState.Running)
				listenerThread.Join();

			base.End();
		}


		/// <summary>
		/// Méthode qui écoute le réseau en permance en quête d'un message UDP sur le port 1523.
		/// </summary>
		private void Listening()
		{
			// Server socket creation
			UdpClient server = null;
			bool error = false;
			int attempts = 0;

			// Try 3 times before throwing error
			do
			{
				try
				{
					server = new UdpClient(1523);
				}
				catch
				{
					error = true;
					attempts++;
					Thread.Sleep(400);
				}
			} while (error && attempts < 4);

			// Impossible to connect
			if (server == null)
			{
				End();
				Console.WriteLine("[ERROR] Can't start server on port 1523. Check your network settings.");
				return;
			}

			server.Client.ReceiveTimeout = 1000;

			// Infinite loop waiting for packet
			while (isRunning)
			{
				try
				{
					IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
					byte[] data = server.Receive(ref ip);
					
					// Send back packet to all clients
					NetworkData cd = new NetworkData(ip, data);
					new Thread(new ParameterizedThreadStart(BroadcastMessage)).Start(cd);
				}
				catch (SocketException e)
				{
				}
			}

			server.Close();
		}


		/// <summary>
		/// Méthode en charge de traiter un message entrant.
		/// </summary>
		/// <param name="messageArgs"></param>
		private void BroadcastMessage(object messageArgs)
		{
			try
			{
				//On récupère les données entrantes et on les formatte comme il faut.
				NetworkData data = messageArgs as NetworkData;
				string message = string.Format("{0}:{1} > {2}", data.Client.Address.ToString(), data.Client.Port, data.strMessage);

				//On renvoie le message formatté à travers le réseau.
				byte[] donnees = Encoding.Default.GetBytes(message);
				broadcaster.Send(donnees, donnees.Length);
			}
			catch { }
		}

	}

}
