using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Modifiers;
using Data.Types;
using Stats;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities.Buffs
{
    public class Conceal : BuffAbility
    {
        public override IEnumerator AbilityActivated(Vector3 targetLocation)
        {
            Debug.Log("Handling activation of Conceal");
            Debug.Log("Concealed!");
            float timeLeft = Duration;
            brokenConcealment = false;
            
            this.AddObserver(BreakConcealment, NotificationType.AbilityDidActivate);
            
            Owner.StatusComponent.AddStatus(StatusType.Hidden);

            OnAbilityActivationFinished(Owner, this);

            var modifiers = Owner.AbilityComponent.Modifiers;
            var markModifier = new MarkOnHitModifier(null);
            modifiers.Add(markModifier);
            modifiers.Add(new DoubleDamageModifier(null));

            while (timeLeft > 0f && Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden))
            {
                if (brokenConcealment) break;
                
                timeLeft -= Time.deltaTime;
                yield return null;
            }
            
            if (Owner.StatusComponent.StatusType.HasFlag(StatusType.Hidden)) Owner.StatusComponent.RemoveStatus(StatusType.Hidden);

            if (modifiers.Contains(markModifier)) modifiers.Remove(markModifier);
            
            this.RemoveObserver(BreakConcealment, NotificationType.AbilityDidActivate);
            Debug.Log("Finished Concealment");
        }

        private bool brokenConcealment = false;
        void BreakConcealment(object sender, object args)
        {
            if (args != this) brokenConcealment = true;
        }
    }
}