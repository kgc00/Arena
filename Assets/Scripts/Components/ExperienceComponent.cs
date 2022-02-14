﻿using Data.StatData;
using Data.Types;
using Units;
using UnityEngine;
using Utils.NotificationCenter;

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

        private const float EXP_CURVE_MODIFIER = 2.75f;
        private const int SKILL_POINTS_PER_LEVEL = 1;
        private const int MAX_LEVEL = 50;
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
            Level = LevelFromExp(CurrentExp);
            if (Level == prevLevel) return;
            SkillPoints += 1 * SKILL_POINTS_PER_LEVEL;
            Owner.OnLevelUp();
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
    }
}