using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attribute;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using Unity.VisualScripting;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        [SerializeField]float maxNavMeshProjectionDistance=1f;
        [SerializeField]float RaycastRadius=0.5f;


        [System.Serializable]
        public struct CursorMapping{
           public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]
        CursorMapping[] cursorMappings;

        private void Awake() {
            health=GetComponent<Health>();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(InteractWithUI())
            return;
            if(health.IsDead())
            {
                SetCursor(CursorType.none);
                return;
            }
            if(InteractWithComponent())return;
            if(InteractWithMovement())return;
            SetCursor(CursorType.none);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastsSort();
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables=hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastsSort()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetRay(),RaycastRadius);
            float[] distance=new float[hits.Length];
            for(int i=0;i<hits.Length;i++)
            {
                distance[i]=hits[i].distance;
            }
            Array.Sort(distance,hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.Ui);
                return true;
            }
            else return false;
        }

        // private bool InteractWithCombat()
        // {
        //     RaycastHit[] hits=Physics.RaycastAll(GetRay());
        //     foreach(RaycastHit raycastHit in hits)
        //     {
        //         CombatTarget target=raycastHit.transform.GetComponent<CombatTarget>();
        //         if(target==null)continue;
        //         if(!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

        //         if(Input.GetMouseButton(0))
        //         {
        //             GetComponent<Fighter>().Fight(target.gameObject);
        //         }
        //         SetCursor(CursorType.combat);
        //          return true;   
        //     }
        //     return false;
        // }

        private bool InteractWithMovement()
        {
            Vector3 target=new Vector3();
            
            bool isHit=RaycastNavMesh(out target );
            if (isHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().MovementTo(target,1);
                }
                SetCursor(CursorType.movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target=new Vector3();
            RaycastHit hit;
            bool isHit = Physics.Raycast(GetRay(), out hit);
            if(!isHit)return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh=NavMesh.SamplePosition(hit.point,out navMeshHit,maxNavMeshProjectionDistance,NavMesh.AllAreas);
            if(!hasCastToNavMesh)return false;
            target=navMeshHit.position;

            // NavMeshPath navMeshPath=new NavMeshPath();
            // bool hasCastPath=NavMesh.CalculatePath(transform.position,target,NavMesh.AllAreas,navMeshPath);
            // if(!hasCastPath)return false;
            // if(navMeshPath.status!=NavMeshPathStatus.PathComplete)return false;
            // if(GetPathLength(navMeshPath)>maxNavPathlength)return false;
            return true;
        }

       

        private static Ray GetRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType combat)
        {
            CursorMapping cursorMapping=GetCursorMapping(combat);
            Cursor.SetCursor(cursorMapping.texture,cursorMapping.hotspot,CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType combat)
        {
            foreach(var item in cursorMappings)
            {
                if(combat==item.cursorType)
                return item;
            }
            return cursorMappings[0];
        }
    }
}
