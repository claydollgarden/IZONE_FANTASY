using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public AudioSource efxSource;

    override protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayOneShot(string soundPath)
    {
        var audioObject = VResourceLoad.Load<AudioClip>("Sound/EffectSound/" + soundPath);

        efxSource.PlayOneShot(audioObject);
    }

    public void PlaySingle(string soundPath)
    {
        var audioObject = VResourceLoad.Load<AudioClip>("Sound/BGM/" + soundPath);

        efxSource.clip = audioObject;
        efxSource.loop = true;
        efxSource.Play();
    }

    public void StopAndPlay(AudioClip clip)
    {
        efxSource.Stop();

        efxSource.clip = clip;

        efxSource.Play();
    }
}
