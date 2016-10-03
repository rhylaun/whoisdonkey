using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common;

namespace Donkey.Server
{
    public static class CardShuffler
    {
        private static readonly Dictionary<Card, int> CardSet = new Dictionary<Card, int>()
        {
            {Card.One, 13},
            {Card.Two, 13},
            {Card.Three, 13},
            {Card.Four, 13},
            {Card.Five, 13},
            {Card.Six, 13},
            {Card.Seven, 13},
            {Card.Eight, 13},
            {Card.Nine, 13},
            {Card.Ten, 13},
            {Card.Eleven, 13},
            {Card.Twelve, 13},
            {Card.Thirteen, 13},
            {Card.Joker, 5},
            {Card.Donkey,1}
        };

        private static readonly Dictionary<int, int> CardsPerPlayer = new Dictionary<int, int>()
        {
            {1,13},//TODO: тестовые раздачи, потом надо удалить
            {2,13},

            {3,13},
            {4,13},
            {5,13},
            {6,13},
            {7,13},
            {8,13},
            {9,13},
            {10,13},
            {11,13},
            {12,13},
        };

        public static GameCardSet GetCardSet(int playersCount)
        {
            var result = new List<PlayerCardSet>();
            for (int i = 0; i < playersCount; i++)
                result.Add(new PlayerCardSet());

            var random = new Random();
            var currentSet = new Dictionary<Card, int>(CardSet);

            var cardsTotal = CardsPerPlayer[playersCount] * playersCount;
            var cardsCurrent = 0;
            var playerIndex = 0;
            while (cardsCurrent < cardsTotal)
            {
                var x = (Card)random.Next(1, 14);
                if (currentSet[x] > 0)
                {

                    result[playerIndex++ % playersCount][x] += 1;
                    currentSet[x]--;
                    cardsCurrent++;
                }
            }

            var donkeyPlayer = random.Next(playersCount);
            while (true)
            {
                var x = (Card)random.Next(1, 14);
                if (result[donkeyPlayer].Contains(x) && result[donkeyPlayer][x] > 0)
                {
                    result[donkeyPlayer][x]--;
                    result[donkeyPlayer][Card.Donkey] = 1;
                    break;
                }
            }
            return new GameCardSet(result);
        }
    }
}
