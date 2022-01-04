using System;
using Data.UnitData;
using Units;
using UnityEngine;

namespace Components {
    public sealed class FundsComponent : MonoBehaviour {
        public int Balance { get; private set; }
        public Unit Owner { get; private set; }

        public FundsComponent Initialize(Unit owner, FundsData fundsData) {
            Balance = fundsData.balance;
            Owner = owner;
            return this;
        }

        public void AddFunds(int amount) {
            AdjustBalance(amount);
        }

        public bool RemoveFunds(int amount) {
            if (!ContainsEnoughFunds(amount).containsEnoughFunds) return false;

            AdjustBalance(-amount);
            return true;
        }

        public void SetBalance(int amount) => Balance = amount;

        public (bool containsEnoughFunds, int remainder) ContainsEnoughFunds(int amount) {
            var remainder = Balance - amount;

            return (remainder >= 0, Mathf.Abs(remainder));
        }

        private void AdjustBalance(int amount) => Balance += amount;
    }
}