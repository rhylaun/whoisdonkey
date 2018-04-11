using System;
using Donkey.Server.CommandProcessors;
using Donkey.Common;
using Donkey.Common.Commands;
using System.Reflection;

namespace Donkey.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Clear();

			Console.WriteLine("Initializing command parser...");
			CommandParser.Init();

			Console.WriteLine("Initializing processors factory...");
			CommandProcessorFactory.Init();

			Console.WriteLine("Loading AI modules...");
			var aiAssembly = Assembly.LoadFrom("DonkeyAI.dll");
			AppDomain.CurrentDomain.Load(aiAssembly.FullName);
			var aiFactory = new AIFactory();
			aiFactory.Init();

			Console.WriteLine("Initializing game server... ");
			using (var gameServer = new GameServer(aiFactory))
			{
				gameServer.Load();

				Console.WriteLine("Initializing request server... ");
				var networkServer = new NetworkServer<TcpRequestServer>(gameServer, Defaults.Port);
				networkServer.Start();

				//TODO: для отладки
				gameServer.CreateLobby("game");

				foreach (var rec in gameServer.Statistic.Records)
					Console.WriteLine($"{rec.Name} {rec.GamesPlayed} {rec.TotalScore} {rec.FinishWithDonkey}");

				Console.WriteLine("Now you can type commands!");
				bool abort = false;
				while (!abort)
				{
					var commandText = Console.ReadLine();
					ClientCommand command = null;
					try
					{
						command = CommandParser.Parse(commandText);
					}
					catch (GameServerException ex)
					{
						Console.WriteLine("Parse error: {0}", ex.Message);
						continue;
					}

					try
					{
						var processor = CommandProcessorFactory.GetProcessor(command);
						var result = processor.ExecuteOn(gameServer);
						Console.WriteLine("Command result: {0}", processor.Answer.Success);
						if (processor.Answer.Success)
							Console.WriteLine(processor.Answer.ToString());
					}
					catch (GameServerException ex)
					{
						Console.WriteLine("Exception: {0}", ex.Message);
					}
					catch (NotImplementedException ex)
					{
						Console.WriteLine("No implementation: {0}", ex.Message);
					}
				}
			}
		}
	}
}
