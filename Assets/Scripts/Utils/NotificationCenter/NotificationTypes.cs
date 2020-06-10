namespace Utils.NotificationCenter
{
    public static class NotificationTypes
    {
        // Ability
        public const string 
            AbilityWillActivate = "AbilityWillActivate",
            AbilityDidActivate = "AbilityDidActivate",
            AbilityDidConnect = "AbilityDidConnect",
            AbilityCompleted = "AbilityCompleted";

        // unit input
        public const string
            DisableRotation = "DisableRotation",
            EnableRotation = "EnableRotation",
            DisableMovement = "DisableMovement",
            EnableMovement = "EnableMovement",
            DisableMovementAndRotation = "DisableMovementAndRotation",
            EnableMovementAndRotation = "EnableMovementAndRotation",
            DisableInput = "DisableInput",
            EnableInput = "EnableInput";
    }
}