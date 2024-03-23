using System;
using RPG.Attribute;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay:MonoBehaviour{

        BaseStats baseStats;

        private void Awake() {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update() {
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            GetComponent<TextMeshProUGUI>().text=String.Format("{0}",baseStats.GetLevel());
        }
    }
}
