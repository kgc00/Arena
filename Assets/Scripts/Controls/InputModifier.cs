using System;

namespace Controls {
    [Flags] 
    public enum InputModifier : short {
        None = 0,
        CannotMove = 1,
        CannotRotate = 2,
        CannotACt = 4,
    }
}