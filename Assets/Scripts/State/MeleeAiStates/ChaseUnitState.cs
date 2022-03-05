using System;
using Common;
using Controls;
using JetBrains.Annotations;
using Pathfinding;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace State.MeleeAiStates
{
    public class ChaseUnitState : MeleeAiState
    {
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private readonly float movementSpeed = 2f;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private readonly float attackRange;
        private IAstarAI astarAI;
        private float chaseTimer;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
            chaseTimer = Random.Range(0,1f) >= Constants.PermaChaseRate ? Random.Range(3, 8f) : float.MaxValue;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Moving);
            astarAI = Owner.GetComponent<IAstarAI>() ?? throw new Exception("Unable to find AstarAI component in " + GetType());
            astarAI.maxSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
            astarAI.isStopped = false;
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Moving);
            astarAI.isStopped = true;
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;
            
            bool invalidTarget = playerTransform == null ||
                                 !targetUnit.StatusComponent.IsVisible();
            
            if (invalidTarget) return new IdleUnitState(Owner);
            
            if (ShouldEnterAttack(out var unitState)) return unitState;
            if (ShouldEnterRelocate(ref unitState)) return unitState;

            astarAI.destination = targetUnit.transform.position;
            return null;
        }

        private bool ShouldEnterRelocate(ref UnitState unitState) {
            var distToPlayer = Vector3.Distance(playerTransform.position, Owner.transform.position);
            chaseTimer -= Time.deltaTime;
            if (chaseTimer > 0) return false;
            if (distToPlayer <= 4.5f) return false;
            unitState = new RelocateUnitState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterAttack([CanBeNull] out UnitState unitState)
        {
            unitState = null;

            var distanceToUnit = Vector3.Distance(Owner.transform.position, playerTransform.position);
            if (distanceToUnit > attackRange) return false;
            
            unitState = new BodySlamState(Owner, playerTransform);
            return true;
        }
    }
}