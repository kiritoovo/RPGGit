using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Inventories{
    public class RandomDropper : ItemDropper
{

    [Tooltip("How far")]
    [SerializeField]float scatterDistance=1;
    [SerializeField]InventoryItem dropItem;

    const int ATTEMPTS=30;

    public void DieDropItem()
    {
        DropItem(dropItem,1);
    }

    // Start is called before the first frame update
    protected Vector3 GetDropLocation()
    {
        for(int i=0;i<ATTEMPTS;i++)
        {
            Vector3 randomPoint=transform.position+Random.insideUnitSphere*scatterDistance;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint,out hit,0.1f,NavMesh.AllAreas))
        {
            return hit.position;
        }
        }
        
        return transform.position;
    }
}
}

