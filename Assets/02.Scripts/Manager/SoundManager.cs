﻿using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip[] sfxClips;

    [Header("UI Sliders")]
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    private float sfxVolume;
    private float bgmVolume;

    protected override void Awake()
    {
        base.Awake();
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();

        sfxSource = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = bgmSource.volume;
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxSource.volume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
            sfxVolume = Mathf.Clamp01(volume);
            sfxSource.volume = volume;
    }

    public void PlaySFX(int clipIndex)
    {
        if (sfxSource != null && sfxClips != null)
        {
            if (clipIndex >= 0 && clipIndex < sfxClips.Length)
            {
                sfxSource.PlayOneShot(sfxClips[clipIndex]);
            }
        }
    }

    public void Init()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void ChangeBgm(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}