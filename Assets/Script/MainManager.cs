using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<SingleBuffDB> currentBuffDB = new List<SingleBuffDB>();
    public BuffListIconObject buffListIconPrefab;
    public GameObject buffIconViewContent;
    public GameObject buffListView;

    public CardListObject cardListObjectPrefab;
    public GameObject cardListViewContent;
    public GameObject cardListView;


    private void Start()
    {
        charSheet = GameManager.Instance.charSheet;
        charIconSheet = GameManager.Instance.charIconSheet;

        if (GameManager.Instance.userData.myCharactersList == null)
        {
            GameManager.Instance.userData.SetAsDefault();
        }

        Debug.Log("GameManager.Instance.userData.myCharactersList : " + GameManager.Instance.userData.myCharactersList.Count);

        for (int i = 0; i < GameManager.Instance.userData.myDeck[0].Count; i++)
        {
            currentSelectedChar = GameManager.Instance.userData.myDeck[0][i];
            SetPlayerChar(GameManager.Instance.userData.myDeckPosition[0][i]);
        }

        SetBuffList();
        SetCardList();
        GameManager.Instance.currentSceneState = GameManager.SceneStatus.main;

        buffListView.SetActive(false);
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.Log("BattleCharacterDie");
        //    playerList[0].PlayCharacterAnimation("BattleCharacterDie");
        //}
    }

    public void CloseBuffListView()
    {
        buffListView.SetActive(false);
    }

    public void CloseCardListView()
    {
        cardListView.SetActive(false);
    }

    public void SetBuffList()
    {
        currentBuffDB = DataBaseManager.Instance.singleBuffDB.GetAll();

        for (int i = 0; i < currentBuffDB.Count; i++)
        {
            BuffListIconObject itemObj = Instantiate(buffListIconPrefab);
            itemObj.SetIcon(currentBuffDB[i]);
            itemObj.transform.SetParent(buffIconViewContent.transform, false);
        }
    }

    public void SetCardList()
    {
        List<BattleCharacterDB> currentCharDB = DataBaseManager.Instance.battleCharacterDB.GetAll();
        List<int> myCharDB = GameManager.Instance.userData.myCharactersList.Keys.ToList();

        for (int i = 0; i < currentCharDB.Count; i++)
        {
            CardListObject itemObj = Instantiate(cardListObjectPrefab);
            var sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + currentCharDB[i].id);
            itemObj.cardImage.sprite = sprite;
            itemObj.cardImage.color = Color.gray;

            for (int j = 0; j < myCharDB.Count; j++)
            {
                if(myCharDB[j] == currentCharDB[i].id)
                {
                    itemObj.cardImage.color = Color.white;
                }
            }
            itemObj.transform.SetParent(cardListViewContent.transform, false);
        }
    }

    public void SetPlayerChar(int selectedMapId)
    {
        BattleCharacter charObj = Instantiate(battleCharPrefab);
        charObj.playerCharInit(currentSelectedChar);
        charObj.SetCharSpriteSet(charSheet[charObj.charnumber - 1]);

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
            DataBaseManager.Instance.saveData.Save();
            //StartCoroutine(FadeIn("NovelScene"));
        }
    }

    public void ReinforceButtonClicked()
    {
        if (isClicked == false)
        {
            StartCoroutine(FadeIn("ReinforceScene"));
        }
    }

    public void BuffListButtonClicked()
    {
        buffListView.SetActive(true);
    }

    public void CardListButtonClicked()
    {
        cardListView.SetActive(true);
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
