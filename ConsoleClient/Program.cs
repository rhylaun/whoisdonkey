﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Client;
using Donkey.Common;
using DonkeyAI;

namespace ConsoleClient
{
	class Program
	{
		static void Main(string[] args)
		{
			AIWorker aiWorker = null;

			Console.Clear();
			Console.Write("Enter server address: ");
			var address = Console.ReadLine();
			if (string.IsNullOrEmpty(address))
				address = "127.0.0.1";
			Console.Write("Enter login: ");
			var login = Console.ReadLine();
			Console.Write("Enter password: ");
			var password = Console.ReadLine();

			Console.WriteLine("Connecting to {0} as [{1}]", address, login);
			//using (var client = new GameClient<TcpNetworkClient>(new AuthData(login, password), address))
			using (var client = GameClientManager.CreateNew(new AuthData(login, password)))
			{
				bool abort = false;
				bool result = false;
				string message = string.Empty;

				//TODO: для отладки
				var register = client.Register();
				WriteToConsole(register, "Register: " + register);
				var auth = client.Auth();
				WriteToConsole(auth, "Auth: " + auth);
				//var join = client.JoinLobby("game");
				//WriteToConsole(join, "Join: " + join);
				//Thread.Sleep(500);
				//var start = client.StartGame();
				//WriteToConsole(start, "Start: " + start);
				//Thread.Sleep(500);
				//client.Update();


				while (!abort)
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write("{0}[{1}]>", client.AuthData.Login, client.State);
					var command = Console.ReadLine().Trim();
					try
					{
						if (string.IsNullOrEmpty(command)) continue;

						result = false;
						message = string.Empty;

						if (command.Equals("runai"))
						{
							if (aiWorker != null)
								aiWorker.Stop();
							aiWorker = AIManager.StartAI(client);
							WriteToConsole(true, "AI started.");
							continue;
						}

						if (command.Equals("stopai"))
						{
							if (aiWorker != null)
								aiWorker.Stop();
							WriteToConsole(true, "AI stopped.");
							continue;
						}

						if (command.Equals("exit"))
							abort = true;

						if (command.Equals("register"))
							result = client.Register();

						if (command.Equals("auth"))
							result = client.Auth();

						if (command.Equals("getlobbies"))
						{
							var lobbies = client.GetLobbies();
							result = true;
							foreach (var lobby in lobbies)
								message += lobby + Environment.NewLine;
							if (lobbies.Count == 0) message = "No lobby...";
						}

						if (command.Equals("leave"))
							result = client.Leave();

						if (command.Equals("start"))
							result = client.StartGame();

						if (command.Equals("cards"))
						{
							var cards = client.GetCards();
							result = true;
							foreach (var card in cards)
								message += card + Environment.NewLine;
							if (cards.Count == 0) message = "No cards...";
						}

						if (command.StartsWith("move"))
						{
							if (GetArgument(command, 1).Equals("pass"))
								result = client.Pass();
							else
							{
								var cards = ParseCards(command.Substring(4));
								result = client.MakeMove(cards);
							}
						}

						if (command.Equals("update"))
						{
							var lastStep = client.CurrentGameStep;
							result = client.Update();
							if (result)
							{
								try
								{
									var lastUpdateHistory = client.GetHistory(lastStep);
									message = WriteHistory(lastUpdateHistory);
								}
								catch (IndexOutOfRangeException)
								{
									result = false;
								}
							}
						}

						if (command.StartsWith("create"))
							result = client.CreateLobby(GetArgument(command, 1));

						if (command.StartsWith("join"))
							result = client.JoinLobby(GetArgument(command, 1));

						if (command.StartsWith("state"))
						{
							result = true;
							message = client.GetLobbyState().Players.Select(x => x.Name).Aggregate((x, y) => $"{x}, {y}");
						}

						if (string.IsNullOrEmpty(message))
							message = result.ToString();
						WriteToConsole(result, message);
					}
					catch (Exception ex)
					{
						WriteError(ex);
					}
				}
			}
		}

		private static void WriteError(Exception ex)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("=============== ERROR ===============");
			Console.Write("Message : " + ex.Message);
			Console.WriteLine();
			Console.Write("StackTrace : " + ex.StackTrace);
			Console.WriteLine("=========== END OF ERROR ============");
			Console.ForegroundColor = color;
		}

		private static string WriteHistory(GameMove[] history)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < history.Length; i++)
			{
				switch (history[i].MoveType)
				{
					case MoveType.Pass:
						builder.AppendLine(string.Format("{1} : [{0}] pass his turn", history[i].PlayerName, history[i].Index));
						break;
					case MoveType.Clear:
						builder.AppendLine(string.Format("{0} : Board was cleared!", history[i].Index));
						break;
					case MoveType.Drop:
						builder.Append(string.Format("{1} : [{0}] move with ", history[i].PlayerName, history[i].Index));
						foreach (var card in history[i].Cards)
							builder.Append(string.Format("[{0}]", card));
						builder.AppendLine();
						break;
				}
			}

			return builder.ToString();
		}

		private static void WriteToConsole(bool success, string text)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
			Console.WriteLine("Result : {0}", text);
			Console.ForegroundColor = color;
		}

		private static string GetArgument(string value, int index)
		{
			return value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[index];
		}

		private static List<Card> ParseCards(string cards)
		{
			var result = new List<Card>();

			var tokens = cards.Trim(' ').Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (tokens.Count() > 3) throw new ArgumentException("Too many cards.");
			if (tokens.Count() < 1) throw new ArgumentException("Too low cards. Zero actually...");

			foreach (var str in tokens)
			{
				int num = 0;
				if (int.TryParse(str, out num))
				{
					result.Add((Card)num);
					continue;
				}

				try
				{
					Card card = (Card)Enum.Parse(typeof(Card), str);
					result.Add(card);
					continue;
				}
				catch
				{
					throw new ArgumentException("Can't parse some card");
				}
			}

			return result;
		}
	}
}
