using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] Toggle tgM, tgS;

    private void OnEnable()
    {
        tgM.onValueChanged.AddListener(SetMusicOnOff);
        tgS.onValueChanged.AddListener(SetSoundOnOff);
        tgM.SetIsOnWithoutNotify( SoundManager.instance.musicOn);
        tgS.SetIsOnWithoutNotify( SoundManager.instance.soundOn);

    }

    private void OnDisable()
    {
        tgM.onValueChanged.RemoveAllListeners();
        tgS.onValueChanged.RemoveAllListeners();
    }


    void SetMusicOnOff(bool b)
    {
        SoundManager.instance.SetMusicOnOff(b);
        GameDataLoader.Instance.SetMusic(b);
    }

    void SetSoundOnOff(bool b)
    {
        SoundManager.instance.SetSoundOnOff(b);
        GameDataLoader.Instance.SetSound(b);
    }
}
