using System;
using Abilities.AttackAbilities;
using Controls;
using JetBrains.Annotations;
using Pathfinding;
using Units;
using UnityEngine;
using static Utils.MathHelpers;
using Random = UnityEngine.Random;

namespace State.BombThrowingAiStates
{
    public class RelocateUnitState : BombThrowingAiState
    {
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private Disrupt disrupt;
        private MissileStorm missileStorm;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private IAstarAI astarAI;
        private Vector3 destination;

        public RelocateUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            disrupt = Owner.AbilityComponent.GetEquippedAbility<Disrupt>();
            missileStorm = Owner.AbilityComponent.GetEquippedAbility<MissileStorm>();
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
            var arenaBounds = 22f;
            var position = Owner.transform.position;
            var x = Clamp(position.x + Random.Range(-arenaBounds, arenaBounds), -arenaBounds, arenaBounds);
            var y = position.y;
            var z = Clamp(position.z + Random.Range(-arenaBounds, arenaBounds), -arenaBounds, arenaBounds);
            destination = new Vector3(x, y, z);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;
            
            UnitState nextState = null;
            bool invalidTarget = playerTransform == null ||
                                 !targetUnit.StatusComponent.IsVisible();
            
            if (invalidTarget) return new IdleUnitState(Owner);
            
            var distToDest = Vector3.Distance(destination, Owner.transform.position);
            if (distToDest > 1) return null;

            var distToPlayer = Vector3.Distance(playerTransform.position, Owner.transform.position);
            if (ShouldEnterDisrupt(ref nextState, distToPlayer)) return nextState;
            return nextState;
        }

        private bool ShouldEnterDisrupt(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > disrupt.Range;
            var abilityCoolingDown = disrupt.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new DisruptState(Owner, playerTransform);
            return true;
        }
    }
}