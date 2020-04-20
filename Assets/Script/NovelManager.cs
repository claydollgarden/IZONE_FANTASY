﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NovelManager : MonoBehaviour
{
    public Image novelBackGround;
    public Image textBackGround;

    public SpriteRenderer fullScreenImage;
    public SpriteRenderer leftChar;
    public SpriteRenderer rightChar;

    public Text novelText;

    public VisualNovelDB currentVisualNovelDB;

    public bool touchFlg = true;

    //대사, 화면에뜰 사람수, 포커스중인 사람, 왼쪽사람번호, 왼쪽사람 스프라이트 번호, 오른쪽사람번호, 오른쪽사람 스프라이트 번호, 화면효과
    public List<string[]> scenarioText = new List<string[]>();

    public int scriptLine = 0;

    public void SetBackGround(string imagePath)
    {
        var sprite = VResourceLoad.Load<Sprite>("BackGround/" + imagePath);

        novelBackGround.sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        leftChar.enabled = false;
        rightChar.enabled = false;

        currentVisualNovelDB = DataBaseManager.Instance.visualNovelDB.Get(GameManager.Instance.userData.questStep + 1);
        SetNovelData();

        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Debug.Log("touchCount");
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Ended && touchFlg == false)
        //    {
        //        touchFlg = true;
        //        scriptLine++;
        //        SetNovelData();
        //    }
        //}

        if(Input.GetMouseButtonUp(0))
        {
            touchFlg = true;
            scriptLine++;

            if (currentVisualNovelDB.script.Length <= scriptLine)
            {
                StartCoroutine(FadeIn("MapScene"));
            }

            SetNovelData();
        }
    }

    public void SetNovelData()
    {
        if (currentVisualNovelDB.script.Length <= scriptLine)
        {
            return;
        }

        if (currentVisualNovelDB.leftchar[scriptLine] == 0)
        {
            leftChar.enabled = false;
        }
        else
        {
            leftChar.enabled = true;
            var left = VResourceLoad.Load<Sprite>("CharThumbnail/" + currentVisualNovelDB.leftchar[scriptLine]);
            leftChar.sprite = left;
        }

        if (currentVisualNovelDB.rightchar[scriptLine] == 0)
        {
            rightChar.enabled = false;
        }
        else
        {
            rightChar.enabled = true;
            var right = VResourceLoad.Load<Sprite>("CharThumbnail/" + currentVisualNovelDB.rightchar[scriptLine]);
            rightChar.sprite = right;
        }

        novelText.text = currentVisualNovelDB.script[scriptLine];

        touchFlg = false;
    }

    public IEnumerator FadeOut()
    {
        Color color = fullScreenImage.color;

        float time = 1.0f;
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

        float time = 1.0f;
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