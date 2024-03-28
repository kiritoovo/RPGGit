
using System.Collections.Generic;
using RPG.Attribute;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

    namespace RPG.StateMachine
{

    public enum StateType{
        Idle,Patrol,Chase,React,SwordAttackState,BowAttackState,WaitBowState,WaitSwordState,DeadState
    }

    public class Parameter{
        public Animator animator;
       public Fighter fighter;
       public GameObject player;
       public  Mover mover;
       public Health health;
    }

    public class FSM : MonoBehaviour {
        
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 0.3f;
        [SerializeField] float patrolSpeedFraction = 0.3f;
        Vector3 guardPostion;
        [SerializeField]float chaseRange=10f;
        int currentWaypointIndex = 0;


        private IState currentStat;
        public Parameter parameter=new Parameter();
        private Dictionary<StateType,IState> states=new Dictionary<StateType, IState>();

        private void Awake() {
            parameter.mover=GetComponent<Mover>();
            parameter.animator = GetComponent<Animator>();
            parameter.fighter=GetComponent<Fighter>();
            parameter.player=GameObject.FindWithTag("Player");
            parameter.health=GetComponent<Health>();
        }

        private void Start() {
            guardPostion = this.transform.position;
            states.Add(StateType.Idle,new IdleState(this));
            states.Add(StateType.Patrol,new PatrolState(this));
            states.Add(StateType.Chase,new ChaseState(this));
            states.Add(StateType.SwordAttackState,new SwordAttackState(this));
            states.Add(StateType.BowAttackState, new BowAttackState(this));
            states.Add(StateType.WaitBowState, new WaitBowState(this));
            states.Add(StateType.WaitSwordState, new WaitSwordState(this));
            states.Add(StateType.DeadState, new DeadState(this));
            TransitionState(StateType.Idle);
 
        }

        private void Update() {
            currentStat.OnUpdate();
        }

        public void  TransitionState(StateType type)
        {
            if(currentStat!=null)
            {
                currentStat.OnExit();
            }
            currentStat=states[type];
            currentStat.OnEnter();
        }

        private bool AtWayPoint()
        {
            float distranceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distranceToWaypoint < waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CirclyWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypoint(currentWaypointIndex);
        }

        public void PatrolBehavior()
        {
            Vector3 nextPosition = guardPostion;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    CirclyWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            parameter.mover.MovementTo(nextPosition, patrolSpeedFraction);
        }

        public bool CanSwordAttack()
        {
            return Vector3.Distance(transform.position,parameter.player.transform.position)<5f;
        }

        public bool IsAggrevated()
        {
            float DistanceToPlayer = Vector3.Distance(parameter.player.transform.position, this.transform.position);
            return (DistanceToPlayer <= chaseRange);
        }
    }

}