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

        IEnumerator HandleActivation()
        {
            Debug.Log("Concealed!");
            var safety = 0f;
            
            float timeLeft = Duration;
            Owner.StatusComponent.AddStatus(Status.Hidden);
            
            Owner.AbilityComponent.Modifiers.Add(new MarkOnHitModifier(null));
            Owner.AbilityComponent.Modifiers.Add(new DoubleDamageModifier(null));
            
            // this.PostNotification(NotificationTypes.AbilityWillActivate, new AddValueModifier(1, 3));
            

            while (timeLeft > 0f && Owner.StatusComponent.Status.HasFlag(Status.Hidden))
            {
                    timeLeft -= Time.deltaTime;
                    yield return null;
            }
            
            if (Owner.StatusComponent.Status.HasFlag(Status.Hidden)) 
                Owner.StatusComponent.RemoveStatus(Status.Hidden);
            
            Debug.Log("Finished Concealment");
        }
    }
}