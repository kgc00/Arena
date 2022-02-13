using System;
using System.Collections;
using Data.Modifiers;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities.Modifiers {
    public class DoubleMovementSpeedModifier : BuffAbilityModifier {
        public DoubleMovementSpeedModifier(Ability ability) : base(ability) {
            Type = AbilityModifierType.DoubleMovementSpeed;
        }
        
        public override bool ShouldConsume() => false;

        public override void Handle() {
            Ability.OnActivation.Insert(0, AddModifier);
            base.Handle();
        }

        private IEnumerator AddModifier(Vector3 arg) {
            this.PostNotification(NotificationType.EnableDoubleMovementSpeed);
            yield break;
        }
    }
}