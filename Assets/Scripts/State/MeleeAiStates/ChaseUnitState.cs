using Controls;
using JetBrains.Annotations;
using Stats;
using Units;
using UnityEngine;

namespace State.MeleeAiStates
{
    public class ChaseUnitState : UnitState
    {
        readonly Transform targetPlayerTransform;
        private readonly Unit targetUnit;
        private readonly float movementSpeed = 2f;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private readonly float attackRange;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            targetPlayerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Moving);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Moving);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            bool invalidTarget = targetPlayerTransform == null ||
                                 targetUnit.StatusComponent.Status.HasFlag(Status.Hidden);
            
            if (invalidTarget) return new IdleUnitState(Owner);
            
            if (ShouldEnterAttack(out var unitState)) return unitState;
            
            return null;
        }

        public override void HandleFixedUpdate(InputValues input)
        {
            bool invalidTarget = targetPlayerTransform == null ||
                                 targetUnit.StatusComponent.Status.HasFlag(Status.Hidden);

            if (invalidTarget) return;
                
            UpdateUnitLocation();
            UpdateUnitRotation();
        }

        private void UpdateUnitRotation()
        {
            var difference = targetPlayerTransform.position - Owner.transform.position;
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }

        private bool ShouldEnterAttack([CanBeNull] out UnitState unitState)
        {
            unitState = null;

            var distanceToUnit = Vector3.Distance(Owner.transform.position, targetPlayerTransform.position);
            if (distanceToUnit > attackRange) return false;
            
            unitState = new AttackUnitState(Owner, targetPlayerTransform);
            return true;
        }

        private void UpdateUnitLocation()
        {
            var moveDirection = targetPlayerTransform.position - Owner.transform.position;
            Owner.Rigidbody.AddForce( moveDirection.normalized * 50f);
            // Owner.transform.position = Vector3.MoveTowards(Owner.transform.position,
            //                                                 targetPlayerTransform.position,
            //                                                 movementSpeed * Time.deltaTime);
        }
    }
}