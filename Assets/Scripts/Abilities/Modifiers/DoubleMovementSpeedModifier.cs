using System;
using System.Collections;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities.Modifiers {
    public class DoubleMovementSpeedModifier : BuffAbilityModifier {
        public DoubleMovementSpeedModifier(Ability ability) : base(ability) {
        }
        
        public override bool ShouldConsume() => false;

        public override void Handle() {
            Debug.Log($"Calling {ToString()} to add a mark on collision.");
            Ability.OnActivation.Insert(0, AddModifier);
            base.Handle();
        }

        private IEnumerator AddModifier(Vector3 arg) {
            NotificationCenter.instance.PostNotification(NotificationType.EnableDoubleMovementSpeed);
            yield break;
        }
    }
}