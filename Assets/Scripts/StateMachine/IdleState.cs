
using RPG.Combat;
using RPG.Control;
using UnityEngine;

namespace RPG.StateMachine{
    public class IdleState : IState
    {
        
        private FSM manager;
        private Parameter parameter;


        public IdleState(FSM manager)
        {
            this.manager=manager;
            this.parameter=manager.parameter;
        }

        public void OnEnter()
        {
            parameter.animator.Play("Locomotion");
            
        }
        public void OnUpdate()
        {
            manager.TransitionState(StateType.Patrol);
            if(parameter.health.IsDead())manager.TransitionState(StateType.DeadState);
        }

        public void OnExit()
        {
            
        }

        
    }

    public class PatrolState : IState
    {
        private FSM manager;
        private Parameter parameter;

        public PatrolState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {
            parameter.animator.Play("Locomotion");
        }
        
        public void OnUpdate()
        {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
            manager.PatrolBehavior();
            if (parameter.fighter.CanAttack(parameter.player)&&manager.IsAggrevated())
            {
                manager.TransitionState(StateType.Chase);
            }
        }

        public void OnExit()
        {
            
        }

        
    }

    public class ChaseState : IState
    {
        private FSM manager;
        private Parameter parameter;

        public ChaseState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {
            
        }
        
        public void OnUpdate()
        {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
            parameter.mover.MovementTo(parameter.player.transform.position,0.7f);
            if(manager.CanSwordAttack())
            {
                manager.TransitionState(StateType.SwordAttackState);
            }
        }

        public void OnExit()
        {
            
        }

        
    }

    public class SwordAttackState : IState
    {
        private FSM manager;
        private Parameter parameter;

        public SwordAttackState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {
            parameter.fighter.EquipWeapon(UnityEngine.Resources.Load<WeaponConfig>("Sword"));
            parameter.fighter.Fight(parameter.player);
            manager.TransitionState(StateType.WaitBowState);
        }
        public void OnUpdate()
                {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
        }
        public void OnExit()
        {
           
        }

        
    }

    public class BowAttackState : IState
    {
        private FSM manager;
        private Parameter parameter;

        public BowAttackState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {
            parameter.fighter.EquipWeapon(UnityEngine.Resources.Load<WeaponConfig>("Bow"));
            parameter.fighter.Fight(parameter.player);
            parameter.animator.Play("Attack");
            manager.TransitionState(StateType.WaitSwordState);
        }
        public void OnUpdate()
                {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
        }
        public void OnExit()
        {
        }

       
    }

    public class WaitBowState : IState
    {
        private FSM manager;
        private Parameter parameter;
        private float waitTime;
        private float MaxTime=4f;

        public WaitBowState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {
            waitTime=0;
        }

        public void OnExit()
        {
            
        }

        public void OnUpdate()
        {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
            waitTime +=Time.deltaTime;
            if(waitTime>=2f) parameter.fighter.Cancel();
            if (waitTime>=MaxTime)manager.TransitionState(StateType.BowAttackState);
        }
    }

    public class WaitSwordState : IState
    {
        private FSM manager;
        private Parameter parameter;
        private float waitTime;
        private float MaxTime = 4f;

        public WaitSwordState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {

            waitTime=0;
        }

        public void OnExit()
        {

        }

        public void OnUpdate()
        {
            if (parameter.health.IsDead()) manager.TransitionState(StateType.DeadState);
            waitTime += Time.deltaTime;
            if (waitTime >= 2f) parameter.fighter.Cancel();
            if (waitTime >= MaxTime) manager.TransitionState(StateType.Chase);
        }
    }

    public class DeadState : IState
    {
        private FSM manager;
        private Parameter parameter;
        

        public DeadState(FSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }

        public void OnEnter()
        {


        }

        public void OnExit()
        {

        }

        public void OnUpdate()
        {
          
        }
    }
}