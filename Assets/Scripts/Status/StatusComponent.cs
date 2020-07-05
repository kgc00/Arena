using Units;
using UnityEngine;

namespace Status
{
    public class StatusComponent : MonoBehaviour
    { 
        public Unit Owner { get; private set; }
        public Types Types { get; private set; } = (Types) 0;
        public StatusComponent Initialize (Unit owner) {
            Owner = owner;
            return this;
        }
        
        public void AddStatus(Types types) => Types |= types;
        public void RemoveStatus(Types types) => Types &= ~types;
        public bool IsVisible() => !Types.HasFlag(Types.Hidden);
    }
 
}