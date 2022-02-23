namespace HandsEvaluator
{
    /// <summary>
    /// A class to evaluate the best 5 Card poker hand in a set of 7 cards
    /// 
    /// Planned functionality to add a lookup table to find winning percentage of hands
    /// </summary>
    public static class HandEvaluator
    {
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

        /// <summary>
        /// An IComparer used to sort string representation of cards
        /// based on the card value (i.e. '3', '7', 'K')
        /// </summary>
        private class CardComparer : IComparer<string>
        {
            /// <summary>
            /// converts the given character representing a card value to an int between 1 and 14
            /// </summary>
            /// <param name="x">char representing card value</param>
            /// <returns>int between 1 and 14 representing card value</returns>
            /// <exception cref="ArgumentException"></exception>
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

            /// <inheritdoc/>
            public int Compare(string x, string y)
            {
                return CharToInt(x[0]) - CharToInt(y[0]);
            }
        }

        /// <summary>
        /// Returns the type of the strongest 5 card Poker hand given 7 cards.
        /// 
        /// Cards are represented as two character strings with a value and suit
        /// Values in order are characters {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, J, Q, K}
        /// Suits can be any of characters {D, H, S, C}
        /// </summary>
        /// <param name="cards">A Set containing *EXACTLY* 7 elements, all describing a unique card using the method described</param>
        /// <returns>The Type of the strongest 5 card Poker hand (i.e. Pair, FourOfKind, RoyalFlush)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static HandType GetHandType(ISet<string> cards)
        {
            if (cards == null) throw new ArgumentNullException("cards cant be null");
            if (cards.Count != 7) throw new ArgumentException("must be 7 cards");

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

        /// <summary>
        /// returns how many of repetitions of each card value are in the hand
        /// </summary>
        /// <param name="cards">A Set containing *EXACTLY* 7 elements, all describing a unique card using the method described</param>
        /// <returns>List of numbers, representing how many of each card value (i.e. if 3 is in list, means there is a value repeated 3 times)</returns>
        private static IList<int> LengthOfAllMatches(IList<string> cards)
        {
            IList<int> matches = new List<int>();

            int currentMatchLength = 1; // at least 1 of each card

            int i = 0;
            while(i < cards.Count - 1) // because the cards are sorted, you can walk down the list, looking at cards to the right to see how many matches there are
                for (int j = i + 1; j < cards.Count; j++)
                    if(cards[i][0] == cards[j][0])
                        currentMatchLength++;
                    else
                    {
                        matches.Add(currentMatchLength);
                        currentMatchLength = 1;
                        i = j; //jumps i to the end of the matches so it doesn't double count mathces
                        break;
                    }
                i++;

            return (IList<int>) matches;
        }

        /// <summary>
        /// Determines whether a 5 card straight can be formed from a collection of 7 cards
        /// </summary>
        /// <param name="cards">A Set containing *EXACTLY* 7 elements, all describing a unique card using the method described</param>
        /// <param name="high">Highest card of Straight, -1 if there is none</param>
        /// <returns></returns>
        private static bool IsStraight(IList<string> cards, out int high)
        {
            Func<string, int> val = x => _comparer.CharToInt(x[0]);

            for (int i = 0; i < 3; i++) // straights can only start on 3rd card or lower
                for (int j = i + 1; j < i + 6; j++)
                    if (val(cards[j]) != val(cards[i]) + (j - i)) break; // makes sure j is as many higher as it is far away from i (i.e. if j is 3 over from i, it should be 3 higher)
                    else if (j == i + 5) // if it is valid and j is 5 away from i then it is a straight
                    {
                        high = j;
                        return true;
                    }
            high = -1;
            return false;
        }

        /// <summary>
        /// Determines whether a 5 card flush can be formed from a collection of 7 cards
        /// </summary>
        /// <param name="cards">A Set containing *EXACTLY* 7 elements, all describing a unique card using the method described</param>
        /// <param name="high">Highest card of Flush, -1 if there is none</param>
        /// <returns></returns>
        private static bool IsFlush(IList<string> cards, out int high)
        {
            Func<string, int> val = x => _comparer.CharToInt(x[0]);

            int[] suits = new int[4];
            int[] highs = new int[4]; // keeps track of high for all suits
            // keep track of how many of each suit there are and if any total 5 then there is a flush
            foreach (string card in cards)
                switch (card[1])
                {
                    case 'D':
                        highs[0] = val(cards[0]) > highs[0] ? val(card) : highs[0]; //updates highest card for suit if needed
                        if (++suits[0] == 5)
                        {
                            high = highs[0];
                            return true;
                        }
                        break;
                    case 'H':
                        highs[1] = val(cards[1]) > highs[1] ? val(card) : highs[1]; //updates highest card for suit if needed
                        if (++suits[1] == 5)
                        {
                            high = highs[1];
                            return true;
                        }
                        break;
                    case 'S':
                        highs[2] = val(cards[2]) > highs[2] ? val(card) : highs[2]; //updates highest card for suit if needed
                        if (++suits[2] == 5)
                        {
                            high = highs[2];
                            return true;
                        }
                        break;
                    case 'C':
                        highs[3] = val(cards[3]) > highs[3] ? val(card) : highs[3]; //updates highest card for suit if needed
                        if (++suits[3] == 5)
                        {
                            high = highs[3];
                            return true;
                        }
                        break;
                }
            high = -1;
            return false;
        }
    }
}