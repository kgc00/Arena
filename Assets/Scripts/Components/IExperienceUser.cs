namespace Components
{
    public interface IExperienceUser
    {
        ExperienceComponent ExperienceComponent { get; }
        void OnLevelUp();
    }
}