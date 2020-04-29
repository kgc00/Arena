﻿using System.Collections;
using Enums;
using Projectiles;
using Stats;
using Units;
using UnityEngine;

namespace Abilities.AttackAbilities
{
    public class Mark : AttackAbility
    {
        public override void Activate(Vector3 targetLocation)
        {
            Debug.Log($"Is on cooldown: {Cooldown.IsOnCooldown}");
            if (Cooldown.IsOnCooldown) return;

            StartCoroutine(HandleActivation(targetLocation));
        }

        private IEnumerator HandleActivation(Vector3 targetLocation)
        {
            yield return new WaitForSeconds(StartupTime);
            
            var projectile = SpawnProjectile();
            InitializeProjectile(targetLocation, projectile);

            Cooldown.SetOnCooldown();
        }


        private void InitializeProjectile(Vector3 targetLocation, GameObject projectile)
        {
            if (projectile == null) return;
            
            projectile.GetComponent<ProjectileComponent>().Initialize(targetLocation, OnAbilityConnected, 10f);
        }

        private GameObject SpawnProjectile()
        {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;
            
            // find offset
            var spawnPos = new Vector3(position.x, 1, position.z) + (forward * 2);
            
            // find rotation
            var relativeOffset = spawnPos - position;
            var yEuler = Quaternion.LookRotation(relativeOffset, Vector3.up).eulerAngles.y;
            var rotation = Quaternion.Euler(0, yEuler,0);

            // instantiation
            return  Instantiate(
                Resources.Load("Projectiles/Projectile", typeof(GameObject)),
                spawnPos, 
                rotation
            ) as GameObject;
        }

        public override void OnAbilityConnected(GameObject other, GameObject projectile)
        {
            var hitGeometry = other.gameObject.CompareTag(Tags.Board.ToString());
            var unit = other.transform.root.GetComponentInChildren<Unit>();
            
            
            if (hitGeometry)
            {
                Destroy(projectile);
                return;
            }

            if (unit == null) return;
            if (!AffectedFactions.Contains(unit.Owner.ControlType)) return;
            
            unit.HealthComponent.AdjustHealth(-Damage);
            AddMark(unit);
            Destroy(projectile);
        }

        private void AddMark(Unit unit)
        {
            unit.StatusComponent.AddStatus(Status.Marked);
            Debug.Log(unit.StatusComponent.Status);
        }
    }
}