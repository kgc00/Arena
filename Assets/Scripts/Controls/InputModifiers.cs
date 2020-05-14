using System;

namespace Controls {
    [Flags] 
    public enum InputModifiers : short {
        None = 0,
        CannotMove = 1,
        CannotRotate = 2,
        CannotACt = 4,
    }
}