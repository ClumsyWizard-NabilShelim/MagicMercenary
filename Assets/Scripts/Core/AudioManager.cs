using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : PersistantSingleton<AudioManager>
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound backgroundMusic;

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.loop = sound.Loop;
            sound.Source.volume = sound.Volume;
            sound.Source.playOnAwake = false;
        }

        AudioSource backgroundSource = gameObject.AddComponent<AudioSource>();
        backgroundMusic.Source = backgroundSource;
        backgroundSource.clip = backgroundMusic.Clip;
        backgroundSource.loop = backgroundMusic.Loop;
        backgroundSource.volume = backgroundMusic.Volume;

        backgroundSource.Play();
    }

    public void PlayAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if(!s.Source.isPlaying)
            s.Source.Play();
    }

    public void StopAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if(s.Source.clip != null)
            s.Source.Stop();
    }
}
