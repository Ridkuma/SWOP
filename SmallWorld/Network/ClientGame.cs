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
		public ClientGame(IMap map, List<Player> players)
			: base(map, players)
		{
			InitClient();
			LocalPlayerId = -666;

			SendPacket(NetworkCommand.ClientLogin, 0, (int) players[0].CurrentFaction.Name, players[0].Name);
			players.Clear();
		}


		public override void Start(bool generateUnits = true)
		{
			// Don't do anything, it's the server which order to launch the game
		}


		public override void NextPlayer()
		{
			SendPacket(NetworkCommand.ClientNextPlayer);
		}


		public override void MoveUnit(IUnit unit, ITile destination)
		{
			SendPacket(NetworkCommand.ClientUnitMove, 0, MapBoard.GetTileId(destination), unit.Name);
		}

        public override void AttackUnit(IUnit unit, ITile destination)
        {
            SendPacket(NetworkCommand.ClientUnitAttack, 0, MapBoard.GetTileId(destination), unit.Name);
        }

		public override void End()
		{
			isRunning = false;
			client.Close();
			listenerThread.Join();

			base.End();
		}


		/// <summary>
		/// Treat incoming server packet
		/// </summary>
		private void TreatPacket(object objData)
		{
			NetworkData data = objData as NetworkData;

			switch (data.Command)
			{
				// I am accepted
				case NetworkCommand.LoginAccepted:
					LocalPlayerId = data.ArgsInt1;
					break;

				// New player in game
				case NetworkCommand.NewPlayer:
					Players.Add(new Player(data.ArgsString, (FactionName) data.ArgsInt2));
					break;

				// Receive info to generate map
				case NetworkCommand.InitMap:
					MapBoard = GameMaster.GM.GameBuilder.BuildMap((BuilderMapStrategy) data.ArgsInt1, data.ArgsInt2);
					break;

				// Launch game
				case NetworkCommand.GameStart:
					base.Start(false);
					break;

				// Next player turn
				case NetworkCommand.NextPlayer:
					base.NextPlayer();
					break;

				// End game
				case NetworkCommand.GameEnd:
					End();
					break;

				// Move unit (or create if inexisting)
				case NetworkCommand.UnitMove:
					bool unitFind = false;
					foreach (IUnit u in Players[data.ArgsInt1].CurrentFaction.Units)
					{
						if (u.Name == data.ArgsString)
						{
							base.MoveUnit(u, MapBoard.GetTileFromId(data.ArgsInt2));
							unitFind = true;
							break;
						}
					}
					if (!unitFind)
					{
						Players[data.ArgsInt1].CurrentFaction.AddUnit(data.ArgsString, MapBoard.GetTileFromId(data.ArgsInt2));
					}
					break;

                // Move unit (or create if inexisting)
                case NetworkCommand.UnitAttack:
                    foreach (IUnit u in Players[data.ArgsInt1].CurrentFaction.Units)
                    {
                        if (u.Name == data.ArgsString)
                        {
                            base.AttackUnit(u, MapBoard.GetTileFromId(data.ArgsInt2));
                            break;
                        }
                    }
                    break;

				default:
					throw new NotSupportedException();
			}
		}

		#region NetworkLogic

		private const int portListener = 5053;
		private const int portClient = 11000;

		private UdpClient client;
		private bool isRunning;
		private Thread listenerThread;


		/// <summary>
		/// Initialize client network
		/// </summary>
		private void InitClient()
		{
			client = new UdpClient();
			client.Connect("127.0.0.1", portClient);
			isRunning = true;

			// Launch the listening thread
			listenerThread = new Thread(new ThreadStart(Listening));
			listenerThread.Start();
		}

		/// <summary>
		/// Active waiting listening for incoming packet
		/// </summary>
		private void Listening()
		{
			UdpClient listener = null; // Listening socket
			IPEndPoint localIpEP = null; // Listening IP

			// Secure creation of the socket
			try
			{
				listener = new UdpClient();
				listener.ExclusiveAddressUse = false;
				localIpEP = new IPEndPoint(IPAddress.Any, portListener);

				listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				listener.ExclusiveAddressUse = false;

				listener.Client.Bind(localIpEP);

				IPAddress multicastAddress = IPAddress.Parse("239.0.0.222");
				listener.JoinMulticastGroup(multicastAddress);
			}
			catch
			{
				Console.WriteLine("[ERROR] Can't connect to UDP port" + portListener + ". Check your network settings.");
				return;
			}

			listener.Client.ReceiveTimeout = 1000;

			// Perpetual waiting for packet
			while (isRunning)
			{
				try
				{
					byte[] packet = listener.Receive(ref localIpEP);
					NetworkData data = new NetworkData(localIpEP, packet);

					if (data.PlayerId == 0 || data.PlayerId == LocalPlayerId || LocalPlayerId < 0) // Block if not wanted
						new Thread(new ParameterizedThreadStart(TreatPacket)).Start(data);
				}
				catch
				{
				}
			}

			listener.Close();
		}


		/// <summary>
		/// Send a packet to the server
		/// </summary>
		private void SendPacket(NetworkCommand cmd, int int1, int int2, string message)
		{
			NetworkData data = new NetworkData(LocalPlayerId, cmd, int1, int2, message);
			byte[] packet = data.ToByte();
			client.Send(packet, packet.Length);
		}
		private void SendPacket(NetworkCommand cmd)
		{
			SendPacket(cmd, 0, 0, "");
		}
		private void SendPacket(NetworkCommand cmd, int int1)
		{
			SendPacket(cmd, int1, 0, "");
		}
		private void SendPacket(NetworkCommand cmd, int int1, int int2)
		{
			SendPacket(cmd, int1, int2, "");
		}
		private void SendPacket(NetworkCommand cmd, string message)
		{
			SendPacket(cmd, 0, 0, message);
		}

		#endregion
	}
}
