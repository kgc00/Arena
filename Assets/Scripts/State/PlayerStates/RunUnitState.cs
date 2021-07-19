using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Units;
using UnityEngine;
using Utils;

namespace State.PlayerStates
{
    public class RunUnitState : PlayerState
    {
        public RunUnitState(Unit owner) : base(owner) { }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (skillBehaviour.ShouldActivateSkill(input, out var unitState)) 
                return unitState;

            var playerIsStationary = Math.Abs(input.Forward) <= movementThreshold && 
                                     Math.Abs(input.Horizontal) <= movementThreshold;
            
            if (playerIsStationary) return new IdleUnitState(Owner);

            return null;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
        }
    }
}