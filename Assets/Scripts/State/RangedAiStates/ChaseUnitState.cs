using Controls;
using JetBrains.Annotations;
using Units;
using UnityEngine;
using IdleUnitState = State.RangedAiStates.IdleUnitState;

namespace State.RangedAiStates
{
    public class ChaseUnitState : UnitState
    {
        Transform targetPlayerTransform;
        private float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private float attackRange = 3.0f;
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            targetPlayerTransform = playerTransform;
            movementSpeed = owner.BaseStats.MovementSpeed.Value;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, true);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (targetPlayerTransform == null) return new IdleUnitState(Owner);
            
            UpdateUnitLocation();
            UpdateUnitRotation();
            
            if (ShouldEnterAttack(out var unitState)) return unitState;
            
            return null;
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

            
            Owner.Animator.SetBool(Attacking, true);
            Owner.Animator.SetBool(Moving, false);
            unitState = new AttackUnitState(Owner, targetPlayerTransform);
            return true;
        }

        private void UpdateUnitLocation()
        {
            Owner.transform.position = Vector3.MoveTowards(Owner.transform.position,
                targetPlayerTransform.position,
                movementSpeed * Time.deltaTime);
        }
    }
}