using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonBehaviour<SoundManager>
{
    private AudioSource audioSource;
    private List<AudioClip> audioBuffer;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        audioBuffer = new List<AudioClip>();
    }

    public void PlaySound(AudioClip clip)
    {

    }
}
