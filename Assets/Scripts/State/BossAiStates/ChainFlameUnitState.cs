using System.Collections;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BossAiStates {
    public class ChainFlameUnitState : AbilityUnitState<ChainFlame> {
        private readonly Transform playerTransform;
        private static readonly int ChainFlame = Animator.StringToHash("ChainFlame");
        private Unit _targetUnit;

        public ChainFlameUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            if (playerTransform != null) {
                _targetUnit = playerTransform.gameObject.GetUnitComponent();
            }
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(ChainFlame);
        }

        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) {
                AbilityFinished = true;
                yield break;
            }

            Owner.Animator.SetTrigger(ChainFlame);
            Owner.AbilityComponent.Activate(ref Ability, playerTransform.position);
        }

        public override UnitState HandleUpdate(InputValues input) {
            if (AbilityFinished != true) return null;
            return new IdleUnitState(Owner);
        }

        public override void HandleFixedUpdate(InputValues input) {
            if (playerTransform == null) return;
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation() {
            if (_targetUnit != null && !_targetUnit.StatusComponent.IsVisible()) {
                return;
            }

            Quaternion wantedRotation =
                Quaternion.LookRotation(playerTransform.position - Owner.transform.position, Vector3.up);
            float rotationDamping = 5f;
            Owner.transform.rotation =
                Quaternion.Slerp(Owner.transform.rotation, wantedRotation, Time.fixedDeltaTime * rotationDamping);
        }
    }
}