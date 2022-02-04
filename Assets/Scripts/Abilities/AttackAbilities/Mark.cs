﻿using System.Collections;
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
            ExecuteOnAbilityFinished();
        }

        protected override void AbilityConnected(GameObject other, GameObject projectile) {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();

            if (hitGeometry) {
                Destroy(projectile);
                return;
            }

            if (unit == null || unit.Owner == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;


            StatusHelper.AddMark(unit);
            var heading = other.transform.position - projectile.transform.position;
            heading.y = 0;
            heading = heading.normalized;
            other.GetComponentInParent<Rigidbody>()?.AddForce(heading * Force, ForceMode.Impulse);
            MonoHelper.SpawnVfx(VfxType.EnemyImpact, projectile.transform.position);
            Destroy(projectile);
        }
    }
}