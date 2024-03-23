using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control{
    public class PatrolPath : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos() {
            for(int i=0;i<transform.childCount;i++)
            {
                int j=GetNextWaypoint(i);
                Gizmos.DrawSphere(GetWaypoint(i),0.3f);
                Gizmos.DrawLine(GetWaypoint(i),GetWaypoint(j));
            }
        }

        public int GetNextWaypoint(int i)
        {
            return (i+1)%transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}


