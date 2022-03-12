namespace Common {
    public static class Constants {
        public static bool IsDebug =
#if UNITY_EDITOR
            true;
#else
            false;
#endif
        
        public static string PrefabsPath = "Prefabs/";
        public static string MaterialsPath = "Materials/";
        public static string UIPath = "UI/";
        public static string AbilityModifierShopDataPath = "Data/Skills/AbilityModifierShopData/";
        public static string PoolingPath = "Data/Spawns/Pooling/";
        public static string SpawnsPath = "Data/Spawns/";
        public static string SavePath = "test.json";
        public static float PermaChaseRate = 0.20f;

    }
}