using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;

    private void Start()
    {
        StartCoroutine(FadeOut());

        //if(GameManager.Instance.userData.questStep == 0)
        //{
        //    StartCoroutine(FadeIn());

        //    SceneManager.LoadScene("NovelScene");
        //}
    }

    public void BackButtonClicked()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void MapButtonClick(Text mapName)
    {
        GameManager.Instance.currentBattleMapData = DataBaseManager.Instance.fieldSymbolDB.Get(int.Parse(mapName.text));

        SceneManager.LoadScene("BattleScene");
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
