using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI damageValue;

    public void DestroyText()
    {
        Destroy(gameObject);
    }

    public void SetDamage(float damge)
    {
        damageValue.text=String.Format("{0:0}",damge);
    }
}
