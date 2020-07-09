using Data.StatData;
using Data.Types;
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
        
        public ExperienceComponent Initialize (Unit owner, ExperienceData data) {
            this.Owner = owner;
            CurrentExp = data.currentExp;
            this.Bounty = data.bounty;
            Unit.OnDeath += AwardBounty;
            return this;
        }

        void AdjustExperience(int amount)
        {
            CurrentExp += amount;
            Debug.Log($"Current EXP is {CurrentExp} EXP");
            CheckForLevelUp(CurrentExp);
        }

         void CheckForLevelUp(int currentExp) { }

        private void AwardBounty(Unit unit)
        {
            // Award xp to local player if monster died
            bool unitWasNotAi = unit.Owner.ControlType != ControlType.Ai;
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            
            if (unitWasNotAi || ownerIsAi) return;
            
            Debug.Log($"Awarding {unit.ExperienceComponent.Bounty} EXP");
            AdjustExperience(unit.ExperienceComponent.Bounty);
        }
    }
}