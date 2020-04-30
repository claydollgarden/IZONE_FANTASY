using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GachaManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;
    public GachaDialog gachaDialog;

    public GameObject gachaPanel;
    public GameObject gacha1Panel;

    public GachaObject[] gachaObjects;
    public GachaObject gacha1Object;

    public Text currentMoney;

    string gachaName;

    int _gachaNumber;
    int _gachaPrice;
    int _gachaCategory;

    // Start is called before the first frame update
    void Start()
    {
        _gachaNumber = 0;
        _gachaPrice = 0;
        _gachaCategory = 0;

        currentMoney.text = GameManager.Instance.userData.currentMoney.ToString();

        StartCoroutine(FadeOut());
    }

    public void GachaConfirmClicked()
    {
        gacha1Panel.SetActive(false);
        gachaPanel.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            gachaObjects[i].ResetObject();
        }

        gacha1Object.ResetObject();
    }

    public void ConfirmButtonClicked()
    {
        gachaDialog.SetActive(false);
        GameManager.Instance.userData.currentMoney = GameManager.Instance.userData.currentMoney - _gachaPrice;

        currentMoney.text = GameManager.Instance.userData.currentMoney.ToString();

        if(_gachaNumber == 1)
        {
            gacha1Panel.SetActive(true);
            StartCoroutine(Gacha1EffectCoroutine());
        }
        else
        {
            gachaPanel.SetActive(true);
            StartCoroutine(GachaEffectCoroutine());
        }
    }

    public void CancelButtonClicked()
    {
        gachaDialog.SetActive(false);
    }

    public void BackButtonClicked()
    {
        StartCoroutine(FadeIn("MainScene"));
    }

    public void Gacha1ButtonClicked(int gachaCategory)
    {
        _gachaNumber = 1;
        _gachaPrice = 300;
        _gachaCategory = gachaCategory;

        ActiveGachaDialog();
    }

    public void Gacha10ButtonClicked(int gachaCategory)
    {
        _gachaNumber = 10;
        _gachaPrice = 3000;
        _gachaCategory = gachaCategory;

        ActiveGachaDialog();
    }

    public void ActiveGachaDialog()
    {
        if (GameManager.Instance.userData.currentMoney < _gachaPrice)
        {
            gachaDialog.SetNoMoneyDialog();
        }
        else
        {
            gachaDialog.SetDialog(_gachaNumber, _gachaPrice, _gachaCategory);
        }
    }
    public IEnumerator GachaEffectCoroutine()
    {
        int charNum = 0;
        for(int i = 0; i < 10; i++)
        {
            charNum = CheckCharRandomNumber();

            if (charNum > 0)
            {
                GameManager.Instance.userData.myCharactersList.Add(charNum, 0);
            }
            else
            {
                GameManager.Instance.userData.myItemList[2] = GameManager.Instance.userData.myItemList[2] + 1;
            }

            gachaObjects[i].charThumbnail.sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + charNum.ToString());
            gachaObjects[i].MoveCard();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator Gacha1EffectCoroutine()
    {
        int charNum = 0;
        charNum = CheckCharRandomNumber();

        if (charNum > 0)
        {
            GameManager.Instance.userData.myCharactersList.Add(charNum, 0);
        }
        else
        {
            GameManager.Instance.userData.myItemList[2] = GameManager.Instance.userData.myItemList[2] + 1;
        }

        gacha1Object.charThumbnail.sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + charNum.ToString());
        gacha1Object.MoveCard();

        yield return new WaitForSeconds(0.2f);
    }

    public int CheckCharRandomNumber()
    {
        int min = 0;
        int max = 0;
        if (_gachaCategory == 1)
        {
            min = 1;
            max = 24;
        }
        else if(_gachaCategory == 2)
        {
            min = 25;
            max = 36;
        }
        int charNumber = Random.Range(min, max);

        foreach (int charKey in GameManager.Instance.userData.myCharactersList.Keys)
        {
            if (charKey == charNumber)
            {
                charNumber = 0;
            }
        }

        return charNumber;
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
