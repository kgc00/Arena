namespace Data.Types
{
    public static class NotificationType
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
            MenuRequested = "MenuRequested",
            EnableInput = "EnableInput";

        public const string
            EnableDoubleMovementSpeed = "EnableDoubleMovementSpeed",
            UnitDidCollide = "UnitDidCollide";
    }
}