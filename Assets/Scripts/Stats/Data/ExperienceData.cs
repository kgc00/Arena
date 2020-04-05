﻿using System;
using UnityEngine;

namespace Stats.Data
{
    [Serializable]
    public class ExperienceData
    {
        [HideInInspector] public int currentExp;
        [SerializeField] public int bounty;
    }
}