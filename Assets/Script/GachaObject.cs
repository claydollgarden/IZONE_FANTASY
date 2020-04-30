using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaObject : MonoBehaviour
{
    public Image charThumbnail;

    public void ResetObject()
    {
        charThumbnail.rectTransform.localPosition = new Vector3(300.0f, 0.0f, 0.0f);
    }

    public void MoveCard()
    {
        StartCoroutine(MoveCardCourutine());
    }

    public IEnumerator MoveCardCourutine()
    {
        float time = 0.2f;
        float elapsedTime = 0.0f;

        var res = charThumbnail.rectTransform.localPosition;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;

            charThumbnail.rectTransform.localPosition = new Vector3(res.x - (t * 300), 0.0f, 0.0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        charThumbnail.rectTransform.localPosition = new Vector3( 0.0f, 0.0f, 0.0f);
    }
}
