using System.Collections.Generic;
using Abilities;
using Data.Types;
using Units;
using UnityEngine;

namespace Status
{
    public class StatusComponent : MonoBehaviour {
        public Unit Owner { get; private set; }
        public StatusType StatusType { get; private set; } = 0;
        private Dictionary<StatusType, MonoStatus> currentStatusEffects;
        private Dictionary<StatusType, bool> canReapplyEffect;
        public StatusComponent Initialize (Unit owner) {
            Owner = owner;
            currentStatusEffects = new Dictionary<StatusType, MonoStatus>();
            canReapplyEffect = new Dictionary<StatusType, bool>();
            canReapplyEffect.Add(StatusType.Fragile, false);
            canReapplyEffect.Add(StatusType.Healthy, false);
            canReapplyEffect.Add(StatusType.Hidden, false);
            canReapplyEffect.Add(StatusType.Marked, false);
            canReapplyEffect.Add(StatusType.Rooted, false);
            canReapplyEffect.Add(StatusType.Slowed, false);
            canReapplyEffect.Add(StatusType.Silenced, false);
            canReapplyEffect.Add(StatusType.Stunned, false);
            canReapplyEffect.Add(StatusType.DragonFury, false);
            return this;
        }

        public void EnableReapplyEffect(StatusType statusType) {
            canReapplyEffect[statusType] = true;
        }
        
        public void AddStatus(StatusType statusType, float duration, int amount) {
            if (StatusType.HasFlag(statusType)) {
                if (canReapplyEffect[statusType]) {
                    ReapplyTimedStatus(statusType, duration, amount);
                }
            } else {
                AddTimedStatus(statusType, duration, amount);
            }
            StatusType |= statusType;
        }


        public void AddStatus(StatusType statusType, int amount) {
            if (StatusType.HasFlag(statusType)) {
                if(canReapplyEffect[statusType]) {
                    ReapplyUntimedStatus(statusType, amount);
                }
            }else {
                AddUntimedStatus(statusType, amount);
            }
            StatusType |= statusType;
        }

        private void AddUntimedStatus(StatusType statusType, int amount) {
            switch (statusType) {
                case StatusType.Marked:
                    currentStatusEffects.Add(StatusType.Marked, gameObject.AddComponent<Marked>().Initialize(Owner, false, amount));
                    break;
                case StatusType.Hidden:
                    break;
            }
        }
        
        private void ReapplyUntimedStatus(StatusType statusType, int amount) {
            switch (statusType) {
                case StatusType.Marked:
                    currentStatusEffects[StatusType.Marked].ReapplyStatus(amount);
                    break;
            }
        }

        private void AddTimedStatus(StatusType statusType, float duration, int amount) {
            switch (statusType) {
                case StatusType.Stunned:
                    currentStatusEffects.Add(StatusType.Stunned, gameObject.AddComponent<Stunned>().Initialize(Owner, duration, amount));
                    break;
            }
        }

        private void ReapplyTimedStatus(StatusType statusType, float duration, int amount) { 
            switch (statusType) {
                case StatusType.Stunned:
                    currentStatusEffects[StatusType.Stunned].ReapplyStatus(duration, amount);
                    break;
            }
        }

        public void TriggerStatus(StatusType statusType, Ability catalyst) {
            if (!StatusType.HasFlag(statusType)) return;
            currentStatusEffects[statusType].TriggerEffect(catalyst);
            currentStatusEffects.Remove(statusType);
            StatusType &= ~statusType;
        }

        public void RemoveStatus(StatusType statusType) {
            if (!StatusType.HasFlag(statusType)) return;
            // some legacy effects dont use the monostatus system
            if(currentStatusEffects.ContainsKey(statusType)) {
                currentStatusEffects[statusType].DisableEffect();
                currentStatusEffects.Remove(statusType);
            }
            StatusType &= ~statusType;
        }

        public bool IsVisible() => !StatusType.HasFlag(StatusType.Hidden);
        public bool IsStunned() => StatusType.HasFlag(StatusType.Stunned);
        public bool IsMarked() => StatusType.HasFlag(StatusType.Marked);
    }
 
}