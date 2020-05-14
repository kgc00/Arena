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
    public class IdleUnitState : PlayerState
    {
        public IdleUnitState(Unit owner, bool rotationDisabled) : base(owner, rotationDisabled) { }
        public override UnitState HandleUpdate(InputValues input)
        {
            if (skillBehaviour.ShouldActivateSkill(input, rotationDisabled, out var unitState)) 
                return unitState;
            
            base.HandleUpdate(input);
            
            bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;
            if (playerIsMoving) return new RunUnitState(Owner, rotationDisabled);
            
            return null;
        }
    }
}