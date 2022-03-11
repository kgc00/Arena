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
        [field:SerializeField]public int CurrentExp { get; private set; }
        public int Bounty;
        public int Level { get; private set; }
        public int SkillPoints;

        private const float EXP_CURVE_MODIFIER = 1f;
        private const int SKILL_POINTS_PER_LEVEL = 1;
        private const int MAX_LEVEL = 50;
        // https://www.transum.org/Maths/Activity/Graph/Desmos.asp
        public static int LevelFromExp(int exp) => Mathf.FloorToInt(EXP_CURVE_MODIFIER * Mathf.Sqrt(Mathf.Max(exp, 1)));
        public static int ExpFromLevel(int level) => Mathf.FloorToInt(EXP_CURVE_MODIFIER * (Mathf.Max(level, 1) * Mathf.Max(level, 1)));
        
        public ExperienceComponent Initialize (Unit owner, ExperienceData data) {
            Owner = owner;
            CurrentExp = Mathf.Max(data.currentExp, 1);
            Level = Mathf.Max(LevelFromExp(CurrentExp), 1);
            Bounty = data.bounty;
            return this;
        }

        void AdjustExperience(int amount) {
            var prevExp = CurrentExp;
            var prevLevel = Level;
            CurrentExp += amount;
            Level = LevelFromExp(CurrentExp);
            onExperienceChanged(Owner, CurrentExp, prevExp);
            if (Level == prevLevel) return;
            SkillPoints += 1 * SKILL_POINTS_PER_LEVEL;
            Owner.OnLevelUp();
            onLevelUp(Owner, Level, prevLevel);
        }
        
        public void AwardBounty(int amount)
        {
            // debug function
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            
            if (ownerIsAi) return;
            
            AdjustExperience(Mathf.Abs(amount));
        }
        
        private void AwardBounty(Unit unit)
        {
            // Award xp to local player if monster died
            bool unitWasNotAi = unit.Owner.ControlType != ControlType.Ai;
            bool ownerIsAi = Owner.Owner.ControlType != ControlType.Local;
            bool unitWasSelf = Owner == null || unit == Owner; 
            
            if (unitWasNotAi || ownerIsAi || unitWasSelf) return;
            
            AdjustExperience(Mathf.Abs(unit.ExperienceComponent.Bounty));
        }

        public void Subscribe() {
            Unit.OnDeath += AwardBounty;
        }
        
        public void Unsubscribe() {
            Unit.OnDeath -= AwardBounty;
        }
    }
}