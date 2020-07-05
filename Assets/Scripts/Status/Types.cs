using System;

namespace Status
{
    [Flags] 
    public enum Types : short
    {
        Healthy = 0,
        Silenced = 1,
        Stunned = 2,
        Rooted = 4,
        Marked = 8,
        Hidden = 16
    };
}