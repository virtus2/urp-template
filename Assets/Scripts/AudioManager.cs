using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource bgm;
    public AudioSource sfxCoin;
    public AudioSource sfxDie;
    public AudioSource sfxEat;
}
