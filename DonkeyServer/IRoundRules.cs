using System.Collections.Generic;
using Donkey.Common;

namespace Donkey.Server
{
	internal interface IRoundRules
	{
		bool ValidateMove(GameMove pendingMove, List<Card> topCards);		
		bool ChangeWinner(GameMove currentWinnerMove, GameMove pendingMove);
		GameMove GetTakeMove(AuthData player, List<Card> cards);
	}
}
