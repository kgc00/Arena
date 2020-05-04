using System;
using System.Collections;
using Abilities.Modifiers;
using Stats;
using UnityEngine;
using Utils.NotificationCenter;

namespace Abilities.Buffs
{
    public class Conceal : BuffAbility
    {
        public override void AbilityActivated(Vector3 targetLocation)
        {
            Debug.Log("Handling activation of Conceal");
            StartCoroutine(HandleActivation());
        }

        private bool brokenConcealment = false;
        void BreakConcealment(object sender, object args)
        {
            if (args != this) brokenConcealment = true;
        }

        IEnumerator HandleActivation()
        {
            Debug.Log("Concealed!");
            float timeLeft = Duration;
            brokenConcealment = false;
            
            this.AddObserver(BreakConcealment, NotificationTypes.AbilityDidActivate);

            Owner.StatusComponent.AddStatus(Status.Hidden);

            var modifiers = Owner.AbilityComponent.Modifiers;
            var markModifier = new MarkOnHitModifier(null);
            modifiers.Add(markModifier);
            modifiers.Add(new DoubleDamageModifier(null));

            while (timeLeft > 0f && Owner.StatusComponent.Status.HasFlag(Status.Hidden))
            {
                if (brokenConcealment) break;
                
                timeLeft -= Time.deltaTime;
                yield return null;
            }
            
            if (Owner.StatusComponent.Status.HasFlag(Status.Hidden)) Owner.StatusComponent.RemoveStatus(Status.Hidden);

            if (modifiers.Contains(markModifier)) modifiers.Remove(markModifier);
            
            this.RemoveObserver(BreakConcealment, NotificationTypes.AbilityDidActivate);
            
            Debug.Log("Finished Concealment");
        }
    }
}