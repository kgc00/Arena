using Units;
using UnityEngine;

namespace Utils {
    public static class GameObjectExtensions {
        public static Unit GetUnitComponent(this GameObject obj) => obj.transform.root.GetComponentInChildren<Unit>();
        public static Transform GetWeaponTransform(this GameObject obj) => obj.transform.root.GetComponentInChildren<WeaponTransform>().transform;
    }
}