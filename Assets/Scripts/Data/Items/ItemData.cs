using System;
using UnityEngine;

namespace Data.Items {
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 0)]

    [Serializable]
    public class ItemData : ScriptableObject {
        [SerializeField] public string Name;
        [SerializeField] public int Cost;
        [SerializeField] public string Description;
        [SerializeField] public Sprite ItemImage;
        [SerializeField] public ItemType ItemType;
    }
}