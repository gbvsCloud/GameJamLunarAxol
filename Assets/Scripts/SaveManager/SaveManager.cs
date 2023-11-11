using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void Save(){
        SaveSetup save = new SaveSetup();
        save.lastLevel = 1;

        string saveToJson = JsonUtility.ToJson(save);
        Debug.Log(saveToJson);
    }
}

[System.Serializable]
public class SaveSetup{
    public int lastLevel;
}

