using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{
    public BuffDataManager buffDataManager;

    public SpriteRenderer fullScreenImage;
    public DeckCharInfoPanel deckCharInfo;
    public Sprite[] charSheet;
    public Sprite[] charIconSheet;

    public BattleCharIcon myCharListPrefab;
    public BattleCharacter battleCharPrefab;
    public BuffDataObject buffIconPrefab;

    public BattleMapTile[] playerTile;
    public List<BattleCharacter> playerList = new List<BattleCharacter>();
    public List<SingleBuffDB> activeBuffDBList = new List<SingleBuffDB>();
    public List<BattleCharIcon> myDeckList = new List<BattleCharIcon>();
    public List<BuffDataObject> myBuffList = new List<BuffDataObject>();

    public GameObject scrollViewContent;
    public GameObject buffIconViewContent;
    public GameObject tileObject;
    public GameObject charObjects;

    public bool isClicked = false;

    public int currentSelectedChar;
    public int currentSelectedCharNumber;

    public int currentDeckNumber;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentSceneState = GameManager.SceneStatus.deck;

        deckCharInfo.SetActiveObject(false);
        buffDataManager.SetBuffDataBase();

        charSheet = GameManager.Instance.charSheet;
        charIconSheet = GameManager.Instance.charIconSheet;

        for (int i = 0; i < GameManager.Instance.userData.myCharacters.Count; i++)
        {
            BattleCharIcon itemObj = Instantiate(myCharListPrefab);
            itemObj.charId = GameManager.Instance.userData.myCharacters[i];
            itemObj.charIcon.sprite = charIconSheet[itemObj.charId - 1];
            itemObj.transform.SetParent(scrollViewContent.transform, false);
            itemObj.Init(GameManager.Instance.userData.myDeck[0]);
            myDeckList.Add(itemObj);
        }

        currentDeckNumber = 0;
        currentSelectedCharNumber = 0;

        SetDeckInit();
        StartCoroutine(FadeOut());
    }

    public void backButtonClicked()
    {
        StartCoroutine(FadeIn("MainScene"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if(hit.collider.GetComponent<BattleMapTile>() != null)
                {
                    SetPlayerChar(hit.collider.GetComponent<BattleMapTile>().mapId);

                    buffDataManager.CheckBuffCharNumber(currentSelectedChar);

                    activeBuffDBList = buffDataManager.GetCurrentBuffList();

                    CheckDeckIcon();
                    BuffIconAdd();

                    currentSelectedChar = 0;
                }

                if (hit.collider.GetComponent<BattleCharIcon>() != null)
                {
                    tileObject.SetActive(false);
                    charObjects.SetActive(false);
                    deckCharInfo.SetActiveObject(true);
                    deckCharInfo.SetData(hit.collider.GetComponent<BattleCharIcon>().charId);
                }
            }
            else
            {
                deckCharInfo.SetActiveObject(false);
                charObjects.SetActive(true);
                tileObject.SetActive(true);
                InitMaptile();
                currentSelectedChar = 0;
                currentSelectedCharNumber = 0;
            }
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            Ray upRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D upHit = Physics2D.GetRayIntersection(upRay, Mathf.Infinity);

            if (upHit.collider != null)
            {
                if (upHit.collider.GetComponent<BattleCharIcon>() != null)
                {
                    tileObject.SetActive(true);
                    charObjects.SetActive(true);
                    deckCharInfo.SetActiveObject(false);

                    BattleCharIcon charIcon;

                    charIcon = upHit.collider.GetComponent<BattleCharIcon>();

                    InitMaptile();
                    if(currentSelectedChar == charIcon.charId)
                    {
                        checkSameCharPosition();
                        CheckDeckIcon();
                        currentSelectedChar = 0;
                        currentSelectedCharNumber = 0;
                    }
                    else
                    {
                        currentSelectedChar = charIcon.charId;
                        currentSelectedCharNumber = charIcon.battleCharDB.namenumber;

                        for (int i = 0; i < charIcon.battleCharDB.position.Length; i++)
                        {
                            playerTile[charIcon.battleCharDB.position[i] - 1].SetActiveSelectImage(true);
                        }
                    }
                }
            }
            else
            {
                tileObject.SetActive(true);
                charObjects.SetActive(true);
                deckCharInfo.SetActiveObject(false);
            }
        }
    }

    public void SetDeckInit()
    {
        if(GameManager.Instance.userData.myDeck[currentDeckNumber].Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.userData.myDeck[currentDeckNumber].Count; i++)
            {
                currentSelectedChar = GameManager.Instance.userData.myDeck[currentDeckNumber][i];

                buffDataManager.CheckBuffCharNumber(currentSelectedChar);

                SetPlayerChar(GameManager.Instance.userData.myDeckPosition[currentDeckNumber][i]);

                currentSelectedChar = 0;
            }

        }

        activeBuffDBList = buffDataManager.GetCurrentBuffList();

        CheckDeckIcon();
        BuffIconAdd();
    }

    public void BuffIconAdd()
    {
        for (int i = 0; i < activeBuffDBList.Count; i++)
        {
            if(CheckSameBuff(activeBuffDBList[i]) == false)
            {
                BuffDataObject itemObj = Instantiate(buffIconPrefab);
                itemObj.InitBuffDataObject(activeBuffDBList[i]);
                itemObj.transform.SetParent(buffIconViewContent.transform, false);
                myBuffList.Add(itemObj);
            }
        }
    }

    public bool CheckSameBuff(SingleBuffDB checkBuff)
    {
        for (int i = 0; i < myBuffList.Count; i++)
        {
            if (myBuffList[i].buffData.id == checkBuff.id)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckDeckIcon()
    {
        for (int i = 0; i < myDeckList.Count; i++)
        {
            myDeckList[i].SetDeckIcon(myDeckList[i].CheckId(playerList.Select(x => x.charId).ToList()));
        }
    }


    public void SetPlayerChar(int selectedMapId)
    {
        if (currentSelectedChar != 0 && checkCurrentCharMapPosition(selectedMapId))
        {
            checkSameCharPosition();
            checkSameMapPosition(selectedMapId);

            BattleCharacter charObj = Instantiate(battleCharPrefab);
            charObj.playerCharInit(charSheet[currentSelectedChar - 1], currentSelectedChar);

            charObj.charPosition = selectedMapId;
            charObj.transform.SetParent(charObjects.transform);
            charObj.transform.position = playerTile[selectedMapId - 1].transform.position;
            charObj.transform.position = new Vector3(charObj.transform.position.x, charObj.transform.position.y, charObj.transform.position.z - 0.5f);

            playerList.Add(charObj);

            InitMaptile();
        }
    }

    public void ResetBuffIcon()
    {
        DeleteBuffICon();

        activeBuffDBList = buffDataManager.GetCurrentBuffList();
    }

    public void DeleteBuffICon()
    {
        for (int i = myBuffList.Count - 1; i >= 0; i--)
        {
            Destroy(myBuffList[i].gameObject);
            myBuffList.RemoveAt(i);
        }

        for (int i = activeBuffDBList.Count - 1; i >= 0; i--)
        {
            activeBuffDBList.RemoveAt(i);
        }
    }

    public void checkSameCharPosition()
    {
        if (playerList.Count > 0)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].charId == currentSelectedChar || playerList[i].charnumber == currentSelectedCharNumber)
                {
                    buffDataManager.DisCountNumber(playerList[i].charnumber);
                    Destroy(playerList[i].gameObject);
                    playerList.RemoveAt(i);

                    ResetBuffIcon();
                    BuffIconAdd();
                }
            }
        }
    }

    public void checkSameMapPosition(int selectedMapId)
    {
        if (playerList.Count > 0)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].charPosition == selectedMapId)
                {
                    buffDataManager.DisCountNumber(playerList[i].charnumber);
                    Destroy(playerList[i].gameObject);
                    playerList.RemoveAt(i);

                    ResetBuffIcon();
                    BuffIconAdd();
                }
            }
        }
    }

    public bool checkCurrentCharMapPosition(int selectedMapId)
    {
        BattleCharacterDB charObjDB = DataBaseManager.Instance.battleCharacterDB.Get(currentSelectedChar);

        for (int i = 0; i < charObjDB.position.Length; i++)
        {
            if (charObjDB.position[i] == selectedMapId)
            {
                return true;
            }
        }

        return false;
    }

    public void InitMaptile()
    {
        for (int i = 0; i < 9; i++)
        {
            playerTile[i].SetActiveSelectImage(false);
        }
    }

    public void DeckNumberButtonClicked(string number)
    {
        currentDeckNumber = int.Parse(number) - 1;
        currentSelectedChar = 0;
        InitMaptile();
        DeleteAllCharList();
        DeleteBuffICon();
        buffDataManager.CleanCheckcount();

        SetDeckInit();
    }

    public void DeckSetButton()
    {
        List<int> setCurrentDeck = new List<int>();
        List<int> setCurrentDeckPosition = new List<int>();

        for (int i = 0; i < playerList.Count; i++)
        {
            setCurrentDeck.Add(playerList[i].charId);
            setCurrentDeckPosition.Add(playerList[i].charPosition);
        }

        GameManager.Instance.userData.myDeck[currentDeckNumber] = setCurrentDeck;
        GameManager.Instance.userData.myDeckPosition[currentDeckNumber] = setCurrentDeckPosition;
    }

    public void DeleteAllCharList()
    {
        if(playerList.Count > 0)
        {
            for (int i = playerList.Count - 1; i >= 0; i--)
            {
                Destroy(playerList[i].gameObject);
                playerList.RemoveAt(i);
            }
        }
    }

    public IEnumerator FadeOut()
    {
        isClicked = false;
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

        isClicked = true;
    }

    public IEnumerator FadeIn(string changescene = "")
    {
        isClicked = false;

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

        isClicked = true;
    }
}
