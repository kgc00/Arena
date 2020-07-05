using System;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace Status {
    public abstract class MonoStatus : MonoBehaviour {
        protected Unit Owner;
        public float Duration { get; protected set; }
        public float TimeLeft { get;protected set; }
        public float Amount { get; protected set; }

        public virtual MonoStatus Initialize(Unit owner, float duration, float amount) {
            Owner = owner;
            Duration = duration;
            TimeLeft = duration;
            Amount = amount;
            EnableEffect();
            return this;
        }

        protected abstract void EnableEffect();

        protected virtual void DisableEffect() {
            Destroy(this, 0.01f);
        }

        protected virtual void Update() {
            TimeLeft = Clamp(TimeLeft - Time.deltaTime, 0, Duration);

            if(TimeLeft > 0) return;

            DisableEffect();
        }
    }
}