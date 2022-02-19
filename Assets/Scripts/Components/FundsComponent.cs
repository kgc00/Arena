using System;
using Data.Types;
using Data.UnitData;
using Units;
using UnityEngine;

namespace Components {
    public sealed class FundsComponent : MonoBehaviour {
        public int Balance { get; private set; }
        public Unit Owner { get; private set; }
        public int Bounty { get; private set; }

        public FundsComponent Initialize(Unit owner, FundsData fundsData) {
            Balance = fundsData.balance;
            Bounty = fundsData.bounty;
            Owner = owner;
            Unit.OnDeath += AwardBounty;
            return this;
        }

        private void OnDestroy() {
            Unit.OnDeath -= AwardBounty;
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
        
        
        private void AwardBounty(Unit unit)
        {
            // Award gold to local player if monster died
            bool unitWasNotAi = unit.Owner.ControlType != ControlType.Ai;
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            bool unitWasSelf = Owner == null || unit == Owner; 
            
            if (unitWasNotAi || ownerIsAi || unitWasSelf) return;
            
            var bounty = Mathf.Abs(unit.FundsComponent.Bounty);
            AddFunds(bounty);
        }
    }
}