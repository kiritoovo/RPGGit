using System;
using RPG.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attribute{
    public class Health:MonoBehaviour,ISaveable{
       LazyValue<float> currentHealth;
       float maxHealth;
        bool isDead;
        [SerializeField] TakeDamageEvent onTakeDamage;

        [System.Serializable]
        public class TakeDamageEvent:UnityEvent<float>{}
        public UnityEvent OnDie;

        private void Awake() {
            currentHealth=new LazyValue<float>(GetInitialHealth);
            
        }

        private void Start() {
            currentHealth.ForceInit();
            maxHealth= GetInitialHealth();
            
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            GetComponent<BaseStats>().OnLevelUp+=UpdateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().OnLevelUp -= UpdateHealth;
        }

        public void UpdateHealth()
        {
            currentHealth.value=GetHealthPercentage()/100* GetComponent<BaseStats>().GetStat(Stat.Health);
            maxHealth= GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

    

        public void TakeDamage(GameObject attacker,float damage)
        {
            currentHealth.value=Math.Max((float)currentHealth.value-damage,0);
            onTakeDamage.Invoke(damage);
            if (currentHealth.value==0){
                OnDie.Invoke();
                Die();
                AwardExperience(attacker);
            }
              

        }

        private void AwardExperience(GameObject attacker)
        {
            Experience exp=attacker.GetComponent<Experience>();
            if(exp==null)return;
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceRefward));
        }

        public float GetHealthPercentage()
        {
            return GetHealthPercent() * 100;
        }

        public float GetHealthPercent()
        {
            return currentHealth.value / maxHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHealth.value;
        }

        private void Die()
        {
            GetComponent<ActionSchedule>().StartAction(null);
            if(isDead)return;
            isDead=true;
            GetComponent<Animator>().SetTrigger("dead");
        }

        

        public object CaptureState()
        {
           return currentHealth.value;
        }

        public void RestoreState(object state)
        {
            currentHealth.value=(float)state;
            maxHealth= GetComponent<BaseStats>().GetStat(Stat.Health);
            if (currentHealth.value<=0)Die();
        }

    }
}