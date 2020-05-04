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
            
            float timeLeft = Duration;
            Owner.StatusComponent.AddStatus(Status.Hidden);

            var markModifier = new MarkOnHitModifier(null);
            Owner.AbilityComponent.Modifiers.Add(markModifier);
            Owner.AbilityComponent.Modifiers.Add(new DoubleDamageModifier(null));

            while (timeLeft > 0f && Owner.StatusComponent.Status.HasFlag(Status.Hidden))
            {
                    timeLeft -= Time.deltaTime;
                    yield return null;
            }
            
            if (Owner.StatusComponent.Status.HasFlag(Status.Hidden)) Owner.StatusComponent.RemoveStatus(Status.Hidden);
            
            // if (Owner.AbilityComponent.Modifiers.Contains(markModifier)) Owner.AbilityComponent.Modifiers.Remove()
            // Debug.Log($"Mark modifier still in modifiers list: {answer}");
            
            Debug.Log("Finished Concealment");
        }
    }
}