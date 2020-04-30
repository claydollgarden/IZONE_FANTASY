using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer vp;

    public void Play(string VideoPath)
    {
        vp.targetCameraAlpha = 1.0f;
        vp.clip = VResourceLoad.Load<VideoClip>("CharEffect/" + VideoPath);
        //vp.clip = "Assets/Resources/CharEffect/" + VideoPath + ".mp4";

        if (vp.url == null)
        {
            vp.clip = VResourceLoad.Load<VideoClip>("CharEffect/" + "default");
            //vp.url = "Assets/Resources/CharEffect/default.mp4";
        }

        vp.Play();
    }

    public void pause()
    {
        vp.Pause();
    }

    public float GetCurrentVideoTime()
    {
        float currentTime = (float)vp.length;

        return currentTime;
    }

    public void Stop()
    {
        vp.Stop();
        vp.targetCameraAlpha = 0.0f;
    }
}
