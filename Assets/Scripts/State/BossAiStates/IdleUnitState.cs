using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.BossAiStates
{
    public class IdleUnitState : BossState
    {
        private Transform playerTransform;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private Roar roar;
        private float _randomSeed;

        public IdleUnitState(Unit owner) : base(owner) {
            playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
            roar = Owner.AbilityComponent.GetEquippedAbility<Roar>();
            _randomSeed = Random.Range(0,1);
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            var isStunned = base.HandleUpdate(input);
            if (isStunned != null) return isStunned;
            UnitState nextState = null;
            if (playerTransform == null) {
                playerTransform = Locator.GetClosestVisiblePlayerUnit(Owner.transform.position);
            }

            bool invalidTarget = playerTransform == null;
            
            if (invalidTarget) return nextState;
            
            var dist = Vector3.Distance(playerTransform.position, Owner.transform.position);
            if (ShouldEnterRoar(ref nextState, dist)) return nextState;
            if (ShouldEnterRelocateState(ref nextState, dist)) return nextState;
            // if (ShouldEnterShield(ref nextState, dist)) return nextState;
            return null;
        }

        private bool ShouldEnterRoar(ref UnitState unitState, float dist) {
            if (dist > roar.Range) return false;
            unitState = new RoarUnitState(Owner, playerTransform);
            return true;
        }
        
        private bool ShouldEnterShield(ref UnitState unitState, float dist) {
            if (dist >= roar.Range) return false;
            
            unitState = new MagicShieldUnitState(Owner, playerTransform);
            return true;
        }

        private bool ShouldEnterRelocateState(ref UnitState unitState, float dist) {
            unitState = new RelocateUnitState(Owner, playerTransform);
            return true;
        }
    }
}