using System;
using UnityEngine;

[Serializable]
public class StatusData
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private int str;
    [SerializeField]
    private int dex;
    [SerializeField]
    private int integer;
    [SerializeField]
    private int luk;

    public int STR => str;
    public int DEX => dex;
    public int INT => integer;
    public int LUK => luk;
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;

    public StatusData() { }

    public StatusData(int str, int dex, int integer, int luk)
    {
        this.str = str;
        this.dex = dex;
        this.integer = integer;
        this.luk = luk;
        this.moveSpeed = 1f;
        this.jumpForce = 4.5f;
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
