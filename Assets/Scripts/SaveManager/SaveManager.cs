using System;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{

    private SaveSetup save;

    public override void Awake() {
        base.Awake();
        save = new SaveSetup();
    }

    #region Save

    [NaughtyAttributes.Button]
    public void Save(){
        string saveToJson = JsonUtility.ToJson(save, true);
        Debug.Log(saveToJson);
        SaveFile(saveToJson);
    }

    public void SaveLastLevel(int level){
        save.lastLevel = level;
        Save();
    }

    private void SaveFile(string json){
        string path = Application.persistentDataPath + "/save.txt";
        Debug.Log(path);
        File.WriteAllText(path, json);
    }

    [NaughtyAttributes.Button]
    private void SaveLevelOne(){
        SaveLastLevel(1);
    }
    [NaughtyAttributes.Button]
    private void SaveLevelFive(){
        SaveLastLevel(5);
    }

    #endregion

}

[System.Serializable]
public class SaveSetup{
    public int lastLevel;
}

