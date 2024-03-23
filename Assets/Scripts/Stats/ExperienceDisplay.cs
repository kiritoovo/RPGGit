using System;
using RPG.Attribute;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay:MonoBehaviour{

        Experience experience;


        private void Awake() {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            SetHealthUI();
        }

        private void SetHealthUI()
        {
            GetComponent<TextMeshProUGUI>().text=String.Format("{0}",experience.GetExperience());
        }
    }
}
