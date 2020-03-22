using Controls;
using Enums;
using JetBrains.Annotations;
using Stats;
using Units;
using UnityEngine;

namespace State.RangedAiStates
{
    public class AttackUnitState : UnitState
    {
        private readonly Transform targetPlayerTransform;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private float attackRange = 3.0f;
        private float padding = 2.0f;

        public AttackUnitState(Unit owner, Transform targetPlayerTransform) : base(owner)
        {
            this.targetPlayerTransform = targetPlayerTransform;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Attacking, true);
            Owner.Animator.SetBool(Moving, false);
        }

        public override void Exit()
        {
            Owner.Animator.SetBool(Attacking, false);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (targetPlayerTransform == null) return new IdleUnitState(Owner);
            
            if (ShouldReturnToIdle(out var unitState)) return unitState;
            if (ShouldReturnToChase(out unitState)) return unitState;

            UpdateUnitRotation();
            return unitState;
        }

        private bool ShouldReturnToIdle([CanBeNull] out UnitState unitState)
        {
            unitState = null;
            return false;
        }

        private bool ShouldReturnToChase([CanBeNull] out UnitState unitState)
        {
            unitState = null;

            var distanceToUnit = Vector3.Distance(Owner.transform.position, targetPlayerTransform.position);
            if (distanceToUnit <= attackRange + padding) return false;
            
            Owner.Animator.SetBool(Attacking, false);
            unitState = new ChaseUnitState(Owner, targetPlayerTransform);
            return true;
        }
        
        private void UpdateUnitRotation()
        {
            var difference = targetPlayerTransform.position - Owner.transform.position;
            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation,
                Quaternion.LookRotation(difference),
                Time.deltaTime * 10f);
        }

        public override void HandleCollisionEnter(Collision other)
        {
            // is the object a unit?
            var objectAsUnit = other.transform.gameObject.GetComponent<Unit>();
            if (objectAsUnit == null) return;

            // is the object a player controlled unit?
            if (objectAsUnit.Owner.ControlType == ControlType.Ai) return;

           DamageUnit(other);
        }

        private void DamageUnit(Collision other)
        {
            // Apply damage
            other.gameObject.GetComponent<HealthComponent>().AdjustHealth(-Owner.BaseStats.Attack.Value);
            Debug.Log("hit player");
        }
    }
}