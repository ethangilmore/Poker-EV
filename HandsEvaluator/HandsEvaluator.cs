namespace HandsEvaluator
{
    public static class HandsEvaluator
    {
        private enum _handType { HighCard, Pair, TwoPair, ThreeOfKind, Straight, Flush, FullHouse, FourOfKind, StraightFlush, RoyalFlush }

        public static void GenerateHands(string filepath)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, int> LoadHands(string filepath)
        {
            throw new NotImplementedException();
        }

        public static float CalculateWinPercentage(string hand)
        {
            throw new NotImplementedException();
        }

        private static _handType GetHandType(string hand)
        {
            throw new NotImplementedException();
        }
    }
}