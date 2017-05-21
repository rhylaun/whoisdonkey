using Donkey.Client;
using Donkey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
				Thread.Sleep(500);
				if (!_client.IsMyTurn)
					continue;

				if (!_client.Update())
					continue;

				var cardsInHand = _client.GetCards();
				var moveList = _client.GetHistory(0);
				var donkeyRound = DetermineDonkeyRound(moveList);
				var cardsOnTable = FindCardsOnTable(moveList);

				var move = _strategy.CalculateMove(cardsInHand, cardsOnTable, donkeyRound, !donkeyRound);
				if (TryMove(move))
					continue;

				move = _strategy.CalculateMove(cardsInHand, cardsOnTable, false, false);
				TryMove(move);
			}
		}

		private bool TryMove(GameMove move)
		{
			if (move.MoveType == MoveType.Pass && _client.Pass())
			{
				WriteToConsole(move.Cards);
				return true;
			}
			else if (_client.MakeMove(move.Cards))
			{
				WriteToConsole(move.Cards);
				return true;
			}

			return false;
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
			if (cards.Count == 0)
			{
				Console.WriteLine("AI Pass");
				return;
			}

			Console.Write("AI move with cards: ");
			foreach (var c in cards)
				Console.Write(c.ToString() + " ");
			Console.WriteLine();
		}

		private bool DetermineDonkeyRound(GameMove[] moves)
		{
			try
			{
				var reversed = moves.Reverse().ToList();
				var clearMove = reversed.FirstOrDefault(x => x.MoveType == MoveType.Clear);
				var roundIndex = reversed.IndexOf(clearMove) - 1;
				return reversed[roundIndex].IsDonkeyMove();
			}
			catch
			{
			}
			return false;
		}

		private List<Card> FindCardsOnTable(GameMove[] moves)
		{
			try
			{
				var reversed = moves.Reverse().ToList();
				for (int i = 0; i < reversed.Count; i++)
				{
					if (reversed[i].MoveType == MoveType.Drop)
						return reversed[i].Cards;
					if (reversed[i].MoveType == MoveType.Clear)
						return new List<Card>();
				}
			}
			catch
			{
			}
			return new List<Card>();
		}
	}
}
