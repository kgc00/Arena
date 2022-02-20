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
            PurchaseComplete = "PurchaseComplete",
            LockedSkillInspected = "LockedSkillInspected";

        public const string
            ChargeDidImpactWall = "ChargeDidImpactWall";
        
        // sfx
        public const string
            // player
            DidCastBurst = "DidCastBurst",
            DidCastPierceAndPull = "DidCastPierceAndPull",
            DidCastConceal = "DidCastConceal",
            DidCastPrey = "DidCastPrey",
            DidCastMark = "DidCastMark",
            DidCastRain = "DidCastRain",
            DidConnectBurst = "DidConnectBurst",
            DidConnectPierceAndPull = "DidConnectPierceAndPull",
            DidLaunchPierceAndPull = "DidLaunchPierceAndPull",
            DidConnectPrey = "DidConnectPrey",
            DidConnectMark = "DidConnectMark",
            DidApplyMark = "DidApplyMark",
            DidTriggerMark = "DidTriggerMark",
            DidLevelUp = "DidLevelUp",
            RainDidFinish = "RainDidFinish",
            // monsters
            DidConnectCharge = "DidConnectCharge",
            DidCastChainFlame = "DidCastChainFlame",
            DidConnectChainFlame = "DidConnectChainFlame",
            DidCastIceBolt = "DidCastIceBolt",
            DidConnectIceBolt = "DidConnectIceBolt",
            DidCastRoar = "DidCastRoar",
            DidConnectRoar = "DidConnectRoar",
            AttackDidCollide = "AttackDidCollide",
            // UI
            DidToggleShopTab = "DidToggleShopTab",
            DidClickShopButton = "DidClickShopButton",
            // Items
            DidPickupHealth = "DidPickupHealth";

        public static string UnitDidSpawn = "UnitDidSpawn",
            ComponentsDidUpdate = "ComponentsDidUpdate",
            GameOver = "GameOver";
    }
}