using System;
using System.Collections;
using UnityEngine;

namespace Spawner {
    
    public abstract class Interval : MonoBehaviour, IInterval
    {
        public IInterval IntervalType { get; protected set; }
        public Spawner Owner { get; private set; }
        public Func<IEnumerator> Spawn { get; private set; }
        public bool Enabled { get; protected set; }
        public float SpawnStartupTime { get; protected set; }
        public float DelayBetweenSpawns { get; protected set; }
        public float DelayBetweenWaves { get; protected set; }

        public virtual Interval Initialize(Func<IEnumerator> spawn, Spawner owner, float delayBetweenWaves, float delayBetweenSpawns, float spawnStartupTime)
        {
            Spawn = spawn;
            Owner = owner;
            SpawnStartupTime = spawnStartupTime;
            DelayBetweenSpawns = delayBetweenSpawns;
            DelayBetweenWaves = delayBetweenWaves;
            return this;
        }

        public void Enable() { }

        public void Disable() { }

        public void HandleUpdate() { }
    }

}