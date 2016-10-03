using System;
using System.Collections.Generic;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GameMove, NeedAuth = true)]
    public class GameMoveCommand : ClientCommand
    {
        public readonly MoveType MoveType;
        public readonly List<Card> Cards;

        public GameMoveCommand(AuthData authData, MoveType moveType, List<Card> cards)
            : base(authData, CommandType.GameMove)
        {
            MoveType = moveType;
            Cards = new List<Card>(cards);
        }
    }
}
