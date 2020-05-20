using System.Collections;
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
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private readonly float attackRange;
        private float padding = 1.0f;
        private Coroutine routine;
        private bool attackComplete;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public AttackUnitState(Unit owner, Transform targetPlayerTransform) : base(owner)
        {
            this.targetPlayerTransform = targetPlayerTransform;
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
            attackComplete = false;
        }

        public override void Enter() => routine = Owner.CoroutineHelper.SpawnCoroutine(HandleAttack());

        private IEnumerator HandleAttack()
        {
            
            if (Owner.Animator == null || !Owner.Animator) yield break;
            Owner.Animator.SetTrigger(Idle);
            
            while (Owner.AbilityComponent.longestRangeAbility.Cooldown.IsOnCooldown)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            Owner.Animator.ResetTrigger(Idle);
            Owner.Animator.SetTrigger(Attacking);
            
            yield return new WaitForSeconds(0.45f);
            Owner.AbilityComponent.longestRangeAbility.AbilityActivated(targetPlayerTransform.position);
            Debug.Log("Finishing attack execution");
            attackComplete = true;
        } 
        
        public override void Exit()
        {
            if (routine != null) Owner.CoroutineHelper.Stop(routine);
            
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Attacking);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (targetPlayerTransform == null) return new IdleUnitState(Owner);

            if (ShouldReturnToIdle(out var unitState)) return unitState;
            if (ShouldReturnToChase(out unitState)) return unitState;

            UpdateUnitRotation();
            if (ShouldAttack(out unitState)) return unitState;
            return unitState;
        }

        private bool ShouldAttack(out UnitState unitState)
        {
            unitState = null;
            if (!attackComplete || Owner.AbilityComponent.longestRangeAbility.Cooldown.IsOnCooldown) return false;

            unitState = new AttackUnitState(Owner, targetPlayerTransform);
            return true;
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
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(-1f);
            Debug.Log("hit player");
        }
    }
}