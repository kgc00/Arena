using Controls;
using JetBrains.Annotations;
using Stats;
using Units;
using UnityEngine;

namespace State.RangedAiStates
{
    public class ChaseUnitState : UnitState
    {
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private readonly float attackRange;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            movementSpeed = 4;
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Moving);

            // Debug.Log("entering chase");
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Moving);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            bool invalidTarget = playerTransform == null ||
                                 targetUnit.StatusComponent.Status.HasFlag(Status.Hidden);
            
            if (invalidTarget) return new IdleUnitState(Owner);
            
            if (ShouldEnterAttack(out var unitState)) return unitState;
            
            return null;
        }

        public override void HandleFixedUpdate(InputValues input)
        {
            bool invalidTarget = playerTransform == null ||
                              targetUnit.StatusComponent.Status.HasFlag(Status.Hidden);

            if (invalidTarget) return;
            
            UpdateUnitLocation();
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation()
        {
            var difference = playerTransform.position - Owner.transform.position;
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }

        private bool ShouldEnterAttack([CanBeNull] out UnitState unitState)
        {
            unitState = null;

            var distanceToUnit = Vector3.Distance(Owner.transform.position, playerTransform.position);
            if (distanceToUnit > attackRange) return false;
            
            unitState = new AttackUnitState(Owner, playerTransform);
            return true;
        }

        private void UpdateUnitLocation()
        {
            var moveDirection = playerTransform.position - Owner.transform.position;
            Owner.Rigidbody.AddForce( moveDirection.normalized * 50f);
        }
    }
}