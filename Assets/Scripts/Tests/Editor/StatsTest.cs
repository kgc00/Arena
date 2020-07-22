using Components;
using Data.Stats;
using Data.Types;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class StatsTest
    {
        [Test]
        public void Test()
        {
            var Stats = new Stats();
            var ms = Stats.StatFromEnum(StatType.MovementSpeed);

            Debug.Log("ms = " + ms);
            Assert.AreSame(ms, Stats.MovementSpeed);
        }
    }
}