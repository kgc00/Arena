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
            var agi = Stats.StatFromEnum(StatType.Agility);

            Debug.Log("agi = " + agi);
            Assert.AreSame(agi, Stats.Agility);
        }
    }
}