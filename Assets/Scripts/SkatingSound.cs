using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkatingSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip skating1;
    [SerializeField]
    private AudioClip skating2;

    private AudioSource source;
    private static SkatingSound instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        source = GetComponent<AudioSource>();
    }

    public static SkatingSound Instance()
    {
        return instance;
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            if (Random.Range(0f, 1f) > 0.5f)
                source.clip = skating1;
            else
                source.clip = skating2;

            source.Play();
        }
    }

    public void TurnOnSound(bool on)
    {
        source.enabled = on;
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
    }
}
