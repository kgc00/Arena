using System.Collections;
using Stats;
using UnityEngine;

namespace Abilities.Buffs
{
    public class Conceal : BuffAbility
    {
        public override void Activate(Vector3 targetLocation)
        {
            StartCoroutine(HandleActivation());
        }

        IEnumerator HandleActivation()
        {
            float timeLeft = Duration;
            Owner.StatusComponent.AddStatus(Status.Hidden);

            while (timeLeft > 0f && Owner.StatusComponent.Status.HasFlag(Status.Hidden))
            {
                    timeLeft -= Time.deltaTime;
                    yield return null;
            }
            
            Owner.StatusComponent.RemoveStatus(Status.Hidden);
            Debug.Log("Finished Concealment");
        }
    }
}