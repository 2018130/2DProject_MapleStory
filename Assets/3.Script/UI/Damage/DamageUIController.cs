using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DamageUIController : MonoBehaviour
{
    [SerializeField]
    private float damageUISpeed;
    [SerializeField]
    private float damageUILifeTime;
    [SerializeField]
    private string damageUIKey;
    [SerializeField]
    private string damageSkinKey;
    [SerializeField]
    private Vector2 damageUIoffset;

    private GameObject damageUIPrefab;
    private static List<Sprite> numberSprites = new List<Sprite>();

    public float DamageUISpeed => damageUISpeed;
    public float DamageUILifeTime => damageUILifeTime;

    private void Awake()
    {
        if (numberSprites.Count != 10)
        {
            for (int i = 0; i < 10; i++)
            {
                Sprite number = Addressables.LoadAssetAsync<Sprite>(damageSkinKey + i.ToString()).WaitForCompletion();
                numberSprites.Add(number);
            }
        }

        damageUIPrefab = Addressables.LoadAssetAsync<GameObject>(damageUIKey).WaitForCompletion();
    }

    public void CreateDamageUI(int damage, Vector2 spawnPosition)
    {
        DamageUI spawnedDamageUI = Instantiate(damageUIPrefab, spawnPosition + damageUIoffset, Quaternion.identity, transform).GetComponent<DamageUI>();
        spawnedDamageUI.NumberSprites = numberSprites;
        spawnedDamageUI.PrintDamage(damage);
    }
}
