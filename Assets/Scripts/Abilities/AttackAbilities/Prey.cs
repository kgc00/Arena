﻿using System.Collections;
using System.Collections.Generic;
using Common;
using Data.Types;
using Projectiles;
using Units;
using UnityEngine;
using Utils;

namespace Abilities.AttackAbilities {
    public class Prey : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);
            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
            yield break;
        }


        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile) {
            if (projectile == null) return;

            projectile.GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnection, 10f);
        }

        private GameObject SpawnProjectile() {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;

            // find offset
            var spawnPos = new Vector3(position.x, 1, position.z) + (forward * 2);

            // find rotation
            var relativeOffset = spawnPos - position;
            var yEuler = Quaternion.LookRotation(relativeOffset, Vector3.up).eulerAngles.y;
            var rotation = Quaternion.Euler(0, yEuler, 0);

            // instantiation
            return Instantiate(
                Resources.Load($"{Constants.PrefabsPath}Projectile", typeof(GameObject)),
                spawnPos,
                rotation
            ) as GameObject;
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

            var totalDamage = Damage;
            var isMarked = unit.StatusComponent.StatusType.HasFlag(StatusType.Marked);
            if (isMarked) {
                totalDamage += 2;
                unit.StatusComponent.RemoveStatus(StatusType.Marked);
                MonoHelper.SpawnVfx(VfxType.MarkExplosion, unit.transform.position);
            }

            Debug.Log($"Prey has connected with a unit: {unit.name}.  The unit has a marked status of {isMarked}.\n" +
                      $"Base damage is {Damage}. Total Damage: {totalDamage}");

            unit.HealthComponent.DamageOwner(Damage, this, Owner);

            Destroy(projectile);
        }
    }
}