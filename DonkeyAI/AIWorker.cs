using Donkey.Client;
using Donkey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DonkeyAI
{
	public class AIWorker
	{
		private readonly IGameClient _client;
		private Thread _thread;
		private bool _abort;
		private IGameStrategy _strategy;

		public AIWorker(IGameClient client, IGameStrategy strategy)
		{
			_client = client;
			_strategy = strategy;
		}

		public void Start()
		{
			if (_thread != null && _thread.IsAlive)
			{
				Stop();
				_thread.Join(1000);
			}
			_abort = false;
			_thread = new Thread(MainLoop) { IsBackground = true };
			_thread.Start();
		}

		public void Stop()
		{
			_abort = true;
		}

		private void MainLoop()
		{
			while (!_abort)
			{
				Thread.Sleep(200);
				_client.Update();
				var cardsInHand = _client.GetCards();
				var moveList = _client.GetHistory(_client.CurrentGameStep);

				if (moveList.Length == 0)
				{
					TryMoveOnEmptyTable(cardsInHand);
					continue;
				}

				var lastMove = moveList.LastOrDefault(x => x.MoveType == MoveType.Drop);
				if (lastMove == null)
				{
					TryMoveOnEmptyTable(cardsInHand);
					continue;
				}

				var move = _strategy.CalculateMove(cardsInHand, lastMove.Cards, false, false);
				if (move.MoveType == MoveType.Pass && _client.Pass())
					Console.WriteLine("AI Pass");
				else if (_client.MakeMove(move.Cards))
					WriteToConsole(move.Cards);
			}
		}

		private void TryMoveOnEmptyTable(List<Card> cardsInHand)
		{
			var move = _strategy.CalculateMove(cardsInHand, new List<Card>(), false, true);
			if (_client.MakeMove(move.Cards))
			{
				WriteToConsole(move.Cards);
				return;
			}
			move = _strategy.CalculateMove(cardsInHand, new List<Card>(), false, false);
			if (_client.MakeMove(move.Cards))
				WriteToConsole(move.Cards);
		}

		private void WriteToConsole(List<Card> cards)
		{
			Console.Write("AI Succesful move with cards: ");
			foreach (var c in cards)
				Console.Write(c.ToString() + " ");
			Console.WriteLine();
		}
	}
}
