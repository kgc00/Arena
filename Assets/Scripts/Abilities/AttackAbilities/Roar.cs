using System.Collections;
using System.Linq;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Abilities.AttackAbilities {
    public class Roar : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            var units = FindObjectsOfType<Unit>().Where(u => AffectedFactions.Contains(u.Owner.ControlType)).ToList();

            if (units.Count == 0) {
                OnAbilityFinished(Owner, this);
                yield break;
            }

            foreach (var u in units) AbilityConnected(u.gameObject);

            var timeLeft = Duration;
            while (timeLeft > 0) {
                foreach (var u in units) {
                    if (u == null) continue;

                    var dist = Vector3.Distance(u.transform.position, Owner.transform.position);
                    if (dist > Range) continue;
                    
                    var heading = u.transform.position - Owner.transform.position;
                    heading.y = 0;
                    var force = heading.normalized * Force;
                    u.Rigidbody.AddForce(force);
                }

                timeLeft = Clamp(timeLeft - Time.deltaTime, 0, Duration);
                yield return null;
            }

            OnAbilityFinished(Owner, this);
        }

        protected override void AbilityConnected(GameObject target, GameObject projectile = null) {
            target.GetComponent<Unit>().HealthComponent.DamageOwner(Damage, this, Owner);
        }
    }
}