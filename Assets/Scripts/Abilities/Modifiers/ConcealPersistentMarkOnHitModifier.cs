using System.Collections;
using Abilities.Buffs;
using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.Modifiers
{
    public class ConcealPersistentMarkOnHitModifier : BuffAbilityModifier
    {
        public ConcealPersistentMarkOnHitModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.ConcealPersistentAddMarkOnHit;
        }
        public override bool ShouldConsume() => false;
        public override void Handle()
        {
            Ability.OnActivation.Insert(0, EnableConcealPersistentAddMarkOnHit);
            base.Handle();
        }

        private IEnumerator EnableConcealPersistentAddMarkOnHit(Vector3 arg) {
            this.PostNotification(NotificationType.EnableConcealPersistentAddMarkOnHit);
            yield break;
        }
    }
}