using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Spawner {
    
    public class ContinuousInterval : Interval, IUnitInterval
    {
        
        public float SpawnStartupTime { get;private set; }
        public float DelayBetweenSpawns { get; private set;}
        public bool Enabled { get; private set; }
        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;
        public float DelayBetweenWaves { get; private set; } = 10f;
        public float CurrentDelay { get; private set; } = 0f;
        public List<Unit> CurrentWave { get; private set;} = new List<Unit>();
        public override Interval Initialize(Func<IEnumerator> spawn, Spawner owner, float delayBetweenWaves, float delayBetweenSpawns, float spawnStartupTime)
        {
            CurrentWave = owner.OwningPlayer.Units;
            return base.Initialize(spawn, owner, delayBetweenWaves, delayBetweenSpawns, spawnStartupTime);
        }
        private void OnEnable() => Enabled = true;
        public void Update() => HandleUpdate();
        public void HandleUpdate()
        {
            if (!Enabled) return;
            
            // return if wave is alive
            if (CurrentWave.Count > 0) return;

            CurrentDelay = Mathf.Clamp(CurrentDelay - Time.deltaTime, 0, DelayBetweenWaves);

            // wait for delay between waves
            if (CurrentDelay > 0) return;
            
            StartCoroutine(Spawn());
            CurrentDelay = DelayBetweenWaves;
        }
    }


}