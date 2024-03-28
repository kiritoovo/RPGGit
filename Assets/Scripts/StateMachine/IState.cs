using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine
{
    public interface IState{
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }    
}