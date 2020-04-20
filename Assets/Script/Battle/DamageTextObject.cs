using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextObject : MonoBehaviour
{
    public Sprite[] DamageNumber;
    public DamageText[] damageText;

    private void Start()
    {
        HideText();
    }
    public void StartDamageTextAnimation(int damage, Vector3 position)
    {
        StartCoroutine(SetNumber(damage, position));
    }

    public IEnumerator SetNumber(int DamageCount, Vector3 targetPosition)
    {
        Debug.Log("targetPosition : " + targetPosition);
        Debug.Log("transform.position : " + transform.position);
        transform.position = targetPosition;

        int number = DamageCount;
        for (int i = 0; i < DamageCount.ToString().Length; i++)
        {
            Debug.Log("damageText");
            damageText[i].damageText.enabled = true;
            damageText[i].damageText.sprite = DamageNumber[number % 10];

            number = number / 10;
        }

        float time = 1.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;

            t = 0.05f * t;

            transform.position = new Vector3(transform.position.x, transform.position.y + t, 25.0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        HideText();
    }

    public void HideText()
    {
        Debug.Log("HideText");
        for (int i = 0; i < damageText.Length; i++)
        {
            damageText[i].damageText.enabled = false;
        }
    }
}
