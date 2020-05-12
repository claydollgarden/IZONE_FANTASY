using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;
    public GameObject[] mapObjects;

    private void Start()
    {
        for(int i = 0; i < mapObjects.Length; i++)
        {
            mapObjects[i].SetActive(false);
        }

        for(int i = 0; i < GameManager.Instance.userData.clearedQuests + 1; i++)
        {
            mapObjects[i].SetActive(true);
        }

        StartCoroutine(FadeOut());

        //if(GameManager.Instance.userData.questStep == 0)
        //{
        //    StartCoroutine(FadeIn());

        //    SceneManager.LoadScene("NovelScene");
        //}
    }

    public void BackButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MapButtonClick(string mapName)
    {
        string[] splitArray = mapName.Split(char.Parse(","));
        GameManager.Instance.currentBattleMapData = DataBaseManager.Instance.fieldSymbolDB.Get(int.Parse(splitArray[0]));

        if(splitArray[1] != "0")
        {
            GameManager.Instance.novelNumber = int.Parse(splitArray[1]);
            StartCoroutine(FadeIn("NovelScene"));
        }
        else
        {
           StartCoroutine(FadeIn("BattleScene"));
        }

    }

    public void NovelButtonClick()
    {
        StartCoroutine(FadeIn("NovelScene"));
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

        if(changescene != "")
        {
            SceneManager.LoadScene(changescene);
        }
    }
}
