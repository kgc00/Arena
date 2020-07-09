using System;

namespace Data.Types
{
    [Flags] 
    public enum StatusType : short
    {
        Healthy = 0,
        Silenced = 1,
        Stunned = 2,
        Rooted = 4,
        Marked = 8,
        Hidden = 16
    };
}