using System;
using UnityEngine;

[Serializable]
public class PlayerCharacterData
{
    [SerializeField]
    // 키 중복검사는 하지 않음
    private string key;
    public string Key => key;

    [SerializeField]
    public string characterName;

    [SerializeField]
    public StatusData statusData;

    public PlayerCharacterData(string name, StatusData statusData)
    {
        key = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        characterName = name;
        this.statusData = statusData;
    }
}
