using Controls;
using Enums;
using JetBrains.Annotations;
using Stats;
using Units;
using UnityEngine;
using System.Collections;

namespace State.MeleeAiStates
{
    public class AttackUnitState : UnitState
    {
        private readonly Transform playerTransform;
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private readonly float attackRange;
        private float padding = 1.0f;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private bool attackComplete;

        public AttackUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
            attackComplete = false;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Attacking);  
        }

        private IEnumerator HandleAttack()
        {
            
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) yield break;
            Owner.Animator.SetTrigger(Idle);
            
            while (Owner.AbilityComponent.longestRangeAbility.Cooldown.IsOnCooldown)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            Owner.Animator.ResetTrigger(Idle);
            Owner.Animator.SetTrigger(Attacking);
            yield return new WaitForSeconds(0.45f);
            
            Owner.AbilityComponent.Activate(Owner.AbilityComponent.longestRangeAbility, 
                                            playerTransform.position);
            
            Debug.Log("Finishing attack execution");
            attackComplete = true;
        } 
        
        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Attacking);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (playerTransform == null) return new IdleUnitState(Owner);
            
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

            unitState = new AttackUnitState(Owner, playerTransform);
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

            var distanceToUnit = Vector3.Distance(Owner.transform.position, playerTransform.position);
            if (distanceToUnit <= attackRange + padding) return false;
            
            Owner.Animator.SetTrigger(Attacking);
            unitState = new ChaseUnitState(Owner, playerTransform);
            return true;
        }
        
        private void UpdateUnitRotation()
        {
            var difference = playerTransform.position - Owner.transform.position;
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
            other.gameObject.GetComponent<HealthComponent>().DamageOwner(-1f, null, Owner);
            Debug.Log("hit player");
        }
    }
}