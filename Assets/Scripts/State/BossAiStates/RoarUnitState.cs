using System.Collections;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;

namespace State.BossAiStates {
    public class RoarUnitState : BossState {
        private static readonly int Roaring = Animator.StringToHash("Roaring");
        private Ability roar;
        private bool roaring;

        public RoarUnitState(Unit owner) : base(owner) {
            roar = Owner.AbilityComponent.GetEquippedAbility<Roar>();
            roar.OnAbilityFinished.Insert( 0,HandleRoarFinished);
        }

        ~RoarUnitState() => roar.OnAbilityFinished.Remove(HandleRoarFinished);

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleRoar());

        public IEnumerator HandleRoar() {
            if (Owner.Animator == null || !Owner.Animator) yield break;
            Owner.Animator.SetTrigger(Roaring);

            roaring = true;
            
            yield return new WaitForSeconds(roar.StartupTime);
            
            Owner.AbilityComponent.Activate(ref roar, Vector3.zero);
            
            yield return new WaitUntil(() => !roaring);
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Roaring);
        }

        private void HandleRoarFinished(Unit u, Ability a) {
            if (u != Owner || a != roar) return;
            roaring = false;
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (roaring) return null;
            return new IdleUnitState(Owner);
        }
    }
}