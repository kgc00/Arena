using Data.Types;
using UnityEngine;

namespace Status {
    public class Marked : MonoStatus {
        public override StatusType Type { get; protected set; } = StatusType.Marked;
        protected override void EnableEffect() { }
    }
}