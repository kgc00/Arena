using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using Utils;

namespace State.PlayerStates
{
    public class RunUnitState : PlayerState
    {
        private static readonly int Moving = Animator.StringToHash("Moving");

        public RunUnitState(Unit owner, bool rotationDisabled) : base(owner, rotationDisabled) { }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (skillBehaviour.ShouldActivateSkill(input, rotationDisabled,out var unitState)) 
                return unitState;

            base.HandleUpdate(input);
            
            var playerIsStationary = Math.Abs(input.Forward) <= movementThreshold && 
                                           Math.Abs(input.Horizontal) <= movementThreshold;
            
            if (playerIsStationary) return new IdleUnitState(Owner, rotationDisabled);
        
            return null;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, true);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetBool(Moving, false);
        }
    }
}