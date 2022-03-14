using System;
using Data.Pickups;
using Data.Types;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Components {
    [RequireComponent(typeof(BoxCollider))]
    public class PickupComponent : MonoBehaviour {
        [SerializeField] private DropType Type;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Transform _vfxSpawnTransform;
        private void OnTriggerEnter(Collider other) {
            var unit = other.gameObject.GetUnitComponent();
            if (unit == null || unit.Owner.ControlType != ControlType.Local) return;

            switch (Type) {
                case DropType.HealthPickupSmall:
                    if (unit.HealthComponent.IsFullHealth) return;
                    unit.HealthComponent.HealOwner(unit.HealthComponent.MaxHp * 0.1f);
                    this.PostNotification(NotificationType.DidPickupHealth);
                    MonoHelper.SpawnVfx(VfxType.HealPickup, _vfxSpawnTransform.position);
                    break;
                case DropType.HealthPickupLarge:
                    if (unit.HealthComponent.IsFullHealth) return;
                    unit.HealthComponent.HealOwner(unit.HealthComponent.MaxHp * 0.3f);
                    this.PostNotification(NotificationType.DidPickupHealth);
                    MonoHelper.SpawnVfx(VfxType.HealPickup, _vfxSpawnTransform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _collider.enabled = false;
            Destroy(gameObject);
        }
    }
}