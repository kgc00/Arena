using System;
using Abilities.AttackAbilities;
using Controls;
using Pathfinding;
using Units;
using UnityEngine;
using static Utils.MathHelpers;
using Random = UnityEngine.Random;

namespace State.BossAiStates {
    public class RelocateUnitState : BossState {
        
        private static readonly int Moving = Animator.StringToHash("Moving");
        private Transform playerTransform;
        private readonly Unit targetUnit;
        private Vector3 destination;
        private Roar roar;
        private IAstarAI astarAI;

        public RelocateUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            roar = Owner.AbilityComponent.GetEquippedAbility<Roar>();
            DefineDestination();
        }
        
        public override void Enter()
        {
            astarAI = Owner.GetComponent<IAstarAI>() ?? throw new Exception("Unable to find AstarAI component in " + GetType());
            astarAI.maxSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
            astarAI.canMove = true; // isStopped would still control the rotation outside this state 
            astarAI.destination = destination;
            
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Moving);
        }

        public override void Exit()
        {
            astarAI.canMove = false;
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Moving);
        }
        
        private void DefineDestination() {
            var arenaBounds = 20f;
            var position = Owner.transform.position;
            var x = Clamp(position.x + Random.Range(-8, 8), -arenaBounds, arenaBounds);
            var y = position.y;
            var z = Clamp(position.z + Random.Range(-8, 8), -arenaBounds, arenaBounds);
            destination = new Vector3(x, y, z);
        }


        public override UnitState HandleUpdate(InputValues input) {
            UnitState nextState = null;
            bool invalidTarget = playerTransform == null ||
                                 !targetUnit.StatusComponent.IsVisible();
            if (invalidTarget) return new IdleUnitState(Owner);
            var distToDest = Vector3.Distance(destination, Owner.transform.position);
            if (distToDest > 1) return null;
            var distToPlayer = Vector3.Distance(playerTransform.position, Owner.transform.position);
            return distToPlayer > roar.Range
                ? (UnitState) new ChainFlameUnitState(Owner, playerTransform)
                : new RoarUnitState(Owner);
        }
    }
}