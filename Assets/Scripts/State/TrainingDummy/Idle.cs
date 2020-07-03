using Controls;
using Units;
using UnityEngine;

namespace State.TrainingDummy
{
    public class Idle : UnitState
    {
        private readonly float movementSpeed = 2f;
        private Transform playerTransform;
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");

        public Idle(Unit owner) : base(owner) { }

        public override void Enter()
        {
            Owner.HealthComponent.Invulnerable = true;
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.SetTrigger(IdleAnimation);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator || playerTransform == null) return;
            Owner.Animator.ResetTrigger(IdleAnimation);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            return null;
        }
    }
}