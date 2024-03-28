using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject LevelupEffect;
        [SerializeField] bool isModify=false;

        public event Action OnLevelUp;
        Experience experience ;
        LazyValue<int> currentLevel;

        private void Awake() {
            experience= GetComponent<Experience>();
            currentLevel=new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
            
        }

        private void OnEnable() {
            if (experience != null)
            {
                experience.onExpGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null)
            {
                experience.onExpGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                SpawnLevelupEffect();
                OnLevelUp();
            }
        }

        private void SpawnLevelupEffect()
        {
            Instantiate(LevelupEffect,transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat))*(GetPercentageModifier(stat)+100)/100;
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!isModify) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }


        private float GetPercentageModifier(Stat stat)
        {
            if (!isModify) return 0;
            float sum = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float damage in modifierProvider.GetPercentageModifier(stat))
                {
                    sum += damage;
                }
            }
            return sum;
        }
        public int GetLevel()
        {

            return currentLevel.value;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperience();
            int penultimateLevel = progression.GetMaxLevel(characterClass,Stat.ExpToLevelUp);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExpToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}