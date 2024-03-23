using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        
       [SerializeField] string uniqueIdentifier="" ;
        static Dictionary<String,SaveableEntity> globalDic=new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptrueState()
        {
            Dictionary<string,object> state=new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()]=saveable.CaptrueState();
            }
            return state;
            // SerializeVector3 vector3=new SerializeVector3(gameObject.transform.position);
            // return vector3;
        }

        public void RestoreState(object state)
        {
            Dictionary<string,object> stateDic=(Dictionary<string,object>)state;
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                string type=saveable.GetType().ToString();
                if(stateDic.ContainsKey(type))
                {
                    saveable.RestoreState(stateDic[type]);
                }
            }
            // SerializeVector3 vector3=(SerializeVector3)state;
            // GetComponent<NavMeshAgent>().enabled=false;
            // gameObject.transform.position=vector3.ToVector();
            // GetComponent<NavMeshAgent>().enabled=true;
            // GetComponent<ActionSchedule>().StopLastAction();
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue)||!IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            globalDic[property.stringValue]=this;
        }
#endif

        private bool IsUnique(string stringValue)
        {
            if(!globalDic.ContainsKey(stringValue))return true;
            if(globalDic[stringValue]==this)return true;
            if(globalDic[stringValue]==null){
                globalDic.Remove(stringValue);
                return true;
            }
            if(globalDic[stringValue].GetUniqueIdentifier()!=stringValue)
            {
                globalDic.Remove(stringValue);
                return true;
            }
           return false;
        }
    }
}


