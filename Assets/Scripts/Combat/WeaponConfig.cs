using System;
using System.Collections.Generic;
using RPG.Attribute;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Make a new weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon weaponPrefeb=null;
        [SerializeField] AnimatorOverrideController animatorOverride=null;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float weaponDamageBonus=0;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] bool isRight=true;
        [SerializeField] Projectile projectile=null;

        const String weaponName="Weapon";

        public Weapon SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon=null;
            if (weaponPrefeb != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(weaponPrefeb, handTransform);
                weapon.gameObject.name = weaponName;
                
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightTransform, Transform leftTransform)
        {
            Transform oldWeapon=rightTransform.Find(weaponName);
            if(oldWeapon==null)
            {
                oldWeapon=leftTransform.Find(weaponName);
            }
            if(oldWeapon==null)return;
            oldWeapon.name="DESTORYING";
            Destroy(oldWeapon.gameObject);
        }

        public void SpwanProjectile(Transform rightTransform,Transform leftTransform,Health target,GameObject attacker,float damage)
        {
            if(projectile!=null)
            {
                
               Projectile projectileInstace= Instantiate(projectile,GetTransform(rightTransform,leftTransform).position,Quaternion.identity);
                projectileInstace.SetTarget(target,attacker,damage);
            }
            
        }

        private Transform GetTransform(Transform rightTransform, Transform leftTransform)
        {
            Transform weaponTransform;
            if (isRight) weaponTransform = rightTransform;
            else weaponTransform = leftTransform;
            return weaponTransform;
        }

        public bool HasProjectile()
        {
            return (projectile!=null);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetDamageBonus()
        {
            return weaponDamageBonus;
        }

        public Projectile GetProjectile()
        {
            return projectile;
        }

       
    }
}