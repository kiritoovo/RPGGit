using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute{
    public class HealthDisplay:MonoBehaviour{

        Health playerHealth;

        private void Awake() {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            GetComponent<TextMeshProUGUI>().text=String.Format("{0:0}/{1:0}",playerHealth.GetCurrentHealth(),playerHealth.GetMaxHealth());
        
        }
    }
}
