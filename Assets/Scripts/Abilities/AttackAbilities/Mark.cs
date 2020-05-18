using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Enums;
using Projectiles;
using Stats;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities
{
    public class Mark : AttackAbility
    {
        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            yield return new WaitForSeconds(StartupTime);
            MonoHelper.SpawnProjectile(Owner.gameObject, targetLocation, OnAbilityConnection);
        }
        
        public override void AbilityConnected(GameObject other, GameObject projectile)
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
            
            
            StatusHelper.AddMark(unit);
            var moveDirection = other.transform.position - projectile.transform.position;
            other.GetComponentInParent<Rigidbody>()?.AddForce( moveDirection.normalized * 7500f);
            Destroy(projectile);
        }
    }
}