using System;
using Abilities.AttackAbilities;
using Components;
using Controls;
using Pathfinding;
using Units;
using UnityEngine;
using static Utils.MathHelpers;
using Random = UnityEngine.Random;

namespace State.MeleeAiStates {
    public class RelocateUnitState : MeleeAiState {
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private BodySlam bodySlam;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private IAstarAI astarAI;
        private Vector3 destination;
        private bool enterChaseOverride;

        public RelocateUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            bodySlam = Owner.AbilityComponent.GetEquippedAbility<BodySlam>();
            DefineDestination();
            HealthComponent.OnDamageStarted += DrawAggro;
        }

        ~RelocateUnitState() {
            HealthComponent.OnDamageStarted -= DrawAggro;
        }
        
        private void DrawAggro(Unit owner, Unit damageDealer, float damage) {
            if (owner != Owner) return;

            enterChaseOverride = true;
        }

        public override void Enter() {
            astarAI = Owner.GetComponent<IAstarAI>() ??
                      throw new Exception("Unable to find AstarAI component in " + GetType());
            astarAI.maxSpeed = Owner.StatsComponent.Stats.MovementSpeed.Value;
            astarAI.isStopped = false; // isStopped would still control the rotation outside this state 
            astarAI.destination = destination;

            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Moving);
        }

        public override void Exit() {
            astarAI.isStopped = true; 
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

        public override UnitState HandleUpdate(InputValues input) {
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;

            UnitState nextState = null;
            bool invalidTarget = playerTransform == null ||
                                 !targetUnit.StatusComponent.IsVisible();

            if (invalidTarget) return new IdleUnitState(Owner);
            var distToPlayer = Vector3.Distance(playerTransform.position, Owner.transform.position);
            if (distToPlayer < 4.5f || enterChaseOverride) {
                nextState = new ChaseUnitState(Owner, playerTransform);
                return nextState;
            }
            
            var distToDest = Vector3.Distance(destination, Owner.transform.position);
            if (distToDest > 1) return null;

            if (ShouldEnterAttack(ref nextState, distToPlayer)) return nextState;
            nextState = new ChaseUnitState(Owner, playerTransform);
            return nextState;
        }

        private bool ShouldEnterAttack(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > bodySlam.Range;
            var abilityCoolingDown = bodySlam.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new BodySlamState(Owner, playerTransform);
            return true;
        }
    }
}