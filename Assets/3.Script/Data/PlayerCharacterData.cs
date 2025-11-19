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
    public string CharacterName;

    // 플레이어 현재 경험치 량
    [SerializeField]
    public int EXP;

    [SerializeField]
    public StatusData statusData;

    public PlayerCharacterData(string name, StatusData statusData)
    {
        key = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        CharacterName = name;
        this.statusData = statusData;
    }

    public int GetLevel()
    {
        int basicExp = 50;
        float acc = 1.2f;
        int lv = 1;

        while(basicExp <= EXP)
        {
            basicExp += (int)(acc * basicExp);
            lv++;
        }

        return lv;
    }

    public int GetTotalRequiredExpForNextLevel()
    {
        int preExp = 0;
        int basicExp = 50;
        float acc = 1.2f;
        int lv = 1;

        while (basicExp <= EXP)
        {
            preExp = basicExp;
            basicExp += (int)(acc * basicExp);
            lv++;
        }

        return basicExp - preExp;
    }

    /// <summary>
    /// 레벨 업 직후 남아있는 경험치량 리턴
    /// </summary>
    /// <returns></returns>
    public int GetRemainExp()
    {
        int preBasicExp = 0;
        int basicExp = 50;
        float acc = 1.2f;
        int lv = 1;

        while (basicExp <= EXP)
        {
            preBasicExp = basicExp;
            basicExp += (int)(acc * basicExp);
            lv++;
        }

        return EXP - preBasicExp < 0 ? 0 : EXP - preBasicExp;
    }
}
