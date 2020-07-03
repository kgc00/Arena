using System.Linq;
using Abilities.AttackAbilities;
using Controls;
using JetBrains.Annotations;
using Stats;
using Units;
using UnityEngine;
using Utils;

namespace State.ChargingAiStates
{
    public class ChaseUnitState : UnitState
    {
        readonly Transform playerTransform;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private readonly float attackRange;

        public ChaseUnitState(Unit owner, Transform playerTransform) : base(owner)
        {
            this.playerTransform = playerTransform;
            attackRange = Owner.AbilityComponent.longestRangeAbility.Range;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Moving);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Moving);
        }

        public override UnitState HandleUpdate(InputValues input) {
            // TODO: add some leashing mechanic or vision limiter

            if (playerTransform == null) return new IdleUnitState(Owner);

            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);

            if (ShouldEnterCharge(out var unitState, dist)) return unitState;
            if (ShouldEnterAttack(out unitState, dist)) return unitState;

            return unitState;
        }

        public override void HandleFixedUpdate(InputValues input) {
            UpdateUnitLocation();
        }

        private bool ShouldEnterAttack(out UnitState unitState, float dist) {
            unitState = null;

            // are we close enough away to use attack and is an attack ready?
            if (dist > attackRange && !Owner.AbilityComponent.longestRangeAbility.Cooldown.IsOnCooldown) 
                return false;

            unitState = new AttackUnitState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterCharge([CanBeNull] out UnitState unitState, float dist) {
            unitState = null;

            // find charge ability
            var charge = Owner.AbilityComponent.equippedAbilities.Values.FirstOrDefault(a => a is Charge);
            if (charge == null) return false;

            // is charge ready to use?
            if (charge.Cooldown.IsOnCooldown) return false;

            // are we too close to use charge?
            if (dist <= attackRange) return false;

            unitState = new ChargeUnitState(Owner, playerTransform);
            return true;
        }

        private void UpdateUnitLocation()
        {
            var moveDirection = playerTransform.position - Owner.transform.position;
            Owner.Rigidbody.AddForce( moveDirection.normalized * Owner.StatsComponent.Stats.MovementSpeed.Value);
        }
    }
}