using System;
using Controls;
using Pathfinding;
using Units;
using UnityEngine;

namespace State.RangedAiStates {
    public class ChaseUnitState : UnitState {
        private const int aqcuisitionRangeMargin = 2;
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private readonly float attackRange;
        private IAstarAI astarAI;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range - aqcuisitionRangeMargin;
        }

        public override void Enter() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Moving);

            astarAI = Owner.GetComponent<IAstarAI>() ?? throw new Exception("Unable to find AstarAI component in " + GetType());
            astarAI.maxSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
            astarAI.isStopped = false;
        }

        public override void Exit() {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Moving);
            astarAI.isStopped = true;
        }

        public override UnitState HandleUpdate(InputValues input) {
            UnitState nextState = null;
            var invalidTarget = playerTransform == null ||
                                !targetUnit.StatusComponent.IsVisible();
            
            
            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);
            if (ShouldEnterIdle(ref nextState, invalidTarget)) return nextState;
            if (ShouldEnterAttack(ref nextState, dist)) return nextState;

            astarAI.destination = targetUnit.transform.position;
            return nextState;
        }

        private bool ShouldEnterIdle(ref UnitState nextState, bool invalidTarget) {
            if (!invalidTarget) return false;

            nextState = new IdleUnitState(Owner);
            return true;
        }

        private bool ShouldEnterAttack(ref UnitState unitState, float dist) {
            if (dist > attackRange) return false;

            unitState = new IceBoltState(Owner, playerTransform);
            return true;
        }

    }
}