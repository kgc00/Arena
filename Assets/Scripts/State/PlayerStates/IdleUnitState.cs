using System;
using Controls;
using  UnityEngine;
public class IdleUnitState : UnitState {
    public IdleUnitState (Unit Owner) : base (Owner) {
    }

    public override void Enter () {
    }

    public override UnitState HandleInput (InputValues input) {
        Owner.Animator.SetBool("Moving", false);
        if (Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0)
        {
            return new RunUnitState(Owner);
        }
        return null;
    }
}