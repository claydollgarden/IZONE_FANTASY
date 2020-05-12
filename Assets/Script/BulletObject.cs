using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public void StartBulletCoroutine(Vector3 end, bool playerSide)
    {
        float margin;
        if (playerSide)
        {
            margin = -2.0f;
        }
        else
        {
            margin = 2.0f;
        }

        var endPosition = new Vector3(end.x + margin, end.y, end.z);

        StartCoroutine(BulletCoroutine(endPosition));
    }
    public IEnumerator BulletCoroutine(Vector3 end)
    {
        float time = 0.4f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;
            var res = Vector3.Lerp(gameObject.transform.position, end, t);
            res.y += Mathf.Sin(t * Mathf.PI);
            transform.position = res;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
