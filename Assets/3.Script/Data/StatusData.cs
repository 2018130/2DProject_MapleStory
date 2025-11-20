using System;
using UnityEngine;
public enum StatusType
{
    None,
    Speed,
    JumpForce,
    ExpRate,
    Atk,
    MaxHP,
    MaxMP,
    STR,
    DEX,
    INT,
    LUK,
}

[Serializable]
public class StatusData
{
    [SerializeField]
    private string key;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;

    // »ç¸Á ½Ã È¹µæ °æÇèÄ¡ ·®
    [SerializeField]
    private int expAmount;

    // °æÇèÄ¡ ¹èÀ²
    [SerializeField]
    private float expRate;

    [SerializeField]
    private int atk;
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int maxMP;
    [SerializeField]
    private int str;
    [SerializeField]
    private int dex;
    [SerializeField]
    private int integer;
    [SerializeField]
    private int luk;

    #region properties
    public string Key
    {
        get => key;
    }

    public int ATK
    {
        get => atk;
        set => atk = value;
    }

    public int MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }

    public int MaxMP
    {
        get => maxMP;
        set => maxMP = value;
    }

    public int STR
    {
        get => str;
        set => str = value;
    }

    public int DEX
    {
        get => dex;
        set => dex = value;
    }

    public int INT
    {
        get => integer;
        set => integer = value;
    }

    public int LUK
    {
        get => luk;
        set => luk = value;
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }

    public float EXPRate
    {
        get => expRate;
        set => expRate = value;
    }
    #endregion

    public int GetEXPAmount()
    {
        return expAmount;
    }

    public StatusData(bool isCharacterStat)
    {
        if(isCharacterStat)
        {
            this.atk = 3;
            this.str = 4;
            this.dex = 4;
            this.integer = 4;
            this.luk = 4;
            this.moveSpeed = 1f;
            this.jumpForce = 4.5f;
            expAmount = 10;
        }
        else
        {
            this.atk = 0;
            this.str = 0;
            this.dex = 0;
            this.integer = 0;
            this.luk = 0;
            this.moveSpeed = 0;
            this.jumpForce = 0;
            expAmount = 0;
        }

        key = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }

    public StatusData(int str, int dex, int integer, int luk)
    {
        this.atk = 3;
        this.str = str;
        this.dex = dex;
        this.integer = integer;
        this.luk = luk;
        this.moveSpeed = 1f;
        this.jumpForce = 4.5f;
        expAmount = 10;

        key = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }

    public void SetRandomStatus()
    {
        float acc = 0f;
        float rand = UnityEngine.Random.Range(0f, 1f);
        str = 4;
        while (acc < rand)
        {
            acc += UnityEngine.Random.Range(0f, 1f);
            str++;
        }

        acc = 0f;
        rand = UnityEngine.Random.Range(0f, 1f);
        dex = 4;
        while (acc < rand)
        {
            acc += UnityEngine.Random.Range(0f, 1f);
            dex++;
        }

        acc = 0f;
        rand = UnityEngine.Random.Range(0f, 1f);
        integer = 4;
        while (acc < rand)
        {
            acc += UnityEngine.Random.Range(0f, 1f);
            integer++;
        }

        acc = 0f;
        rand = UnityEngine.Random.Range(0f, 1f);
        luk = 4;
        while (acc < rand)
        {
            acc += UnityEngine.Random.Range(0f, 1f);
            luk++;
        }
    }
}
