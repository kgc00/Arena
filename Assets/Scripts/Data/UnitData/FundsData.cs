using System;
using UnityEngine;

namespace Data.UnitData {
    [Serializable]
    public class FundsData {
        [SerializeField]public int balance;
        [SerializeField] public int bounty;
        public FundsData(FundsData fundsData) {
            balance = fundsData.balance;
            bounty = fundsData.bounty;
        }
    }
}