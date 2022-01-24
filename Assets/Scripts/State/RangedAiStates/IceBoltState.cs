using Controls;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using System.Collections;
using Abilities;
using Abilities.AttackAbilities;

namespace State.RangedAiStates {
    public class IceBoltState : AbilityUnitState<IceBolt> {
        private readonly Transform _playerTransform;
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public IceBoltState(Unit owner, Transform targetTransform) : base(owner) {
            _playerTransform = targetTransform;
        }

        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator || _playerTransform == null) {
                AbilityFinished = true;
                yield break;
            }

            Owner.Animator.SetTrigger(Attacking);
            yield return Owner.AbilityComponent.Activate(ref Ability, _playerTransform.position);
        }

        protected override void HandleAbilityFinished(Unit u, Ability a) {
            Owner.CoroutineHelper.StartCoroutine(ExecuteWaitForCooldown());
        }

        IEnumerator ExecuteWaitForCooldown() {
            yield return new WaitUntil(() => !Ability.Cooldown.IsOnCooldown);
            base.HandleAbilityFinished(Owner, Ability);
        }

        public override UnitState HandleUpdate(InputValues input) {
            UnitState state = null;
            var distanceToUnit = Vector3.Distance(Owner.transform.position, _playerTransform.position);
            var isWithinAttackRange = distanceToUnit <= Ability.Range;
            
            if (ShouldEnterAttack(ref state, isWithinAttackRange)) {
                return state;
            }

            if (ShouldEnterIdle(ref state)) {
                return state;
            }

            if (ShouldEnterChase(ref state, isWithinAttackRange)) {
                return state;
            }

            return null;
        }


        private bool ShouldEnterAttack([CanBeNull] ref UnitState unitState, bool isWithinAttackRange) {
            if (isWithinAttackRange && AbilityFinished) {
                unitState = new IceBoltState(Owner, _playerTransform);
                return true;
            }

            return false;
        }

        private bool ShouldEnterIdle([CanBeNull] ref UnitState unitState) {
            var playerIsHidden = false;
            if (_playerTransform.TryGetComponent(out Unit unit)) {
                playerIsHidden = !unit.StatusComponent.IsVisible();
            }

            if (_playerTransform != null && !playerIsHidden) return false;
            unitState = new IdleUnitState(Owner);
            return true;
        }

        private bool ShouldEnterChase([CanBeNull] ref UnitState unitState, bool isWithinAttackRange) {
            if (isWithinAttackRange || !AbilityFinished) return false;
            unitState = new ChaseUnitState(Owner, _playerTransform);
            return true;
        }

        public override void HandleFixedUpdate(InputValues input) {
            if (_playerTransform == null || Owner.transform == null) return;
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation() {
            var transform = Owner.transform;
            var heading = (_playerTransform.position - transform.position).normalized;
            Owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(heading),
                Time.deltaTime * 20f);
        }
    }
}