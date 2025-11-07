using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private float overDarkColorRate = 1f;

    private void OnMouseOver()
    {
        image.color = new Color(image.color.r * overDarkColorRate,
                                image.color.g * overDarkColorRate,
                                image.color.b * overDarkColorRate);
    }
}
