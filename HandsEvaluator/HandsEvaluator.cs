namespace HandsEvaluator
{
    public static class HandsEvaluator
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
                if (!double.TryParse(x.ToString(), out double a))
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

        private static HandType GetHandType(ISet<string> cards)
        {
            if (cards == null) throw new ArgumentNullException("cards cant be null");

            List<string> sortedCards = new List<string>(cards);
            sortedCards.Sort(_comparer);

            List<int> matches = (List<int>)LengthOfAllMatches(sortedCards);
            bool isFlush = IsFlush(sortedCards);
            bool isStraight = IsStraight(sortedCards);

            // TODO: implement Royal Flush
            if (isFlush && isStraight) return HandType.StraightFlush;
            if (matches.Contains(4)) return HandType.FourOfKind;
            if (matches.Contains(3) && matches.Contains(2)) return HandType.FullHouse;
            if (isFlush) return HandType.Flush;
            if (isStraight) return HandType.Straight;
            if (matches.Contains(3)) return HandType.ThreeOfKind;
            if (matches.Remove(2) && matches.Contains(2)) return HandType.TwoPair;
            return HandType.HighCard;
        }

        private static IList<int> LengthOfAllMatches(IList<string> cards)
        {
            IList<int> matches = new List<int>();

            int currentMatchLength = 0;

            for (int i = 0; i < cards.Count - 1; i++)
                for (int j = i + i; j < cards.Count; j++)
                    if(cards[i][0] == cards[j][0])
                        currentMatchLength++;
                    else
                    {
                        matches.Add(currentMatchLength);
                        currentMatchLength = 0;
                        break;
                    }

            return (IList<int>) matches;
        }

        private static bool IsStraight(IList<string> cards)
        {
            Func<string, int> val = x => _comparer.CharToInt(x[0]);

            for (int i = 0; i < 3; i++)
                for (int j = i + 1; j < i + 6; i++)
                    if (val(cards[j]) != val(cards[i]) + (j - i)) break;
                    else if (j == i + 5) return true;
            return false;
        }

        private static bool IsFlush(IList<string> cards)
        {
            int[] suits = new int[4];
            foreach (string card in cards)
                switch (card[1])
                {
                    case 'D':
                        if (++suits[0] == 5) return true;
                        break;
                    case 'H':
                        if (++suits[1] == 5) return true;
                        break;
                    case 'S':
                        if (++suits[2] == 5) return true;
                        break;
                    case 'C':
                        if (++suits[3] == 5) return true;
                        break;
                }
            return false;
        }
    }
}