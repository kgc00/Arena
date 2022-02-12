using UnityEngine;

namespace Spawner {
    
    public class TimerInterval : Interval, ITimerInterval {
        public bool withRandomOffset;
        public override void Enable() => Enabled = true;
        public override void Disable() => Enabled = false;

        public float SpawnInterval { get; private set; } = 6f;
        public float TimeSinceLastSpawn { get; private set;}

        private void OnEnable() => Enabled = false;

        public void Update() => HandleUpdate();

        public override void HandleUpdate()
        {
            if (!Enabled) return;

            TimeSinceLastSpawn += Time.deltaTime;

            if (TimeSinceLastSpawn < SpawnInterval) return;
            
            StartCoroutine(Spawn());
            TimeSinceLastSpawn = 0f;
        }
    }
}