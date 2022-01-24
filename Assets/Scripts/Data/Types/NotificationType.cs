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

        // ability modifiers
        public const string
            EnableDoubleMovementSpeed = "EnableDoubleMovementSpeed",
            EnableConcealPersistentAddMarkOnHit = "EnableConcealPersistentAddMarkOnHit",
            UnitDidCollide = "UnitDidCollide";


        // shop
        public const string
            SkillScrollViewToggleToggledOn = "SkillScrollViewToggleToggledOn",
            SkillScrollViewToggleToggledOff = "SkillScrollViewToggleToggledOff",
            InsufficientFundsForPurchase = "InsufficientFundsForPurchase",
            PurchaseComplete = "PurchaseComplete";

        public const string
            ChargeDidImpactWall = "ChargeDidImpactWall";
    }
}