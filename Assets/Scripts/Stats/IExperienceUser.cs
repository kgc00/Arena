namespace Stats
{
    public interface IExperienceUser
    {
        ExperienceComponent ExperienceComponent { get; }
        void OnLevelUp();
    }
}