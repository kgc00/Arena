using Data.Types;
using Units;
using UnityEngine;

namespace Status
{
    public class StatusComponent : MonoBehaviour
    { 
        public Unit Owner { get; private set; }
        public StatusType StatusType { get; private set; } = (StatusType) 0;
        public StatusComponent Initialize (Unit owner) {
            Owner = owner;
            return this;
        }
        
        public void AddStatus(StatusType statusType) => StatusType |= statusType;
        public void RemoveStatus(StatusType statusType) => StatusType &= ~statusType;
        public bool IsVisible() => !StatusType.HasFlag(StatusType.Hidden);
    }
 
}