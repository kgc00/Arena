using Controls;
using Units;
using UnityEngine;

namespace State.ChargingAiStates
{
    public class StunUnitState : UnitState
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        public StunUnitState(Unit owner) : base(owner) { }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            return Owner.StatusComponent.IsStunned() ? null : new IdleUnitState(Owner);
        }
    }
}