using Data.Modifiers;
using Status;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.Modifiers {
    public class DamageOnCollision : AttackAbilityModifier {
        public DamageOnCollision(Ability ability) : base(ability) {
            Type = AbilityModifierType.DamageOnCollision;
        }
        
        public override void Handle()
        {
            Debug.Log($"Calling {ToString()} to add fragile status.");
            Ability.OnAbilityConnection.Insert(0, AddFragile);
            base.Handle();
        }
        
        private void AddFragile(GameObject target, GameObject projectile = null)
        {
            if (target.transform.root.TryGetComponent<Unit>(out var unit)) {
                unit.gameObject.AddComponent<Fragile>().Initialize(unit, 5f, (int)Ability.Damage);
            }
        }

        public override bool ShouldConsume() => false;
    }
}