using Enums;
using Units;
using UnityEngine;

namespace Stats
{
    public class ExperienceComponent : MonoBehaviour
    {
        public static System.Action<Unit, float> onExperienceChanged = delegate { };
        public Unit Owner;
        public int CurrentExp { get; private set; }
        public int Bounty;
        
        public ExperienceComponent Initialize (Unit owner) {
            this.Owner = owner;
            CurrentExp = 0;
            this.Bounty = (int)owner.BaseStats.Bounty.Value;
            Unit.OnDeath += AwardBounty;
            return this;
        }

        void AdjustExperience(int amount)
        {
            CurrentExp += amount;
            Debug.Log($"Current EXP is {CurrentExp} EXP");
            CheckForLevelUp(CurrentExp);
        }

         void CheckForLevelUp(int currentExp)
        {
            
        }

        private void AwardBounty(Unit unit)
        {
            // Award xp to local player if monster died
            bool unitWasNotAi = unit.Owner.ControlType != ControlType.Ai;
            bool thisIsNotAPlayer = Owner.Owner.ControlType != ControlType.Local;
            if (unitWasNotAi || thisIsNotAPlayer) return;
            
            Debug.Log($"Awarding {unit.BaseStats.Bounty.Value} EXP");
            AdjustExperience((int)unit.BaseStats.Bounty.Value);
        }
    }
}