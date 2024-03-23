using System.Collections;
using System.Collections.Generic;
using RPG.Attribute;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Canvas canvas;

    private void Start() {
        if(Mathf.Approximately(health.GetHealthPercent(), 1))
            canvas.enabled=false;
    }

    private void Update() {
        if (Mathf.Approximately(health.GetHealthPercent(), 1))
            canvas.enabled = false;
    }

    // Update is called once per frame
    public void UpdateHealthBar()
    {
        if(canvas.enabled==false)canvas.enabled=true;
        if (Mathf.Approximately(health.GetHealthPercent(), 0))
        {
            canvas.enabled = false;
        }
        rectTransform.localScale=new Vector3(health.GetHealthPercent(),1);
    }
}
