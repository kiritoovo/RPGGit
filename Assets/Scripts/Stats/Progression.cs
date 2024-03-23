using System.Collections.Generic;
using RPG.Attribute;
using UnityEngine;

namespace RPG.Stats{
    [CreateAssetMenu(fileName = "Progression", menuName = "Progression", order = 0)]
    public class Progression : ScriptableObject {
    public Dictionary<CharacterClass,Dictionary<Stat,float[]>> lookupTable;
    [SerializeField]ProgressionCharacterClass[] progressionCharacterClasses;

    [System.Serializable]
    class ProgressionCharacterClass{
        public CharacterClass characterClass;
        public ProgressionStat [] stats;
    }

    [System.Serializable]
    public class ProgressionStat{
        public Stat stat;
        public float[] leavels;
    }

    public float GetStat(Stat stat,CharacterClass characterClass,int level)
    {
        // foreach(ProgressionCharacterClass progressionCharacterClass in progressionCharacterClasses)
        // {
        //     if(progressionCharacterClass.characterClass!=characterClass)continue;
        //     foreach(ProgressionStat progressionStat in progressionCharacterClass.stats)
        //     {
        //         if(progressionStat.stat!=stat)continue;
        //         if(progressionStat.leavels.Length<level)continue;
        //         return progressionStat.leavels[level-1];
        //     }
        // }
        // return 0;
        BuildLookup();
        float[] leavels=lookupTable[characterClass][stat];
        if(leavels.Length<level){
            return 0;
        }
        return leavels[level-1];
    }

    public void BuildLookup()
    {
        if(lookupTable!=null)return;
        lookupTable=new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
        foreach(ProgressionCharacterClass progressionCharacterClass in progressionCharacterClasses)
        {
            var statLookupTable=new Dictionary<Stat,float[]>();
            foreach(ProgressionStat progressionStat in progressionCharacterClass.stats)
            {
                statLookupTable[progressionStat.stat]=progressionStat.leavels;
            }
            lookupTable[progressionCharacterClass.characterClass]=statLookupTable;
        }
        
    }

    public int GetMaxLevel(CharacterClass characterClass,Stat stat)
    {
        BuildLookup();
        float[] levels=lookupTable[characterClass][stat];
        return levels.Length;
    }
}
}

