using System;
using Units;
using UnityEngine;

namespace Stats
{
    public class StatusComponent : MonoBehaviour
    { 
        public Unit Owner { get; private set; }
        public Status Status { get; private set; } = (Status) 0;
        public StatusComponent Initialize (Unit owner) {
            Owner = owner;
            return this;
        }
        
        public void AddStatus(Status status) => Status |= status;
        public void RemoveStatus(Status status) => Status &= ~status;
        public bool IsVisible() => !Status.HasFlag(Status.Hidden);
    }
 
}