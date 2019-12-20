using UnityEngine;

namespace Abilities.AttackAbilities
{
    public class ShootCrossbow : AttackAbility
    {
        public override void Activate()
        {
            Debug.Log("shot crossbow");
        }

        public override void OnAbilityConnected(GameObject targetedUnit)
        {
            Debug.Log("crossbow bolt connected");
        }
    }
}