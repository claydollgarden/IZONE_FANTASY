using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GachaManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;
    public GachaDialog gachaDialog;

    int _gachaNumber;
    int _gachaPrice;

    // Start is called before the first frame update
    void Start()
    {
        _gachaNumber = 0;
        _gachaPrice = 0;

        StartCoroutine(FadeOut());
    }

    public void ConfirmButtonClicked()
    {
        gachaDialog.SetActive(false);
    }

    public void CancelButtonClicked()
    {
        gachaDialog.SetActive(false);
    }

    public void BackButtonClicked()
    {
        StartCoroutine(FadeIn("MainScene"));
    }

    public void GachaButtonClicked(int gachaNumber)
    {
        _gachaNumber = gachaNumber;

        if (_gachaNumber == 1)
        {
            _gachaPrice = 300;
        }
        else if(_gachaNumber == 10)
        {
            _gachaPrice = 3000;
        }

        gachaDialog.SetDialog(_gachaNumber, _gachaPrice);
    }

    public IEnumerator FadeOut()
    {
        Color color = fullScreenImage.color;

        float time = 0.5f;
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;

            color.a = 1.0f - t;

            elapsedTime += Time.deltaTime;

            fullScreenImage.color = color;

            yield return null;
        }

        color.a = 0.0f;
        fullScreenImage.color = color;
    }

    public IEnumerator FadeIn(string changescene = "")
    {
        Color color = fullScreenImage.color;

        float time = 0.5f;
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;

            color.a = t;

            elapsedTime += Time.deltaTime;

            fullScreenImage.color = color;

            yield return null;
        }

        color.a = 1.0f;
        fullScreenImage.color = color;

        if (changescene != "")
        {
            SceneManager.LoadScene(changescene);
        }
    }
}
