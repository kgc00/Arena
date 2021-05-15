using System;

namespace Data.Types {
    [Flags]
    public enum IndicatorType {
        None = 1,
        Arrow = 2,
        Circle = 4,
        Rectangle = 8,
    }
}