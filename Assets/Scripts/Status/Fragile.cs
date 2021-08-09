using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

namespace Status {
    // hurts unit when it collides with other objects
    public class Fragile : MonoStatus {
        protected override void EnableEffect() { }

        private void OnCollisionEnter(Collision other) {
            if (TryGetComponent<Unit>(out var unit)) {
                var amount = Mathf.Max(Amount, 1);
                unit.HealthComponent.DamageOwner(amount);
                Debug.Log($"Fragile - dealing ${amount} damage to ${unit.name}");
                Destroy(this);
            }
            
            // this.PostNotification(NotificationType.UnitDidCollide, new FragileCollision(other, this));
        }
    }

    internal class FragileCollision {
        private readonly Collision _other;
        private readonly Fragile _fragile;
        public FragileCollision(Collision other, Fragile fragile) {
            _other = other;
            _fragile = fragile;
        }
    }
}