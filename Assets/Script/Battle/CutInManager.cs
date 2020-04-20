using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutInManager : MonoBehaviour
{
    public SpriteRenderer thumbnailImage;
    public GameObject maskImage;

    public IEnumerator LoadThumbNailAndPlay(string imagePath)
    {
        maskImage.SetActive(true);
        Color color = thumbnailImage.material.color;

        var sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + imagePath);

        thumbnailImage.sprite = sprite;

        float time = 0.3f;
        float elapsedTime = 0.0f;

        while(elapsedTime < time)
        {
            var t = elapsedTime / time;

            color.a = t;
            thumbnailImage.material.color = color;

            t = 5.0f - (4.0f * t);
            maskImage.transform.localScale = new Vector3(t, t, 1.0f);

            elapsedTime += Time.deltaTime;
            thumbnailImage.material.color = color;
            yield return null;
        }

        maskImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        yield return new WaitForSeconds(0.2f);

        color.a = 0;
        maskImage.SetActive(false);
        maskImage.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
        thumbnailImage.material.color = color;
    }
}