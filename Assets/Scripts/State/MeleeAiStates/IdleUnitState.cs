using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.MeleeAiStates
{
    public class IdleUnitState : UnitState
    {
        private readonly float movementSpeed;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private Transform playerTransform;

        public IdleUnitState(Unit owner) : base(owner)
        {
            movementSpeed = 2f;
            // playerTransform = Locator.GetClosestPlayerUnit(Owner.transform.position);
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, false);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            // TODO: add some leashing mechanic or vision limiter
            
            if (playerTransform == null) playerTransform = Locator.GetClosestPlayerUnit(Owner.transform.position);

            if (playerTransform == null) return null;
            
            return new ChaseUnitState(Owner, playerTransform);
        }
    }
}