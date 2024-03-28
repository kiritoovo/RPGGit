using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Utils;
using RPG.Attribute;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseRange = 5f;
        [SerializeField] float suspicionTime=3f;
        [SerializeField]float maxAggravateTime=3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance=0.3f;
        [SerializeField] float patrolSpeedFraction=0.3f;
        [SerializeField]float shoutDistance=5f;
        float lastSeeTime;
        float aggravateTime=Mathf.Infinity;
        GameObject player;
        Health health;
        Fighter fighter;
        LazyValue<Vector3> guardPostion;
        Mover mover;
        int currentWaypointIndex=0;
        // Start is called before the first frame update
        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
           fighter=GetComponent<Fighter>();
           health=GetComponent<Health>();
           mover=GetComponent<Mover>();
           guardPostion=new LazyValue<Vector3>(GetPosition);
        } 
        
        private Vector3 GetPosition()
        {
            return transform.position;
        }

        void Start()
        {

            guardPostion.ForceInit();
        }

 

        // Update is called once per frame
        void Update()
        {
            lastSeeTime+=Time.deltaTime;
            aggravateTime+=Time.deltaTime;
            if(health.IsDead())return;
            if (IsAggrevated()&&fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if(lastSeeTime<=suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition=guardPostion.value;
            if(patrolPath!=null)
            {
                if(AtWayPoint())
                {
                    CirclyWaypoint();
                }
                nextPosition=GetCurrentWaypoint();
            }
            mover.MovementTo(nextPosition,patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CirclyWaypoint()
        {
            currentWaypointIndex=patrolPath.GetNextWaypoint(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distranceToWaypoint=Vector3.Distance(transform.position,GetCurrentWaypoint());
                return distranceToWaypoint < waypointTolerance;
        }

        void SuspicionBehaviour()
        {
            GetComponent<ActionSchedule>().StopLastAction();
        }

        void AttackBehaviour()
        {
            AggrevateNear();
            lastSeeTime =0;
            GetComponent<Fighter>().Fight(player);
        }

        private void AggrevateNear()
        {
            RaycastHit[] raycastHits=Physics.SphereCastAll(transform.position,shoutDistance,Vector3.up,0);
            foreach(RaycastHit hit in raycastHits)
            {
                AIController aIController=hit.collider.GetComponent<AIController>();
                if(aIController!=null)aIController.Aggrevate();
            }
        }

        bool IsAggrevated()
        {
            float DistanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
            return (DistanceToPlayer<=chaseRange)||(aggravateTime<maxAggravateTime);
        }

        public void Aggrevate()
        {
            aggravateTime=0;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(this.transform.position,chaseRange);
            
        }


    }

}

