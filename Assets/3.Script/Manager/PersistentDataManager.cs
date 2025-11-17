using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerCharacterDataJson
{
    [SerializeField]
    public List<PlayerCharacterData> data = new List<PlayerCharacterData>();
}

public class PersistentDataManager : SingletonBehaviour<PersistentDataManager>
{
    private string dataPath;

    private string playerCharacterDataFileName = "playerCharacter.json";
    private PlayerCharacterDataJson playerCharacterDataJson = new PlayerCharacterDataJson();

    private void Start()
    {
        dataPath = Application.persistentDataPath;
    }

    public void SaveToJson(PlayerCharacterDataJson playerCharacterDataJson)
    {
        string jsonData = JsonUtility.ToJson(playerCharacterDataJson, true);
        File.WriteAllText(Path.Combine(dataPath, playerCharacterDataFileName), jsonData);
        Debug.Log($"Save data to json path : {dataPath}");
    }

    public PlayerCharacterDataJson LoadFromJson()
    {
        string jsonData = File.ReadAllText(Path.Combine(dataPath, playerCharacterDataFileName));

        if(jsonData.Length == 0)
        {
            playerCharacterDataJson = new PlayerCharacterDataJson();
        }
        else
        {
            playerCharacterDataJson = JsonUtility.FromJson<PlayerCharacterDataJson>(jsonData);
        }

        return playerCharacterDataJson;
    }

}
