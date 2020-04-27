using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public Image backGroundImage;
    public SpriteRenderer fullScreenImage;

    public bool isClicked = false;

    public GameObject[] mapTile;

    public Sprite[] charSheet;
    public Sprite[] charIconSheet;

    public BattleCharacter battleCharPrefab;

    public List<BattleCharacter> playerList = new List<BattleCharacter>();

    public int currentSelectedChar = 0;

    private void Start()
    {
        charSheet = GameManager.Instance.charSheet;
        charIconSheet = GameManager.Instance.charIconSheet;

        for (int i = 0; i < GameManager.Instance.userData.myDeck[0].Count; i++)
        {
            currentSelectedChar = GameManager.Instance.userData.myDeck[0][i];
            SetPlayerChar(GameManager.Instance.userData.myDeckPosition[0][i]);
        }

        GameManager.Instance.currentSceneState = GameManager.SceneStatus.main;
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("BattleCharacterDie");
            playerList[0].PlayCharacterAnimation("BattleCharacterDie");
        }
    }

    public void SetPlayerChar(int selectedMapId)
    {
        BattleCharacter charObj = Instantiate(battleCharPrefab);
        charObj.playerCharInit(charSheet[currentSelectedChar - 1], currentSelectedChar);

        charObj.charPosition = selectedMapId;
        charObj.transform.position = mapTile[selectedMapId - 1].transform.position;
        charObj.transform.position = new Vector3(charObj.transform.position.x, charObj.transform.position.y, charObj.transform.position.z - 0.5f);
        charObj.transform.localScale = new Vector3(54.0f, 54.0f, 1.0f);
        playerList.Add(charObj);
    }

    public void DeckButtonClicked()
    {
        if(isClicked == false)
        {
            StartCoroutine(FadeIn("DeckScene"));
        }
    }

    public void MapButtonClicked()
    {
        if (isClicked == false)
        {
            StartCoroutine(FadeIn("MapScene"));
        }
    }

    public void GachaButtonClicked()
    {
        if (isClicked == false)
        {
            StartCoroutine(FadeIn("GachaScene"));
        }
    }

    public void SettingButtonClicked()
    {
        if (isClicked == false)
        {
            StartCoroutine(FadeIn("NovelScene"));
        }
    }

    public void ReinforceButtonClicked()
    {
        if (isClicked == false)
        {
            StartCoroutine(FadeIn("ReinforceScene"));
        }
    }

    public IEnumerator FadeOut()
    {
        isClicked = true;
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

        isClicked = false;
    }

    public IEnumerator FadeIn(string changescene = "")
    {
        isClicked = true;

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

        isClicked = false;
    }
}
