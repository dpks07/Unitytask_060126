using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    public static GameDataLoader Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void Start()
    {
        LoadLevelValue();
        CheckForSound();
        CheckForMusic();
    }

    void CheckForSound()
    {
        if (PlayerPrefs.HasKey(ConstantKeys.SOUND))
        {
            if (PlayerPrefs.GetInt(ConstantKeys.SOUND) == 0)
            {
                // Set SOund Off
                SoundManager.instance.SetSoundOnOff(false);
            }
            else
            {
                SoundManager.instance.SetSoundOnOff(true);
                // Set Sound On
            }
        }
        else
        {
            SoundManager.instance.SetSoundOnOff(true);
            PlayerPrefs.SetInt(ConstantKeys.SOUND, 1);
            // Set Sound ON
        }
    }

    void CheckForMusic()
    {
        if (PlayerPrefs.HasKey(ConstantKeys.MUSIC))
        {
            if (PlayerPrefs.GetInt(ConstantKeys.MUSIC) == 0)
            {
                // Set SOund Off
                SoundManager.instance.SetMusicOnOff(false);
            }
            else
            {
                SoundManager.instance.SetMusicOnOff(true);
                // Set Sound On
            }
        }
        else
        {
            SoundManager.instance.SetMusicOnOff(true);
            PlayerPrefs.SetInt(ConstantKeys.MUSIC, 1);
            // Set Sound ON
        }
    }

    public void SetMusic(bool b)
    {
        PlayerPrefs.SetInt(ConstantKeys.MUSIC, b ? 1 : 0);

    }

    public void SetSound(bool b)
    {
        PlayerPrefs.SetInt(ConstantKeys.SOUND, b ? 1 : 0);

    }

    public void SetHighMatch(int matches)
    {
        PlayerPrefs.SetInt(ConstantKeys.HIGHESTMATCH, Mathf.Max(PlayerPrefs.GetInt(ConstantKeys.HIGHESTMATCH), matches));
    }

    public void SetBestTurn(int turns)
    {
        if (PlayerPrefs.GetInt(ConstantKeys.MINIMUMTURNS) == 0)
        {
            PlayerPrefs.SetInt(ConstantKeys.MINIMUMTURNS, turns);
        }
        else
        {
            PlayerPrefs.SetInt(ConstantKeys.MINIMUMTURNS, Mathf.Min(PlayerPrefs.GetInt(ConstantKeys.MINIMUMTURNS), turns));
        }
    }

    public int GetHighMatch()
    {
        return PlayerPrefs.GetInt(ConstantKeys.HIGHESTMATCH, 0);
    }

    public int GetBestTurn()
    {
        return PlayerPrefs.GetInt(ConstantKeys.MINIMUMTURNS, 0);
    }

    void LoadLevelValue()
    {
        int level = 1;
        if (PlayerPrefs.HasKey(ConstantKeys.LEVEL))
        {
            level = PlayerPrefs.GetInt(ConstantKeys.LEVEL);
        }
        else
        {
            PlayerPrefs.SetInt(ConstantKeys.LEVEL, level);
        }
        GameManager.instance.SetToggleOnStart(level);

    }

    public void SetLevelValue(int level)
    {
        PlayerPrefs.SetInt(ConstantKeys.LEVEL, level);
    }
}
