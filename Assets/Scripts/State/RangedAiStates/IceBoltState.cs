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
        private Unit _targetUnit;

        public IceBoltState(Unit owner, Transform targetTransform) : base(owner) {
            _playerTransform = targetTransform;
            _playerTransform.TryGetComponent(out Unit unit);
            _targetUnit = unit;
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
            var isNotVisible = _targetUnit == null || !_targetUnit.StatusComponent.IsVisible();
            var invalidTarget = _playerTransform == null ||
                                isNotVisible;
            var distanceToUnit = Vector3.Distance(Owner.transform.position, _playerTransform.position);
            var isWithinAttackRange = distanceToUnit <= Ability.Range;
            
            if (ShouldEnterIdle(ref state, invalidTarget)) {
                return state;
            }
            
            if (ShouldEnterAttack(ref state, isWithinAttackRange)) {
                return state;
            }


            if (ShouldEnterChase(ref state, isWithinAttackRange)) {
                return state;
            }

            return null;
        }


        private bool ShouldEnterIdle([CanBeNull] ref UnitState unitState, bool invalidTarget) {
            if (!invalidTarget) return false;
            unitState = new IdleUnitState(Owner);
            return true;
        }
        
        private bool ShouldEnterAttack([CanBeNull] ref UnitState unitState, bool isWithinAttackRange) {
            if (isWithinAttackRange && AbilityFinished) {
                unitState = new IceBoltState(Owner, _playerTransform);
                return true;
            }

            return false;
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