using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemIcon : MonoBehaviour
{
    public Image iconImage;
    public SpriteRenderer iconSprite;

    public void StartAnimation()
    {
        StartCoroutine(UpCoroutine());
    }

    public IEnumerator UpCoroutine()
    {
        float time = 0.2f;
        float elapsedTime = 0f;

        var res = transform.position;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;
            transform.position = new Vector3(res.x, res.y + (t*5) + 1, res.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(DownCoroutine());
    }

    public IEnumerator DownCoroutine()
    {
        float time = 0.2f;
        float elapsedTime = 0f;

        var res = transform.position;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;
            transform.position = new Vector3(res.x, res.y - (t * 5) + 1, res.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
