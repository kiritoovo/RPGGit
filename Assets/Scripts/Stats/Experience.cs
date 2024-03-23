using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats{
    public class Experience:MonoBehaviour,ISaveable{
        [SerializeField] float experience;
        public event Action onExpGained;



        public void GainExperience(float expereince){
            this.experience+=expereince;
            onExpGained();
        }

        public float GetExperience()
        {
            return experience;
        }

        public object CaptrueState()
        {
            return experience;   
        }        
        public void RestoreState(object state)
        {
            experience=(float)state;
        }
    }
}