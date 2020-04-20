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
        //vp.url = "Assets/Resources/CharEffect/Hitomi/hitomi1.mp4";
        vp.url = "Assets/Resources/CharEffect/" + VideoPath + ".mp4";

        if (vp.url == null)
        {
            vp.url = "Assets/Resources/CharEffect/default.mp4";
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
