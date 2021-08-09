using System.Collections;
using Abilities;
using Abilities.Buffs;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BossAiStates {
    public class MagicShieldUnitState : UnitState {
        private static readonly int Guarding = Animator.StringToHash("Guarding");
        private Ability magicShield;
        private bool shieldActive;

        public MagicShieldUnitState(Unit owner) : base(owner) {
            magicShield = Owner.AbilityComponent.GetEquippedAbility<MagicShield>();
            magicShield.OnAbilityFinished.Insert(0, HandleMagicShieldFinished);
        }
        
        ~MagicShieldUnitState() => magicShield.OnAbilityFinished.Remove(HandleMagicShieldFinished);

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleShield());
        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Guarding);
        }

        private IEnumerator HandleShield() {
            if (Owner.Animator == null || !Owner.Animator) yield break;
            Owner.Animator.SetTrigger(Guarding);

            shieldActive = true;
            
            yield return new WaitForSeconds(magicShield.StartupTime);

            Owner.Animator.speed = 0;

            Owner.AbilityComponent.Activate(ref magicShield, Owner.transform.position);

            yield return new WaitUntil(() => !shieldActive);

            Owner.Animator.speed = 1;
        }

        void HandleMagicShieldFinished(Unit u, Ability a) {
            if (u != Owner || a != magicShield) return;

            shieldActive = false;
        }
        
        public override UnitState HandleUpdate(InputValues input) {
            if (shieldActive) return null;
            return new RoarUnitState(Owner);
        }
    }
}