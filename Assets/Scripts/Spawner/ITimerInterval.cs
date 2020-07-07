namespace Spawner {
    public interface ITimerInterval : IInterval
    {
        float SpawnInterval { get; }
        float TimeSinceLastSpawn { get; }
    }
}