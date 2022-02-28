using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Components;
using Data;
using Data.Params;
using Data.Types;
using State;
using Units;
using UnityEngine;
using Utils;
using Utils.NotificationCenter;

namespace Abilities.AttackAbilities {
    public class MissileStorm : AttackAbility {
        public override IEnumerator AbilityActivated(Vector3 targetLocation) {
            yield return new WaitForSeconds(StartupTime);
            this.PostNotification(NotificationType.DidCastDisrupt);
            OnAbilityActivationFinished(Owner, this);

            // todo
            
            ExecuteOnAbilityFinished();
        }

        
        protected override void AbilityConnected(GameObject other, GameObject projectile = null) {
            // todo
        }
    }
}