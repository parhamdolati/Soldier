using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource SoundeffectAudio, MusicAudio;
    public AudioClip ClickClip,TrueClip,FalseClip;
    public AudioClip Zoom;
    public AudioClip Star;
    public AudioClip gameStart;

    public void Click()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = ClickClip;
            SoundeffectAudio.Play();
        }
    }

    public void True()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = TrueClip;
            SoundeffectAudio.Play();
        }
    }
    
    public void False()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = FalseClip;
            SoundeffectAudio.Play();
            if (PlayerPrefs.GetInt("Vibrate") == 1)
                Vibration.Vibrate(40);
        }
    }

    public void MusicOnOff()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
            MusicAudio.Stop();
        else
        {
            MusicAudio.Play();
        }
    }

    public void MenuZoom()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = Zoom;
            SoundeffectAudio.Play();
        }
    }

    public void GetStar()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = Star;
            SoundeffectAudio.Play();
        }
    }

    public void GameStart()
    {
        if (PlayerPrefs.GetInt("SoundEffect") == 1)
        {
            SoundeffectAudio.clip = gameStart;
            SoundeffectAudio.Play();
        }
    }
}
