using System.Collections;
using System.Collections.Generic;
using Common;
using Data.Types;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class Prey : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.DidCastPrey);
            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);
            OnAbilityActivationFinished(Owner, this);
            ExecuteOnAbilityFinished();
        }


        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile) {
            if (projectile == null) return;

            projectile.GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnection, ProjectileSpeed);
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

            var isMarked = unit.StatusComponent.IsMarked();
            if (isMarked) {
                unit.StatusComponent.TriggerStatus(StatusType.Marked, this);
            } else {
                unit.HealthComponent.DamageOwner(Damage, this, Owner);
            }
            this.PostNotification(NotificationType.DidConnectPrey);
            MonoHelper.SpawnVfx(VfxType.EnemyImpact, projectile.transform.position);
            Destroy(projectile);
        }
    }
}