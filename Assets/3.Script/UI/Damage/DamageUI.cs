using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    private GameObject contents;

    private List<Sprite> numberSprites;
    public List<Sprite> NumberSprites
    {
        get => numberSprites;
        set => numberSprites = value;
    }


    public void PrintDamage(int damage)
    {
        float posX = 0;
        float size = 40;
        float maxPosY = 5;
        List<int> damageDigit = new List<int>();

        while(damage % 10 != 0)
        {
            int digitNum = damage % 10;
            damage /= 10;

            damageDigit.Add(digitNum);
        }

        damageDigit.Reverse();

        foreach (var digit in damageDigit)
        {
            GameObject gameObject = new GameObject("", typeof(RectTransform));
            RectTransform spawnedObjRect = gameObject.GetComponent<RectTransform>();
            spawnedObjRect.sizeDelta = new Vector2(size, size);

            gameObject.AddComponent<Image>().sprite = numberSprites[digit];
            gameObject.transform.SetParent(contents.transform);
            spawnedObjRect.transform.localPosition = new Vector2(posX, UnityEngine.Random.Range(0, maxPosY));


            posX += spawnedObjRect.rect.width;
        }
        StartCoroutine(DamageLife_co());
    }

    private IEnumerator DamageLife_co()
    {
        float speed = GameManager.Instance.CurrentSceneContext.MainUIManager.DamageUIController.DamageUISpeed;
        float lifeTime = GameManager.Instance.CurrentSceneContext.MainUIManager.DamageUIController.DamageUILifeTime;
        float timer = 0f;

        while(timer < lifeTime)
        {
            yield return null;
            // »ö º¯°æ
            float delta = Time.deltaTime * GameManager.Instance.CurrentSceneContext.GameDeltaTime;
            timer += delta;

            transform.localPosition += new Vector3(0, speed * delta);
            SetAlpha(Mathf.Lerp(1, 0, timer / lifeTime));
        }
        SetAlpha(0);
        Destroy(gameObject);
    }
    private void SetAlpha(float value)
    {
        for(int i = 0; i < contents.transform.childCount; i++)
        {
            Image numberUIImage = contents.transform.GetChild(i).GetComponent<Image>();
            numberUIImage.color = new Color(numberUIImage.color.r, numberUIImage.color.g, numberUIImage.color.b, value);
        }
    }
}
