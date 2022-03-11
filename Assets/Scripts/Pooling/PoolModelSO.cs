using System.Collections.Generic;
using UnityEngine;

namespace Pooling {
    [CreateAssetMenu(fileName = "PoolModelSO", menuName = "ScriptableObjects/PoolModelSO", order = 0)]
    public class PoolModelSO : ScriptableObject {
        public List<PoolModel> models;
    }
}