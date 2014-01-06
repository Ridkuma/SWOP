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
		public ServerGame(IMap map, List<Player> players)
			: base(map, players)
		{
			InitServer();
			LocalPlayerId = 0;
		}


		public override void Start(bool generateUnits = true)
		{
			base.Start();

			// Send each units created to every players (overkill powaaa)
			foreach (Player p in Players)
			{
				foreach (IUnit u in p.CurrentFaction.Units)
				{
					SendMulticast(NetworkCommand.UnitMove, Players.IndexOf(p), MapBoard.GetTileId(u.Position), u.Name);
				}
			}

			SendMulticast(NetworkCommand.GameStart);
		}


		/// <summary>
		/// Warn each players of the new game turn
		/// </summary>
		public override void NextPlayer()
		{
			base.NextPlayer();
			SendMulticast(NetworkCommand.NextPlayer);
		}


		/// <summary>
		/// Warn each players of the new move
		/// </summary>
		public override void MoveUnit(IUnit unit, ITile destination)
		{
			base.MoveUnit(unit, destination);
			SendMulticast(NetworkCommand.UnitMove, 0, MapBoard.GetTileId(destination), unit.Name);
		}
		public void MoveUnit(IUnit unit, ITile destination, int ownerPlayer)
		{
			base.MoveUnit(unit, destination);
			SendMulticast(NetworkCommand.UnitMove, ownerPlayer, MapBoard.GetTileId(destination), unit.Name);
		}


		public override void Save()
		{
			throw new NotImplementedException();
		}


		public override void End()
		{
			SendMulticast(NetworkCommand.GameEnd);
			isRunning = false;
			
			if (listenerThread != null && listenerThread.ThreadState == ThreadState.Running)
				listenerThread.Join();

			base.End();
		}


		/// <summary>
		/// Treat incoming client packet
		/// </summary>
		private void TreatPacket(object objData)
		{
			NetworkData data = objData as NetworkData;
			
			switch (data.Command)
			{
				// New player in game
				case NetworkCommand.ClientLogin:
					Players.Add(new Player(data.ArgsString, (FactionName) data.ArgsInt2));
					AddClient();
					break;

				// Client give 
				// Next player turn
				case NetworkCommand.ClientNextPlayer:
					NextPlayer();
					break;


				// Player ask to move one of his unit
				case NetworkCommand.ClientUnitMove:
					foreach (IUnit u in Players[data.PlayerId].CurrentFaction.Units)
					{
						if (u.Name == data.ArgsString)
						{
							MoveUnit(u, MapBoard.GetTileFromId(data.ArgsInt2), data.PlayerId);
						}
					}
					break;

				default:
					throw new NotSupportedException();
			}
		}


		#region NetworkLogic

		private const int portMulticaster = 5053;
		private const int portServer = 11000;

		private UdpClient multicaster;
		private IPEndPoint remoteIpEP;
		private bool isRunning;
		private Thread listenerThread;


		/// <summary>
		/// Initialize the server network
		/// </summary>
		private void InitServer()
		{
			multicaster = new UdpClient();
			IPAddress multicastAddress = IPAddress.Parse("239.0.0.222");
			multicaster.JoinMulticastGroup(multicastAddress);
			remoteIpEP = new IPEndPoint(multicastAddress, portMulticaster);

			isRunning = true;

			// Start the listening thread
			listenerThread = new Thread(new ThreadStart(Listening));
			listenerThread.Start();
		}


		/// <summary>
		/// Add a new player to the existing list and informs other player
		/// </summary>
		private void AddClient()
		{
			if (CurrentTurn > 0)
				throw new NotSupportedException(); // Game cannot accept new player when already started

			int newPlayerId = Players.Count - 1;

			// Send init info to new client
			SendToOneClient(newPlayerId, NetworkCommand.LoginAccepted, newPlayerId);
			SendToOneClient(newPlayerId, NetworkCommand.InitMap, (int) GameMaster.GM.GameBuilder.LastBuilderMapStrategy, MapBoard.RandomSeed);
			for (int i = 0; i < Players.Count - 1; i++) // Send to new client the existing players (except itself)
				SendToOneClient(newPlayerId, NetworkCommand.NewPlayer, i, (int) Players[i].CurrentFaction.Name, Players[i].Name);

			// Inform all clients
			SendMulticast(NetworkCommand.NewPlayer, newPlayerId, (int) Players[newPlayerId].CurrentFaction.Name, Players[newPlayerId].Name);
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
					server = new UdpClient(portServer);
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
				Console.WriteLine("[ERROR] Can't start server on port " + portServer + ". Check your network settings.");
				return;
			}

			server.Client.ReceiveTimeout = 1000;

			// Infinite loop waiting for packet
			while (isRunning)
			{
				try
				{
					IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
					byte[] packet = server.Receive(ref ip);
					NetworkData data = new NetworkData(ip, packet);

					new Thread(new ParameterizedThreadStart(TreatPacket)).Start(data);
				}
				catch (SocketException e)
				{
				}
			}

			server.Close();
		}

		/// <summary>
		/// Send a packet to only one player
		/// </summary>
		private void SendToOneClient(int playerId, NetworkCommand cmd, int int1, int int2, string message)
		{
			try
			{
				// Send back to all clients, but only the playerId will process it (TODO: may be improved)
				NetworkData backData = new NetworkData(playerId, cmd, int1, int2, message);
				byte[] d = backData.ToByte();
				multicaster.Send(d, d.Length, remoteIpEP);
			}
			catch { }
		}
		private void SendToOneClient(int playerId, NetworkCommand cmd)
		{
			SendToOneClient(playerId, cmd, 0, 0, "");
		}
		private void SendToOneClient(int playerId, NetworkCommand cmd, int int1)
		{
			SendToOneClient(playerId, cmd, int1, 0, "");
		}
		private void SendToOneClient(int playerId, NetworkCommand cmd, int int1, int int2)
		{
			SendToOneClient(playerId, cmd, int1, int2, "");
		}
		private void SendToOneClient(int playerId, NetworkCommand cmd, string message)
		{
			SendToOneClient(playerId, cmd, 0, 0, message);
		}

		/// <summary>
		/// Send a packet to all players
		/// </summary>
		private void SendMulticast(NetworkCommand cmd, int int1, int int2, string message)
		{
			try
			{
				// Send back to all clients
				NetworkData backData = new NetworkData(0, cmd, int1, int2, message);
				byte[] d = backData.ToByte();
				multicaster.Send(d, d.Length, remoteIpEP);
			}
			catch { }
		}
		private void SendMulticast(NetworkCommand cmd)
		{
			SendMulticast(cmd, 0, 0, "");
		}
		private void SendMulticast(NetworkCommand cmd, int int1)
		{
			SendMulticast(cmd, int1, 0, "");
		}
		private void SendMulticast(NetworkCommand cmd, int int1, int int2)
		{
			SendMulticast(cmd, int1, int2, "");
		}
		private void SendMulticast(NetworkCommand cmd, string message)
		{
			SendMulticast(cmd, 0, 0, message);
		}
		#endregion
	}

}
