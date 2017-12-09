using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour{

    public static SoundController Instance { get; protected set; }
    Dictionary<string, AudioClip> SoundsMap;

    float soundCooldown;

    public AudioSource ambientSource, fxSource;

    public float soundVolume = 0.6f;

    private void Awake()
    {
        if (Instance != null)
            Instance = null;

        Instance = this;
        //ambientSource = GetComponent<AudioSource>();
        InitSoundMap();
        ambientSource.mute = true;
        
    }
    public void PlayAmbient(){
        ambientSource.mute = false;
    }
    public void MuteAmbient(){
        ambientSource.mute = true;
    }

    void InitSoundMap()
    {
        SoundsMap = new Dictionary<string, AudioClip>();
        AudioClip[] sounds = Resources.LoadAll<AudioClip>("Sounds/");
        for (int i = 0; i < sounds.Length; i++)
        {
            SoundsMap.Add(sounds[i].name, sounds[i]);
        }
    }


    public void PlaySound(string id)
    {
        if (SoundsMap.ContainsKey(id) && fxSource.isPlaying == false)
        {
            fxSource.PlayOneShot(SoundsMap[id], soundVolume);
        }
    }

}
