using System.Collections.Generic;
using Abilities.Modifiers;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class AbilityModifiersTest
    {
        [Test]
        public void Test()
        {
            var modifier = new MarkOnHitModifier(null);
            var ddMod = new DoubleDamageModifier(null);
            List<AbilityModifier> Modifiers = new List<AbilityModifier>();
            Modifiers.Add(modifier);
            Modifiers.Add(ddMod);

            Assert.Contains(modifier, Modifiers);
            Assert.AreSame(modifier, Modifiers[0]);

            Modifiers.Remove(modifier);

            Assert.AreSame(ddMod, Modifiers[0]);
            Assert.AreEqual(Modifiers.Count, 1);

            Modifiers.RemoveAll(x => x.ShouldConsume());
            
            Assert.AreEqual(0, Modifiers.Count);
        }
    }
}