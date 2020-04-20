﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;
    public bool isClicked = false;

    private void Start()
    {
        SoundManager.Instance.PlaySingle("ColorConcept");

        GameManager.Instance.currentSceneState = GameManager.SceneStatus.title;
    }

    public void GameStartButton()
    {
        if(isClicked == false)
        {
            SoundManager.Instance.PlayOneShot("TeamSlogan");
            StartCoroutine(NextScene());
        }
    }

    public IEnumerator NextScene()
    {
        isClicked = true;
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

        SceneManager.LoadScene("MainScene");

        isClicked = false;
    }
}