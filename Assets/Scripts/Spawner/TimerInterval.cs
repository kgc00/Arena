using UnityEngine;

namespace Spawner {
    
    public class TimerInterval : Interval, ITimerInterval
    {
        
        public float SpawnStartupTime { get;private set; }
        public float DelayBetweenSpawns { get; private set;}
        public bool Enabled { get; private set; }
        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;

        public float SpawnInterval { get; private set; } = 6f;
        public float TimeSinceLastSpawn { get; private set;}

        private void OnEnable() => Enabled = false;

        public void Update() => HandleUpdate();

        public void HandleUpdate()
        {
            if (!Enabled) return;

            TimeSinceLastSpawn += Time.deltaTime;

            if (TimeSinceLastSpawn < SpawnInterval) return;
            
            StartCoroutine(Spawn());
            TimeSinceLastSpawn = 0f;
        }
    }
}