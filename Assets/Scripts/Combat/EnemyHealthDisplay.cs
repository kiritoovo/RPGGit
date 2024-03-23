using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute{
    public class EnemyHealthDisplay:MonoBehaviour{

        Fighter fighter;

        private void Awake() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            if(fighter.GetTarget()==null) GetComponent<TextMeshProUGUI>().text = String.Format("");
            else GetComponent<TextMeshProUGUI>().text=String.Format("{0:0}/{1:0}",fighter.GetTarget().GetCurrentHealth(),fighter.GetTarget().GetMaxHealth());
        }
    }
}
