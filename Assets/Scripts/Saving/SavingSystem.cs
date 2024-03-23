using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(String saveFile)
        {
            Dictionary<String,object> state=LoadFile(saveFile);
            int buildIndex= SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("LastSceneIndex"))
            {
                if (buildIndex!= (int)state["LastSceneIndex"])
                {
                    buildIndex= (int)state["LastSceneIndex"];
                }   
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            restoreState(state);
        }

        public void Save(String saveFile)
        {
            Dictionary<string,object>state=LoadFile(saveFile);
            CaptrueState(state);
            SaveFile(saveFile,state);
        }

        private void SaveFile(string saveFile, Dictionary<string, object> dictionary)
        {
            String savePath = GetPathFromSaveFile(saveFile);
            using (FileStream stream = File.Create(savePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream,dictionary);
            }
        }

        public void Load(String saveFile)
        {
            restoreState(LoadFile(saveFile));
        }

        private Dictionary<string,object> LoadFile(String saveFile)
        {
            String savePath = GetPathFromSaveFile(saveFile);
            if(!File.Exists(savePath))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.OpenRead(savePath))
            {
                BinaryFormatter binaryFormatter=new BinaryFormatter();
                return (Dictionary<string, object>)binaryFormatter.Deserialize(stream);
            }
        }

        private void  CaptrueState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveableEntity in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptrueState();
            }
            int SceneIndex=SceneManager.GetActiveScene().buildIndex;
            state["LastSceneIndex"]=SceneIndex;
        }

        private void restoreState(Dictionary<string,object> state)
        {
            Dictionary<String, object> stateDic = (Dictionary<String,object>)state;
            foreach(SaveableEntity saveableEntity in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
            {
                string id=saveableEntity.GetUniqueIdentifier();
                if(state.ContainsKey(id))
                    saveableEntity.RestoreState(stateDic[id]);
            }
        }

        public void Delete(String saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        
        
        String GetPathFromSaveFile(String saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }


    }

}

