namespace HandsEvaluator
{
    public static class HandEvaluator
    {
        private enum _suit { Diamonds, Hearts, Spades, Clubs }
        public enum HandType { HighCard, Pair, TwoPair, ThreeOfKind, Straight, Flush, FullHouse, FourOfKind, StraightFlush, RoyalFlush }

        private static CardComparer _comparer = new CardComparer();

        public static void GenerateHands(string filepath)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, int> LoadHands(string filepath)
        {
            throw new NotImplementedException();
        }

        public static float CalculateWinPercentage(ISet<string> cards)
        {
            throw new NotImplementedException();
        }

        private class CardComparer : IComparer<string>
        {
            public int CharToInt(char x)
            {
                if (x == '0') return 10;
                if (int.TryParse(x.ToString(), out int a)) return a;
                switch (x)
                {
                    case 'J': return 11;
                    case 'Q': return 12;
                    case 'K': return 13;
                    case 'A': return 14;
                }
                throw new ArgumentException($"Invalid card value: {x}");
            }

            public int Compare(string x, string y)
            {
                return CharToInt(x[0]) - CharToInt(y[0]);
            }
        }

        public static HandType GetHandType(ISet<string> cards)
        {
            if (cards == null) throw new ArgumentNullException("cards cant be null");

            List<string> sortedCards = new List<string>(cards);
            sortedCards.Sort(_comparer);

            List<int> matches = (List<int>)LengthOfAllMatches(sortedCards);
            bool isFlush = IsFlush(sortedCards, out int flushHigh);
            bool isStraight = IsStraight(sortedCards, out int straightHigh);

            if (isFlush && isStraight && flushHigh == straightHigh)
                if (flushHigh == 14)
                    return HandType.RoyalFlush;
                else
                    return HandType.StraightFlush;
            if (matches.Contains(4)) return HandType.FourOfKind;
            if (matches.Contains(3) && matches.Contains(2)) return HandType.FullHouse;
            if (isFlush) return HandType.Flush;
            if (isStraight) return HandType.Straight;
            if (matches.Contains(3)) return HandType.ThreeOfKind;
            if (matches.Count(x => x == 2) == 2) return HandType.TwoPair;
            if (matches.Contains(2)) return HandType.Pair;
            return HandType.HighCard;
        }

        private static IList<int> LengthOfAllMatches(IList<string> cards)
        {
            IList<int> matches = new List<int>();

            int currentMatchLength = 1;

            int i = 0;
            while(i < cards.Count - 1)
                for (int j = i + 1; j < cards.Count; j++)
                    if(cards[i][0] == cards[j][0])
                        currentMatchLength++;
                    else
                    {
                        matches.Add(currentMatchLength);
                        currentMatchLength = 1;
                        i = j;
                        break;
                    }
                i++;

            return (IList<int>) matches;
        }

        private static bool IsStraight(IList<string> cards, out int high)
        {
            Func<string, int> val = x => _comparer.CharToInt(x[0]);

            for (int i = 0; i < 3; i++)
                for (int j = i + 1; j < i + 6; j++)
                    if (val(cards[j]) != val(cards[i]) + (j - i)) break;
                    else if (j == i + 5)
                    {
                        high = j;
                        return true;
                    }
            high = -1;
            return false;
        }

        private static bool IsFlush(IList<string> cards, out int high)
        {
            int[] suits = new int[4];
            foreach (string card in cards)
                switch (card[1])
                {
                    case 'D':
                        high = suits.Max();
                        if (++suits[0] == 5) return true;
                        break;
                    case 'H':
                        high = suits.Max();
                        if (++suits[1] == 5) return true;
                        break;
                    case 'S':
                        high = suits.Max();
                        if (++suits[2] == 5) return true;
                        break;
                    case 'C':
                        high = suits.Max();
                        if (++suits[3] == 5) return true;
                        break;
                }
            high = -1;
            return false;
        }
    }
}