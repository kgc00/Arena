﻿using System;
using Abilities.AttackAbilities;
using Controls;
using JetBrains.Annotations;
using Pathfinding;
using Units;
using UnityEngine;

namespace State.ChargingAiStates
{
    public class ChaseUnitState : ChargeAiState
    {
        readonly Transform playerTransform;
        private readonly Unit targetUnit;
        private Charge charge;
        private OrcSlash slash;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private IAstarAI astarAI;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            targetUnit = playerTransform.GetComponentInChildren<Unit>();
            charge = Owner.AbilityComponent.GetEquippedAbility<Charge>();
            slash = Owner.AbilityComponent.GetEquippedAbility<OrcSlash>();
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
            
            UnitState nextState = null;
            bool invalidTarget = playerTransform == null ||
                                 !targetUnit.StatusComponent.IsVisible();
            
            if (invalidTarget) return new IdleUnitState(Owner);
            
            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);
            if (ShouldEnterAttack(ref nextState, dist)) return nextState;
            if (ShouldEnterCharge(ref nextState, dist)) return nextState;

            astarAI.destination = targetUnit.transform.position;
            return nextState;
        }

        private bool ShouldEnterAttack(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > slash.Range;
            var abilityCoolingDown = slash.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new OrcSlashState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterCharge(ref UnitState unitState, float dist) {
            var abilityWillNotReach = dist > charge.Range;
            var abilityCoolingDown = charge.Cooldown.IsOnCooldown;
            if (abilityWillNotReach || abilityCoolingDown) return false;

            unitState = new ChargeUnitState(Owner, playerTransform);
            return true;
        }
    }
}