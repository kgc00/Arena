using Controls;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using System.Collections;
using Abilities;
using Abilities.AttackAbilities;
using Random = UnityEngine.Random;

namespace State.BombThrowingAiStates {
    public class DisruptState : AbilityUnitState<Disrupt> {
        private readonly Transform _playerTransform;
        private static readonly int Disrupt = Animator.StringToHash("Disrupt");
        private readonly Unit _targetUnit;
        private const float DisruptRecastRate = 75;

        public DisruptState(Unit owner, Transform targetTransform) : base(owner) {
            _playerTransform = targetTransform;
            _playerTransform.TryGetComponent(out Unit unit);
            _targetUnit = unit;
        }

        protected override IEnumerator HandleAbility() {
            if (Owner.Animator == null || !Owner.Animator || _playerTransform == null) {
                AbilityFinished = true;
                yield break;
            }

            Owner.Animator.SetTrigger(Disrupt);
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

            if (ShouldEnterIdle(ref state, invalidTarget)) {
                return state;
            }
            
            var distanceToUnit = Vector3.Distance(Owner.transform.position, _playerTransform.position);
            var isWithinAttackRange = distanceToUnit <= Ability.Range;
            
            // if there is a max distance for the ability, the unit could just sit here until the play walks into it
            
            if (!AbilityFinished){
                return null;
            }

            if (!isWithinAttackRange){
                return new RelocateUnitState(Owner, _playerTransform);
            }

            if (Random.Range(0,100f) >= DisruptRecastRate){
                return new DisruptState(Owner, _playerTransform);
            }

            return new RelocateUnitState(Owner, _playerTransform);
        }


        private bool ShouldEnterIdle([CanBeNull] ref UnitState unitState, bool invalidTarget) {
            if (!invalidTarget) return false;
            unitState = new IdleUnitState(Owner);
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