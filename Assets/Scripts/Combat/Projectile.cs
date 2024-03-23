using System.Collections;
using System.Collections.Generic;
using RPG.Attribute;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Health target;
    [SerializeField] float speed=8f;
    [SerializeField] float projectileDamage=2f;
    [SerializeField] bool isHoming=false;
    [SerializeField] GameObject hitEffect=null;
    [SerializeField]float maxLifeTime=10f;
    [SerializeField] GameObject[] destroyAfterHit;
    [SerializeField]float destroyTime=0.3f;

    GameObject attacker;
    float sumDamage;

    private void Start() {
        transform.LookAt(GetAimTransform());
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null)return;
        if(isHoming&&!target.IsDead()){
            transform.LookAt(GetAimTransform());
        }
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
        
    }

    public void SetTarget(Health target,GameObject attacker,float damage)
    {
        this.target=target;
        this.attacker=attacker;
        sumDamage=damage;
        Destroy(gameObject,maxLifeTime);
    }

    Vector3 GetAimTransform()
    {
        CapsuleCollider capsuleCollider=target.GetComponent<CapsuleCollider>();
        if(capsuleCollider==null)
        {
            return target.transform.position;
        }
        return target.transform.position+capsuleCollider.height/2*Vector3.up;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Health>()==target)
        {
            if(target.IsDead())return;
            speed=0;
            if(hitEffect!=null)
            {
                Instantiate(hitEffect,GetAimTransform(),transform.rotation);
            }
            target.TakeDamage(attacker,sumDamage);
            foreach(GameObject toDestroy in destroyAfterHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject,destroyTime);
        }
    }

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }
}
