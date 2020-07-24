using System;
using System.Collections;
using UnityEngine;

namespace Spawner {
    
    public abstract class Interval : MonoBehaviour, IInterval
    {
        public virtual IInterval IntervalType { get; protected set; }
        protected Spawner Owner { get; private set; }
        protected Func<IEnumerator> Spawn { get; private set; }
        public virtual bool Enabled { get; protected set; }
        public virtual float SpawnStartupTime { get; protected set; }
        public virtual float DelayBetweenSpawns { get; protected set; }
        public virtual float DelayBetweenWaves { get; protected set; }

        public virtual Interval Initialize(Func<IEnumerator> spawn, Spawner owner, float delayBetweenWaves, float delayBetweenSpawns, float spawnStartupTime)
        {
            Spawn = spawn;
            Owner = owner;
            SpawnStartupTime = spawnStartupTime;
            DelayBetweenSpawns = delayBetweenSpawns;
            DelayBetweenWaves = delayBetweenWaves;
            return this;
        }

        public virtual void Enable() { }

        public virtual void Disable() { }

        public virtual void HandleUpdate() { }
    }

}