using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioSource musicAdSource, soundAdSource;
    [SerializeField] AudioClip cardflip, matchClip, misMatchClip, gameOverClip;

    public bool soundOn {  get; private set; }
    public bool musicOn { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void SetMusicOnOff(bool b)
    {
        musicOn = b;
        if (b)
        {
            musicAdSource.Play();
        }
        else
        {
            musicAdSource.Pause();
        }

    }

    public void SetSoundOnOff(bool b)
    {
        soundOn = b;
        
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundOn)
        {
            switch(soundType)
            {
                case SoundType.cardFlip:
                    soundAdSource.PlayOneShot(cardflip);
                    break;
                case SoundType.match:
                    soundAdSource.PlayOneShot(matchClip);
                    break;
                case SoundType.misMatch:
                    soundAdSource.PlayOneShot(misMatchClip);
                    break;
                case SoundType.gameOver:
                    soundAdSource.PlayOneShot(gameOverClip);
                    break;
            }
        }
    }
}


public enum SoundType
{
    match, misMatch, gameOver, cardFlip
}
