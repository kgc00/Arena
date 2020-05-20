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
        public IdleUnitState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input)
        {
            if (skillBehaviour.ShouldActivateSkill(input, out var unitState)) 
                return unitState;

            bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;
            if (playerIsMoving) return new RunUnitState(Owner);
            
            return base.HandleUpdate(input);
        }

        public override void Enter() {
            Debug.Log($"Entered Idle state, {Owner.inputModifierComponent.InputModifier}");
        }

        public override void Exit() {
            Debug.Log("Exited Idle state");
        }
    }
}