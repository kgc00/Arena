using System.Collections;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;

namespace State.BossAiStates {
    public class RoarUnitState : AbilityUnitState<Roar> {
        private static readonly int Roaring = Animator.StringToHash("Roaring");
        private Transform playerTransform;

        public RoarUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Roaring);
        }

        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator) yield break;
            Owner.Animator.SetTrigger(Roaring);

            yield return new WaitForSeconds(Ability.StartupTime);
            
            Owner.AbilityComponent.Activate(ref Ability, Vector3.zero);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (AbilityFinished != true) return null;
            return new ChainFlameUnitState(Owner, playerTransform);
        }
    }
}