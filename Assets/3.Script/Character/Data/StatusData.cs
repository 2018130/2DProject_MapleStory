using System;
using UnityEngine;

[Serializable]
public class StatusData
{
    private int str;
    private int dex;
    private int integer;
    private int luk;

    public int STR => str;
    public int DEX => dex;
    public int INT => integer;
    public int LUK => luk;

    public StatusData() { }

    public StatusData(int str, int dex, int integer, int luk)
    {
        this.str = str;
        this.dex = dex;
        this.integer = integer;
        this.luk = luk;
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
