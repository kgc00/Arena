using System.Collections.Generic;
using Abilities.Modifiers;
using NUnit.Framework;
using UnityEngine;
using Stats;

namespace Tests
{
    public class StatsTest
    {
        [Test]
        public void Test()
        {
            var Stats = new Stats.Stats();
            var agi = Stats.StatFromEnum(StatType.Agility);

            Debug.Log("agi = " + agi);
            Assert.AreSame(agi, Stats.Agility);
        }
    }
}