using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace State.BossAiStates {
    public class RelocateUnitState : BossState {
        
        private static readonly int Moving = Animator.StringToHash("Moving");
        private Transform playerTransform;
        private Vector3 destination;
        private Roar roar;

        public RelocateUnitState(Unit owner, Transform playerTransform) : base(owner) {
            this.playerTransform = playerTransform;
            roar = Owner.AbilityComponent.GetEquippedAbility<Roar>();
            DefineDestination();
        }
        
        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Moving);
        }

        private void DefineDestination() {
            // hardcoded.  should replace with a pathfinding system at some point
            var arenaBounds = 20f;
            var position = Owner.transform.position;
            var x = Clamp(position.x + Random.Range(-8, 8), -arenaBounds, arenaBounds);
            var y = position.y;
            var z = Clamp(position.z + Random.Range(-8, 8), -arenaBounds, arenaBounds);
            destination = new Vector3(x, y, z);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Moving);
        }

        public override UnitState HandleUpdate(InputValues input) {
            var distToDest = Vector3.Distance(destination, Owner.transform.position);
            if (distToDest > 1) return null;

            if (playerTransform == null) return new IdleUnitState(Owner);
            
            var distToPlayer = Vector3.Distance(playerTransform.position, Owner.transform.position);

            return distToPlayer > roar.Range
                ? (UnitState) new ChainFlameUnitState(Owner, playerTransform)
                : new RoarUnitState(Owner);
        }

        public override void HandleFixedUpdate(InputValues input) {
            if (playerTransform == null) return;

            UpdateUnitLocation();
            UpdateUnitRotation();
        }
        
        
        private void UpdateUnitLocation()
        {
            var heading = destination - Owner.transform.position;
            Owner.Rigidbody.AddForce( heading.normalized * Owner.StatsComponent.Stats.MovementSpeed.Value);
        }
        private void UpdateUnitRotation()
        {
            var difference = destination - Owner.transform.position;
            Owner.Rigidbody.MoveRotation(Quaternion.LookRotation(difference));
        }
    }
}