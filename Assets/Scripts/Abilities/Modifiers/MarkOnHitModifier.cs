using Stats;
using Units;
using UnityEngine;

namespace Abilities.Modifiers
{
    public class MarkOnHitModifier : AttackAbilityModifier
    {
        public MarkOnHitModifier(Ability ability) : base(ability) { }

        public override void Handle()
        {
            Ability.OnAbilityConnection.Insert(0, AddMark);
            base.Handle();
        }
        
        private void AddMark(GameObject target, GameObject projectile = null)
        {
            var unit = target.transform.root.GetComponentInChildren<Unit>();
            if (unit == null) return;
            
            unit.StatusComponent.AddStatus(Status.Marked);
            Debug.Log(unit.StatusComponent.Status);
        }
    }
}