using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SmallWorld
{
	//The commands for interaction between server and client
	enum NetworkCommand
	{
		Login,
		Logout,
		InitMap,
		InitPlayers,
		Message,
		List,
		Move,
		Attack,
		EndTurn
	}


	/// <summary>
	/// The data structure by which the server and the client interact with each other
	/// </summary>
	class NetworkData
	{
		public IPEndPoint Client { get; protected set; }
		public int PlayerId { get; protected set; }
		public NetworkCommand Command { get; protected set; }
		public string strMessage;

		// Default constructor
		public NetworkData(int playerId, NetworkCommand cmd, string msg)
		{
			PlayerId = playerId;
			Command = cmd;
			strMessage = msg;
		}

		// Converts the received Bytes into an object of type NetworkData
		public NetworkData(IPEndPoint client, byte[] data)
		{
			Client = client;

			// The first four bytes store the length of the name
			int pos = 0;
			PlayerId = BitConverter.ToInt32(data, 0);
			// The next four bytes are for the Command
			pos += 4;
			Command = (NetworkCommand) BitConverter.ToInt32(data, pos);
			// The next four store the length of the message
			pos += 4;
			int msgLen = BitConverter.ToInt32(data, pos);
			// Finally the string message
			pos += 4;
			strMessage = (msgLen > 0) ? Encoding.UTF8.GetString(data, 12, msgLen) : null;

			// tmp
			GameMaster.GM.CurrentGame.OnRaiseNewChatMessage(">" + PlayerId + " : " + strMessage);
		}

		// Converts the Data structure into an array of bytes to send
		public byte[] ToByte()
		{
			List<byte> result = new List<byte>();

			result.AddRange(BitConverter.GetBytes((int) PlayerId)); // First four are for the Command
			result.AddRange(BitConverter.GetBytes((int) Command)); // Next four are for the Command

			result.AddRange(BitConverter.GetBytes((strMessage != null) ? strMessage.Length : 0)); // Length of the message
			if (strMessage != null) //And lastly we add the message text to our array of bytes
				result.AddRange(Encoding.UTF8.GetBytes(strMessage));

			return result.ToArray();
		}
	}
}
