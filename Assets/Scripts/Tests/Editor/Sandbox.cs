using System.Collections.Generic;
using Abilities.Modifiers;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class Sandbox
    {
        [Test]
        public void Test()
        {
            var modifier = new MarkOnHitModifier(null);
            var ddMod = new DoubleDamageModifier(null);
            List<AbilityModifier> Modifiers = new List<AbilityModifier>();
            Modifiers.Add(modifier);
            Modifiers.Add(ddMod);

            Debug.Log($"Count: {Modifiers.Count}");

            Assert.Contains(modifier, Modifiers);
            Assert.AreSame(modifier, Modifiers[0]);

            Modifiers.Remove(modifier);

            Assert.AreSame(ddMod, Modifiers[0]);
            Assert.AreEqual(Modifiers.Count, 1);
        }
    }
}