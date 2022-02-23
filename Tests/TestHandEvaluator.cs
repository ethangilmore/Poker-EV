using HandsEvaluator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TestHandEvaluator
    {
        [TestMethod]
        public void StressTest()
        {
            string val = "1234567890JQK";
            string suit = "DHSC";

            Random r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                HashSet<string> cards = new HashSet<string>() { };

                for (int j = 0; j < 7; j++)
                    cards.Add(val[r.Next(13)].ToString() + suit[r.Next(4)].ToString());
                HandEvaluator.GetHandType(cards);
            }
        }

        [TestMethod]
        public void TestStraight()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "7C", "0S", "6H", "2S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.Straight, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestFlush()
        {
            HashSet<string> cards = new HashSet<string>() { "5S", "7C", "0S", "6H", "2S", "8S", "9S" };
            Assert.AreEqual(HandEvaluator.HandType.Flush, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestFullHouse()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "2C", "5S", "5H", "2S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.FullHouse, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestTwoPair()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "2C", "5S", "3H", "2S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.TwoPair, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestFourOfKind()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "5C", "5S", "5H", "2S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.FourOfKind, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestThreeOfKind()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "2C", "5S", "5H", "6S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.ThreeOfKind, HandEvaluator.GetHandType(cards));
        }

        [TestMethod]
        public void TestDouble()
        {
            HashSet<string> cards = new HashSet<string>() { "5D", "2C", "5S", "KH", "6S", "8S", "9H" };
            Assert.AreEqual(HandEvaluator.HandType.Pair, HandEvaluator.GetHandType(cards));
        }
    }
}