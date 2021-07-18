using System.Collections;
using System.Linq;
using Data.AbilityData;
using Data.Types;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Mark : AttackAbility {
        private Material _fresnel;

        public override AttackAbility Initialize(AttackAbilityData data, Unit owner) {
            _fresnel = MonoHelper.LoadMaterial(MaterialType.MarkOutline);
            return base.Initialize(data, owner);
        }

        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            var proj = MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection, 10f);
            var renderer = proj.transform.root.GetComponentInChildren<Renderer>();
            var withFresnel = renderer.materials.ToList();
            withFresnel.Add(_fresnel);
            renderer.materials = withFresnel.ToArray();

            OnAbilityActivationFinished(Owner, this);
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile) {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();

            if (hitGeometry) {
                Destroy(projectile);
                return;
            }

            if (unit == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;


            StatusHelper.AddMark(unit);
            var moveDirection = other.transform.position - projectile.transform.position;
            other.GetComponentInParent<Rigidbody>()?.AddForce(moveDirection.normalized * 7500f);
            Destroy(projectile);
        }
    }
}