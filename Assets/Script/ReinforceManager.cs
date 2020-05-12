using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReinforceManager : MonoBehaviour
{
    public SpriteRenderer fullScreenImage;
    public SpriteRenderer expImage;

    public List<BattleCharIcon> myDeckList = new List<BattleCharIcon>();
    public BattleCharacterDB currentSelectedCharDB;

    public Dictionary<int, int> testList = new Dictionary<int, int>() { { 4, 146 }, { 9, 482 } };

    public Sprite[] charIconSheet;

    public BattleCharIcon myCharListPrefab;
    public GameObject scrollViewContent;

    public bool isClicked = false;
    public Image thumbnailImage;

    public Text nameText;
    public Text atkText;
    public Text defText;
    public Text speedText;
    public Text descText;
    public Text levelText;
    public Text expText;

    public Text illustedName;

    public Text item1Count;
    public Text item2Count;

    public Text expCountText;

    int item1 = 0;
    int item2 = 0;
    int expCount = 0;

    int currentCharNumber = 0;

    void Start()
    {
        charIconSheet = GameManager.Instance.charIconSheet;

        foreach (var myCharacter in GameManager.Instance.userData.myCharactersList.OrderBy(i => i.Key))
        {
            BattleCharIcon itemObj = Instantiate(myCharListPrefab);
            itemObj.charId = myCharacter.Key;
            //itemObj.charId = GameManager.Instance.userData.myCharacters[i];
            itemObj.Init();
            itemObj.transform.SetParent(scrollViewContent.transform, false);
            itemObj.charIcon.sprite = charIconSheet[itemObj.battleCharDB.namenumber - 1];
            myDeckList.Add(itemObj);
        }

        //for (int i = 0; i < GameManager.Instance.userData.myCharactersList.Count; i++)
        //{
        //    BattleCharIcon itemObj = Instantiate(myCharListPrefab);
        //    itemObj.charId = GameManager.Instance.userData.myCharactersList.Keys.ToList()[i];
        //    itemObj.transform.SetParent(scrollViewContent.transform, false);
        //    itemObj.Init();
        //    itemObj.charIcon.sprite = charIconSheet[itemObj. - 1];
        //    itemObj.SetDeckIcon(false);
        //    myDeckList.Add(itemObj);
        //}

        item1 = GameManager.Instance.userData.myItemList[1];
        item2 = GameManager.Instance.userData.myItemList[2];

        SetData(myDeckList[0].charId);

        StartCoroutine(FadeOut());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<BattleCharIcon>() != null)
                {
                    SetData(hit.collider.GetComponent<BattleCharIcon>().charId);
                }
            }
        }

    }

    public void SetData(int battleCharNumber)
    {
        var sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + battleCharNumber.ToString());

        thumbnailImage.sprite = sprite;

        currentSelectedCharDB = DataBaseManager.Instance.battleCharacterDB.Get(battleCharNumber, false);
        int level = (GameManager.Instance.userData.myCharactersList[battleCharNumber] / 150);
        levelText.text = (level + 1).ToString();
        nameText.text = currentSelectedCharDB.name;
        atkText.text = (currentSelectedCharDB.atk * (10 + level) / 10).ToString();
        defText.text = (currentSelectedCharDB.def * (10 + level) / 10).ToString();
        descText.text = currentSelectedCharDB.description;
        speedText.text = (currentSelectedCharDB.speed * (10 + level) / 10).ToString();
        expText.text = (GameManager.Instance.userData.myCharactersList[battleCharNumber] % 150).ToString();
        
        expImage.transform.localScale = new Vector3((float)(GameManager.Instance.userData.myCharactersList[battleCharNumber] % 150) / 150, 1.0f, 1.0f);

        item1Count.text = " x " + item1.ToString();
        item2Count.text = " x " + item2.ToString();

        illustedName.text = "illusted by : @" + currentSelectedCharDB.illust;

        expCountText.text = expCount.ToString();

        currentCharNumber = battleCharNumber;
    }

    public void itemButtonClicked(int number)
    {
        if(number == 1 && item1 > 0)
        {
            item1--;
            item1Count.text = " x " + item1.ToString();
            expCount += 30;
            expCountText.text = expCount.ToString();
        }
        else if(number == 2 && item2 > 0)
        {
            item2--;
            item2Count.text = " x " + item2.ToString();
            expCount += 60;
            expCountText.text = expCount.ToString();
        }
    }

    public void backButtonClicked()
    {
        StartCoroutine(FadeIn("MainScene"));
    }

    public void SetButtonClicked()
    {
        GameManager.Instance.userData.myItemList[1] = item1;
        GameManager.Instance.userData.myItemList[2] = item2;
        item1 = GameManager.Instance.userData.myItemList[1];
        item2 = GameManager.Instance.userData.myItemList[2];

        StartCoroutine(CountAnimation(expCount));

        expCount = 0;
        expCountText.text = expCount.ToString();
    }

    public void ClearButtonClicked()
    {
        item1 = GameManager.Instance.userData.myItemList[1];
        item2 = GameManager.Instance.userData.myItemList[2];
        item1Count.text = " x " + item1.ToString();
        item2Count.text = " x " + item2.ToString();

        expCount = 0;
        expCountText.text = expCount.ToString();
    }

    public IEnumerator CountAnimation(int countNumber)
    {
        float time = 0.5f;
        float elapsedTime = 0.0f;
        float count = GameManager.Instance.userData.myCharactersList[currentCharNumber];

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;

            count += (t * countNumber);

            expImage.transform.localScale = new Vector3((count % 150) / 150, 1.0f, 1.0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        GameManager.Instance.userData.myCharactersList[currentCharNumber] = GameManager.Instance.userData.myCharactersList[currentCharNumber] + countNumber;

        expText.text = (GameManager.Instance.userData.myCharactersList[currentCharNumber] % 150).ToString();

        int level = (GameManager.Instance.userData.myCharactersList[currentCharNumber] / 150);
        levelText.text = (level + 1).ToString();
        atkText.text = (currentSelectedCharDB.atk * (10 + level) / 10).ToString();
        defText.text = (currentSelectedCharDB.def * (10 + level) / 10).ToString();
        speedText.text = (currentSelectedCharDB.speed * (10 + level) / 10).ToString();

        expImage.transform.localScale = new Vector3((float)(GameManager.Instance.userData.myCharactersList[currentCharNumber] % 150) / 150, 1.0f, 1.0f);
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

        fullScreenImage.enabled = false;
    }

    public IEnumerator FadeIn(string changescene = "")
    {
        fullScreenImage.enabled = true;
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
