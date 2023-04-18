using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceBG, audioSourceSFX;

    public AudioClip bgMusic, btns, cash, buildignsSound, danger, warning;

    [SerializeField]
    public enum effects
    {
        btn, cash, danger, warning, building
    }


    public void playBG(bool b)
    {
        if (b)
        {
            audioSourceBG.clip = bgMusic;
            audioSourceBG.Play();
        }
        else
        {
            if (audioSourceBG.isPlaying)
            {
                audioSourceBG.Stop();
            }
        }
    }

    public void playEffect(string effect)
    {
        if (effect == effects.btn.ToString())
        {
            audioSourceSFX.clip = btns;
        }
        else if (effect == effects.cash.ToString())
        {
            audioSourceSFX.clip = cash;
        }
        else if (effect == effects.danger.ToString())
        {
            audioSourceSFX.clip = danger;
        }
        else if (effect == effects.warning.ToString())
        {
            audioSourceSFX.clip = warning;
        }
        else if (effect == effects.building.ToString())
        {
            audioSourceSFX.clip = buildignsSound;
        }
        audioSourceSFX.Play();
    }

    public void MuteBG(bool b)
    {
        audioSourceSFX.mute = b;
    }

    public void MuteSFX(bool b)
    {
        audioSourceBG.mute = b;
    }

}
