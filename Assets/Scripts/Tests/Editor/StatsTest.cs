using Data.Stats;
using Data.Types;
using NUnit.Framework;

namespace Tests.Editor
{
    public class StatsTest
    {
        [Test]
        public void Test()
        {
            var Stats = new Stats();
            var ms = Stats.StatFromEnum(StatType.MovementSpeed);

            Assert.AreSame(ms, Stats.MovementSpeed);
        }
    }
}