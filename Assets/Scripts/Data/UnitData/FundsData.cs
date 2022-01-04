using System;
using UnityEngine;

namespace Data.UnitData {
    [Serializable]
    public class FundsData {
        [SerializeField]public int balance;
        public FundsData(FundsData fundsData) {
            balance = fundsData.balance;
        }
    }
}