﻿using System.Collections;
using System.Linq;
using Abilities.AttackAbilities;
using Components;
using Controls;
using Data.Types;
using JetBrains.Annotations;
using Units;
using UnityEngine;

namespace State.ChargingAiStates
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

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleAttack());

        private IEnumerator HandleAttack()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) yield break;
            Owner.Animator.SetTrigger(Attacking);
            yield return new WaitForSeconds(0.45f);
            if (playerTransform == null) yield break;
            
            Owner.AbilityComponent.Activate(Owner.AbilityComponent.longestRangeAbility, 
                                            playerTransform.position);
            
            Debug.Log("Finishing attack execution");
            Owner.Animator.ResetTrigger(Attacking);
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
            
            if (ShouldEnterIdle(out var unitState)) return unitState;
            if (ShouldEnterChase(out unitState)) return unitState;
            if (ShouldAttack(out unitState)) return unitState;
            
            return unitState;
        }

        public override void HandleFixedUpdate(InputValues input) {
            UpdateUnitRotation();
        }

        private bool ShouldAttack(out UnitState unitState)
        {
            unitState = null;
            if (!attackComplete || Owner.AbilityComponent.longestRangeAbility.Cooldown.IsOnCooldown) return false;

            unitState = new AttackUnitState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterIdle([CanBeNull] out UnitState unitState)
        {
            unitState = null;
            
            if (playerTransform.GetComponentInChildren<Unit>().StatusComponent.IsVisible()) return false;
            
            unitState = new IdleUnitState(Owner);
            return true;
        }

        private bool ShouldEnterChase([CanBeNull] out UnitState unitState)
        {
            unitState = null;

            var distanceToUnit = Vector3.Distance(Owner.transform.position, playerTransform.position);
            if (distanceToUnit <= attackRange + padding) return false;
            
            unitState = new ChaseUnitState(Owner, playerTransform);
            return true;
        }

        private void UpdateUnitRotation()
        {
            var difference = playerTransform.position - Owner.transform.position;
            Owner.Rigidbody.MoveRotation(Quaternion.LookRotation(difference));
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