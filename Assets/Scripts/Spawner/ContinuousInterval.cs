using System;
using System.Collections;
using System.Collections.Generic;
using Data.Types;
using Units;
using UnityEngine;

namespace Spawner {
    public class ContinuousInterval : Interval, IUnitInterval {
        public override float SpawnStartupTime { get; protected set; }
        public override float DelayBetweenSpawns { get; protected set; }
        public override bool Enabled { get; protected set; }
        public override void Enable() => Enabled = true;
        public override void Disable() => Enabled = false;
        public override float DelayBetweenWaves { get; protected set; } = 10f;
        public float CurrentDelay { get; private set; } = 0f;
        public List<Unit> CurrentWave { get; private set; } = new List<Unit>();

        public override Interval Initialize(Func<IEnumerator> spawn, Spawner owner, float delayBetweenWaves,
            float delayBetweenSpawns, float spawnStartupTime) {
            CurrentWave = owner.owningPlayer.Units;
            Unit.OnDeath += CheckForPlayerDeath;
            return base.Initialize(spawn, owner, delayBetweenWaves, delayBetweenSpawns, spawnStartupTime);
        }

        private void OnEnable() => Enabled = true;
        public void Update() => HandleUpdate();
        /// <summary>
        /// Disables interval logic once player has died to prevent case where the
        /// timer ticks down after player death and they transition to upgrade the screen
        /// </summary>
        /// <param name="obj"></param>
        private void CheckForPlayerDeath(Unit obj) {
            if(obj.Owner.ControlType == ControlType.Local) Disable();
        }
        public override void HandleUpdate() {
            if (!Enabled) return;

            // return if wave is alive
            if (CurrentWave.Count > 0) return;

            CurrentDelay = Mathf.Clamp(CurrentDelay - Time.deltaTime, 0, DelayBetweenWaves);

            // wait for delay between waves
            if (CurrentDelay > 0) return;

            HandleNewWave();
        }

        private void HandleNewWave() {
            if (Owner.Handler.hordeSpawnData.Waves.Count == Owner.Handler.currentIndex) {
                Owner.HandleWavesCleared();
                Disable();
            }
            else {
                StartCoroutine(Spawn());
                CurrentDelay = DelayBetweenWaves;
                Owner.Handler.IncrementIndex();
            }
        }
    }
}