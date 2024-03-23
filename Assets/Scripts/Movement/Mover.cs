using RPG.Attribute;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction,ISaveable
    {
        [SerializeField] float maxSpeed=7f;
        [SerializeField] float maxNavPathlength = 30f;
        // Start is called before the first frame update
        NavMeshAgent navMeshAgent;
        Health health;

        void Awake()
        {
            navMeshAgent=GetComponent<NavMeshAgent>();
            health=GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            // if(Input.GetMouseButton(0))
            // {
            //     MoveToCursor();
            // }
            SetAnimator();
            if(health.IsDead())
                navMeshAgent.enabled=false;
        }

        public void MovementTo(Vector3 destination,float speedFraction)
        {
            GetComponent<ActionSchedule>().StartAction(this);
            MoveTo(destination,speedFraction);
        }

        public void MoveTo(Vector3 destination,float speedFraction)
        {
                navMeshAgent.speed=maxSpeed*speedFraction;
                navMeshAgent.destination = destination;
                navMeshAgent.isStopped = false;
        }

        public bool CanMoveTo(Vector3 target)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            bool hasCastPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, navMeshPath);
            if (!hasCastPath) return false;
            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(navMeshPath) > maxNavPathlength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath navMeshPath)
        {
            float sum = 0;
            if (navMeshPath.corners.Length < 2) return sum;
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
            {
                sum += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
            }
            return sum;
        }


        public void Cancel()
        {
            navMeshAgent.isStopped=true;
        }

        private void SetAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            GetComponent<Animator>().SetFloat("forwardSpeed", localVelocity.z);
        }

        public object CaptrueState()
        {
            SerializeVector3 vector3=new SerializeVector3(gameObject.transform.position);
            return vector3;
        }

        public void RestoreState(object state)
        {
            SerializeVector3 vector3=(SerializeVector3)state;
            navMeshAgent.enabled=false;
            gameObject.transform.position=vector3.ToVector();
            navMeshAgent.enabled=true;
            GetComponent<ActionSchedule>().StopLastAction();
        }
    }

}