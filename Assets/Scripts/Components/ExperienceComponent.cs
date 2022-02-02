using Data.StatData;
using Data.Types;
using Units;
using UnityEngine;

namespace Components
{
    public class ExperienceComponent : MonoBehaviour
    {
        public static System.Action<Unit, float, float> onExperienceChanged = delegate { };
        public static System.Action<Unit, int, int> onLevelUp = delegate { };
        public Unit Owner;
        public int CurrentExp { get; private set; }
        public int Bounty;
        public int Level { get; private set; }

        private const float EXP_CURVE_MODIFIER = 0.75f;
        // https://www.transum.org/Maths/Activity/Graph/Desmos.asp
        private int LevelFromExp(int exp) => Mathf.RoundToInt(EXP_CURVE_MODIFIER * Mathf.Sqrt(Mathf.Max(exp, 1)));
        private int ExpFromLevel(int level) => Mathf.RoundToInt(EXP_CURVE_MODIFIER * (Mathf.Max(level, 1) * Mathf.Max(level, 1)));
        
        public ExperienceComponent Initialize (Unit owner, ExperienceData data) {
            this.Owner = owner;
            CurrentExp = data.currentExp;
            this.Bounty = data.bounty;
            Unit.OnDeath += AwardBounty;
            return this;
        }

        void AdjustExperience(int amount) {
            var prevExp = amount;
            var prevLevel = Level;
            CurrentExp += amount;
            onExperienceChanged(Owner, CurrentExp, prevExp);
            Debug.Log($"Current EXP is {CurrentExp} EXP");
            Level = LevelFromExp(CurrentExp);
            if (Level == prevLevel) return;
            Owner.OnLevelUp();
        }
        
        public void AwardBounty(int amount)
        {
            // debug function
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            
            if (ownerIsAi) return;
            
            Debug.Log($"Awarding {amount} EXP");
            AdjustExperience(Mathf.Abs(amount));
        }
        
        private void AwardBounty(Unit unit)
        {
            // Award xp to local player if monster died
            bool unitWasNotAi = unit.Owner.ControlType != ControlType.Ai;
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            
            if (unitWasNotAi || ownerIsAi) return;
            
            Debug.Log($"Awarding {unit.ExperienceComponent.Bounty} EXP");
            AdjustExperience(Mathf.Abs(unit.ExperienceComponent.Bounty));
        }
    }
}