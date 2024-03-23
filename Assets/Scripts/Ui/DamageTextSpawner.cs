using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField]DamageText damageText=null;
    // Start is called before the first frame update
    public void Spwan(float damage)
    {
        DamageText instance=Instantiate<DamageText>(damageText,parent:transform);
        instance.SetDamage(damage);
    }
}
