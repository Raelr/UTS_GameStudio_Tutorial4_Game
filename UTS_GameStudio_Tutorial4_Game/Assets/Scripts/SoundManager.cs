using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource playerSounds;

    [SerializeField]
    AudioSource backgroundMusic;

    [SerializeField]
    AudioSource singleSounds;

    [SerializeField]
    AudioClip backgroundClip;

    public static SoundManager instance;

    private void Awake() {

        instance = this;
    }

    void Start()
    {

        backgroundMusic.clip = backgroundClip;
        backgroundMusic.Play();
    }
    
    
}
