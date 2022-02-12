using System;
using Abilities;
using Data.Types;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Status {
    public abstract class MonoStatus : MonoBehaviour {
        protected Unit Owner;
        public float Duration { get; protected set; }
        public float TimeLeft { get;protected set; }
        public int Amount { get; protected set; }
        public bool IsTimed { get; set; }
        public bool Initialized { get; set; }
        public abstract StatusType Type {  get; protected set; }
        public virtual MonoStatus Initialize(Unit owner, float duration, int amount) {
            Owner = owner;
            Duration = duration;
            IsTimed = true;
            TimeLeft = duration;
            Amount = amount;
            Initialized = true;
            EnableEffect();
            return this;
        }

        public virtual MonoStatus Initialize(Unit owner, bool isTimed, int amount) {
            Owner = owner;
            IsTimed = isTimed;
            Amount = amount;
            Initialized = true;
            EnableEffect();
            return this;
        }


        protected abstract void EnableEffect();

        public virtual void DisableEffect() {
            Destroy(this, 0.01f);
        }

        public virtual void TriggerEffect(Ability catalyst) {
            DisableEffect();
        }

        public virtual void ReapplyStatus(int amount) {
            IsTimed = false;
            Amount += amount;
        }
        
        public virtual void ReapplyStatus(float duration, int amount) {
            Duration = duration;
            TimeLeft = Duration - TimeLeft;
            Amount += amount;
        }

        protected virtual void Update() {
            if (!IsTimed || !Initialized) return;
            TimeLeft = Clamp(TimeLeft - Time.deltaTime, 0, Duration);
            if(TimeLeft > 0) return;
            // todo - trigger vs disable logic
            Owner.StatusComponent.RemoveStatus(Type);
        }
    }
}