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

[Serializable]
public class SkillDataJson
{
    [SerializeField]
    public List<ActiveSkillData> activeSkillList = new List<ActiveSkillData>();
    [SerializeField]
    public List<BaseSkillData> passiveSkillList = new List<BaseSkillData>();
}

public class PersistentDataManager : SingletonBehaviour<PersistentDataManager>
{
    private string dataPath;

    private string playerCharacterDataFileName = "playerCharacter.json";
    private PlayerCharacterDataJson playerCharacterDataJson = new PlayerCharacterDataJson();

    private string skillDataFileName = "skill.json";
    private SkillDataJson skillDataJson = new SkillDataJson();

    private void Start()
    {
        dataPath = Application.persistentDataPath;
    }

    public void SaveToJson(PlayerCharacterDataJson playerCharacterDataJson)
    {
        string jsonData = JsonUtility.ToJson(playerCharacterDataJson, true);
        File.WriteAllText(Path.Combine(dataPath, playerCharacterDataFileName), jsonData);
        //Debug.Log($"Save data to json path : {dataPath}");
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

    public void SaveToJson(SkillDataJson skillDataJson)
    {
        string jsonData = JsonUtility.ToJson(skillDataJson, true);
        File.WriteAllText(Path.Combine(dataPath, skillDataFileName), jsonData);
        //Debug.Log($"Save data to json path : {dataPath}");
    }
    public SkillDataJson LoadSkillDataFromJson()
    {
        string path = Path.Combine(dataPath, skillDataFileName);

        if(File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return skillDataJson = JsonUtility.FromJson<SkillDataJson>(jsonData);
        }

        return null;
    }
}
