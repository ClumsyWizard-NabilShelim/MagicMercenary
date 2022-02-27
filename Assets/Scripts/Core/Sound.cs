using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound 
{
    public string Name;
    public AudioClip Clip;

    public bool Loop;

    [Range(0.0f, 1.0f)]
    public float Volume;

    [HideInInspector]
    public AudioSource Source;
}
