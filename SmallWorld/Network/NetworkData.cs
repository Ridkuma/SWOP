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
		LoginAccepted,		// (newPlayerId)
		NewPlayer,			// (newPlayerId, factionId, name)
		InitMap,			// (mapStrategy, randomSeed)
		GameStart,			// ()
		GameEnd,			// ()
		NextPlayer,			// ()
		Message,			// (0, 0, message)
		Move,
		Attack,
		
		ClientLogin,		// (0, factionId, name)
		ClientLogout,
		ClientGiveIpEP,		// (0, port, ipToParse)
		ClientNextPlayer,	// ()
		ClientMessage,		// (0, 0, message)
	}


	/// <summary>
	/// The data structure by which the server and the client interact with each other
	/// </summary>
	class NetworkData
	{
		public int PlayerId { get; protected set; }
		public NetworkCommand Command { get; protected set; }
		public int ArgsInt1 { get; protected set; }
		public int ArgsInt2 { get; protected set; }
		public string ArgsString { get; protected set; }

		// Default constructor
		public NetworkData(int playerId, NetworkCommand cmd, int argsInt1, int argsInt2, string msg)
		{
			PlayerId = playerId;
			Command = cmd;
			ArgsInt1 = argsInt1;
			ArgsInt2 = argsInt2;
			ArgsString = msg;
		}

		// Converts the received Bytes into an object of type NetworkData
		public NetworkData(IPEndPoint client, byte[] data)
		{
			// The first 4 bytes store the target of the message
			int pos = 0;
			PlayerId = BitConverter.ToInt32(data, 0);
			// The next 4 bytes are for the Command
			pos += 4;
			Command = (NetworkCommand) BitConverter.ToInt32(data, pos);
			// The next 8 bytes are for the two int parameters
			pos += 4;
			ArgsInt1 = BitConverter.ToInt32(data, pos);
			pos += 4;
			ArgsInt2 = BitConverter.ToInt32(data, pos);
			// The next 4 store the length of the string message
			pos += 4;
			int strLen = BitConverter.ToInt32(data, pos);
			// Finally the string message
			pos += 4;
			ArgsString = (strLen > 0) ? Encoding.UTF8.GetString(data, pos, strLen) : null;

			// tmp
			GameMaster.GM.CurrentGame.OnRaiseNewChatMessage("[" + PlayerId + "]" + Command + " (" + ArgsInt1 + "," + ArgsInt2 + ",'" + ArgsString + "')");
		}

		// Converts the Data structure into an array of bytes to send
		public byte[] ToByte()
		{
			List<byte> result = new List<byte>();

			result.AddRange(BitConverter.GetBytes((int) PlayerId)); // 4 first byte = Player Id
			result.AddRange(BitConverter.GetBytes((int) Command)); // 4 next = Command

			result.AddRange(BitConverter.GetBytes((int) ArgsInt1)); // ArgsInt1
			result.AddRange(BitConverter.GetBytes((int) ArgsInt2)); // ArgsInt2

			result.AddRange(BitConverter.GetBytes((ArgsString != null) ? ArgsString.Length : 0)); // Length of the message
			if (ArgsString != null) //And lastly we add the message text to our array of bytes
				result.AddRange(Encoding.UTF8.GetBytes(ArgsString));

			return result.ToArray();
		}
	}
}
