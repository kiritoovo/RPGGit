using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField]
    GameObject DestroyGameobject=null;
    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<ParticleSystem>().IsAlive())
        {
            if(DestroyGameobject!=null)
            {
                Destroy(DestroyGameobject);
            }
            else{
                Destroy(gameObject);
            }
            
        }
    }
}
}


