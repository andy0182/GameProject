using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DeenGames.Utils.AStarPathFinder;

namespace AutomatedTests
{
    [TestFixture]
    public class PathFinderHelperTests
    {
        [Test]
        public void TestRoundToNearestPowerOfTwoPositiveCases()
        {
            Dictionary<int, int> expectedAndActual = new Dictionary<int, int>();
            expectedAndActual.Add(1, 1);
            expectedAndActual.Add(2, 2);
            expectedAndActual.Add(3, 4);
            expectedAndActual.Add(4, 4);
            expectedAndActual.Add(5, 8);
            expectedAndActual.Add(6, 8);
            expectedAndActual.Add(7, 8);
            expectedAndActual.Add(8, 8);
            expectedAndActual.Add(9, 16);
            expectedAndActual.Add(10, 16);

            expectedAndActual.Add(100, 128);
            expectedAndActual.Add(253, 256);
            expectedAndActual.Add(261, 512);
            expectedAndActual.Add(777, 1024);
            expectedAndActual.Add(1111, 2048);
            expectedAndActual.Add(2227, 4096);
            expectedAndActual.Add(6086, 8192);
            expectedAndActual.Add(11398, 16384);
            expectedAndActual.Add(31313, 32768);

            foreach (int key in expectedAndActual.Keys)
            {
                Assert.That(PathFinderHelper.RoundToNearestPowerOfTwo(key) == expectedAndActual[key],
                    "Power of two for " + key + " was supposed to be " + expectedAndActual[key] + " but turned out to be " + PathFinderHelper.RoundToNearestPowerOfTwo(key));
            }
        }

        [Test]
        public void TestRoundToNearestPowerOfTwoWithNonPositiveNRaisesException()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => PathFinderHelper.RoundToNearestPowerOfTwo(-27));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => PathFinderHelper.RoundToNearestPowerOfTwo(-1));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => PathFinderHelper.RoundToNearestPowerOfTwo(0));
        }
    }
}
