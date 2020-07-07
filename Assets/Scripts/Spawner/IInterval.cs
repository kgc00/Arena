namespace Spawner {
    
    public interface IInterval
    {
        bool Enabled { get; }
        void Enable();
        void Disable();
        void HandleUpdate();
        
        float SpawnStartupTime { get; }
        float DelayBetweenSpawns { get; }
        float DelayBetweenWaves { get; }
    }


}