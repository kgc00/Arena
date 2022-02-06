using System;
using Controls;
using Data.Types;

namespace Status {
    public class Stunned : MonoStatus {
        public override StatusType Type { get; protected set; } = StatusType.Stunned;

        protected override void EnableEffect() {
            Owner.InputModifierComponent
                .AddModifier(InputModifier.CannotAct)
                .AddModifier(InputModifier.CannotMove)
                .AddModifier(InputModifier.CannotRotate);
        }

        protected override void DisableEffect() {
            Owner.InputModifierComponent
                .RemoveModifier(InputModifier.CannotAct)
                .RemoveModifier(InputModifier.CannotMove)
                .RemoveModifier(InputModifier.CannotRotate);
            base.DisableEffect();
        }
    }
}