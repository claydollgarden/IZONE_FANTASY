using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public CutInManager cutInManager;
    public VideoManager videoManager;
    public EffectManager effectManager;
    public BuffDataManager buffDataManager;
    public BattleMapTile[] playerTile;
    public BattleMapTile[] enemyTile;

    public List<BattleCharacter> enemyList = new List<BattleCharacter>();
    public List<BattleCharacter> playerList = new List<BattleCharacter>();
    public List<BattleCharacter> allBattleCharList = new List<BattleCharacter>();
    public List<SingleBuffDB> activeBuffDBList = new List<SingleBuffDB>();
    public List<BuffDataObject> myBuffList = new List<BuffDataObject>();

    public BuffDataObject buffIconPrefab;

    public BattleCharacter battleCharPrefab;

    public int currentSelectedChar;
    public int currentDeckNumber;

    public GameObject charObjects;
    public GameObject tileObject;
    public GameObject buffIconViewObject;
    public GameObject buffIconViewContent;

    public Sprite[] charSheet;

    public BattleStatus battleStatus;

    public List<BattleCharacter> charTurnList = new List<BattleCharacter>();

    public BattleCharacter currentTurnChar;
    public BattleCharacter targetChar;

    public SkillCard selectedCard;

    public bool battleClearFlg = false;

    public int gold = 0;
    public int exp = 0;
    public List<int> itemList = new List<int>();

    public enum BattleStatus
    {
        Init = 0,
        decksetting,
        battlestart,
        nextturn,
        select,
        active,
        turnend,
        exit
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentSceneState = GameManager.SceneStatus.battle;
        battleUIManager.battleResultPanel.gameObject.SetActive(false);
        battleUIManager.ActiveButtons(true);
        buffIconViewObject.SetActive(true);
        buffDataManager.SetBuffDataBase();
        battleStatus = BattleStatus.Init;
        battleCharPrefab.enabled = false;
        selectedCard = null;
        InitMaptile();

        StartCoroutine("StartBattle");
    }

    public void setEnemy()
    {
        for (int i = 0; i < GameManager.Instance.currentBattleMapData.enemy.Length; i++)
        {
            BattleCharacter charObj = Instantiate(battleCharPrefab);

            charObj.enemyCharInit(charSheet[GameManager.Instance.currentBattleMapData.enemy[i] - 1], GameManager.Instance.currentBattleMapData.enemy[i]);

            charObj.charPosition = GameManager.Instance.currentBattleMapData.position[i];
            charObj.transform.position = enemyTile[charObj.charPosition].transform.position;
            charObj.transform.position = new Vector3(charObj.transform.position.x, charObj.transform.position.y, charObj.transform.position.z - 0.5f);

            enemyList.Add(charObj);
        }
    }

    public void InitMaptile()
    {
        for (int i = 0; i < 9; i++)
        {
            playerTile[i].SetActiveSelectImage(false);
            playerTile[i].SetActiveTargetImage(false);

            enemyTile[i].SetActiveSelectImage(false);
            enemyTile[i].SetActiveTargetImage(false);
        }
    }

    public void SetPlayerChar(int selectedMapId)
    {
        if (currentSelectedChar != 0)
        {
            BattleCharacter charObj = Instantiate(battleCharPrefab);
            charObj.playerCharInit(charSheet[currentSelectedChar - 1], currentSelectedChar);

            charObj.charPosition = selectedMapId;
            charObj.transform.SetParent(charObjects.transform);
            charObj.transform.position = playerTile[selectedMapId - 1].transform.position;
            charObj.transform.position = new Vector3(charObj.transform.position.x, charObj.transform.position.y, charObj.transform.position.z - 0.5f);

            playerList.Add(charObj);

            currentSelectedChar = 0;

            InitMaptile();
        }
    }

    public void SetOrderTurn()
    {
        charTurnList = allBattleCharList.OrderByDescending(x => x.speed).ToList();
        foreach (var turn in charTurnList) 
        {
            Debug.Log("턴 순서 : " + turn.charName);
        }
    }

    public void SkillCardClicked(SkillCard cardInfo)
    {
        InitMaptile();
        battleUIManager.skillSelectPanel.ActiveSkillCards(false);
        selectedCard = cardInfo;

        SetTargetIcon(cardInfo.skillData.target);

        cardInfo.SelectedCard(true);
    }

    public void SetTargetIcon(int[] Targets)
    {
        for(int i = 0; i < Targets.Length; i ++)
        {
            enemyTile[Targets[i] - 1].SetActiveTargetImage(true);
        }
    }

    public bool CheckSkillTarget(BattleCharacter targetChar)
    {
        for(int i = 0; i < selectedCard.skillData.target.Length; i++)
        {
            if( selectedCard.skillData.target[i] == targetChar.charPosition + 1)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckEnemySkillTarget()
    {
        SkillCard skillCard = new SkillCard();
        for(int i = 0; i < currentTurnChar.battleEnemySkills.Count; i++)
        {
            for(int j = 0; j < currentTurnChar.battleEnemySkills[i].target.Length; j++)
            {
                for (int k = 0; k < playerList.Count; k++)
                {
                    if (currentTurnChar.battleEnemySkills[i].target[j] == playerList[k].charPosition)
                    {
                        targetChar = playerList[k];

                        skillCard.skillData = currentTurnChar.battleEnemySkills[i];

                        selectedCard = skillCard;
                    }
                }
            }
        }
    }

    public void SetBuffStatus()
    {
        foreach(BattleCharacter currentChar in playerList)
        {
            foreach(BuffDataObject buffData in myBuffList)
            {
                currentChar.SetBuffStatus(buffData.buffData.status, buffData.buffData.effect);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            
            if (battleStatus == BattleStatus.select)
            {
                if (hit.collider != null && selectedCard != null)
                {
                    if (hit.collider.GetComponent<BattleCharacter>() != null && CheckSkillTarget(hit.collider.GetComponent<BattleCharacter>()))
                    {
                        targetChar = hit.collider.GetComponent<BattleCharacter>();
                    
                        InitMaptile();

                        battleStatus = BattleStatus.active;
                    }
                }
                else
                {
                    InitMaptile();
                    battleUIManager.skillSelectPanel.ActiveSkillCards(false);
                    selectedCard = null;
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray upRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D upHit = Physics2D.GetRayIntersection(upRay, Mathf.Infinity);
        }
    }

    public void SetDeckInit()
    {
        if (GameManager.Instance.userData.myDeck[currentDeckNumber].Count > 0)
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

        BuffIconAdd();
    }

    public void BuffIconAdd()
    {
        for (int i = 0; i < activeBuffDBList.Count; i++)
        {
            BuffDataObject itemObj = Instantiate(buffIconPrefab);
            itemObj.InitBuffDataObject(activeBuffDBList[i]);
            itemObj.transform.SetParent(buffIconViewContent.transform, false);
            myBuffList.Add(itemObj);
        }
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

    public void DeleteAllCharList()
    {
        if (playerList.Count > 0)
        {
            for (int i = playerList.Count - 1; i >= 0; i--)
            {
                Destroy(playerList[i].gameObject);
                playerList.RemoveAt(i);
            }
        }
    }

    public void DeckSetCompleteClicked()
    {
        battleStatus = BattleStatus.battlestart;
    }

    public void GoToDeckSceneClicked()
    {
        SceneManager.LoadScene("DeckScene");
    }

    public bool CheckEnemeyAllDie()
    {
        foreach(BattleCharacter enemyCharacter in enemyList)
        {
            if(enemyCharacter.isAlive == true)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckPlayerAllDie()
    {
        foreach (BattleCharacter playerCharacter in playerList)
        {
            if (playerCharacter.isAlive == true)
            {
                return false;
            }
        }

        return true;
    }

    public void BattleResultButtonClicked()
    {
        if (battleStatus == BattleStatus.exit)
        {
            foreach(BattleCharacter player in playerList)
            {
                GameManager.Instance.userData.myCharactersList[player.charId] = GameManager.Instance.userData.myCharactersList[player.charId] + exp;
            }

            GameManager.Instance.userData.currentMoney = GameManager.Instance.userData.currentMoney + gold;

            foreach(int item in itemList)
            {
                if(item < 3)
                {
                    GameManager.Instance.userData.myItemList[item] = GameManager.Instance.userData.myItemList[item] + 1;
                }
                else
                {
                    ItemDB itemObj = DataBaseManager.Instance.itemDB.Get(item);

                    GameManager.Instance.userData.myCharactersList.Add(itemObj.charnumber, 0);
                }

            }

            SceneManager.LoadScene("MainScene");
        }
        else
        {
            battleUIManager.skillSelectPanel.activeSkillPanel(true);
            battleUIManager.battleResultPanel.gameObject.SetActive(false);
        }
    }

    public void PauseButtonClicked()
    {
        battleUIManager.skillSelectPanel.activeSkillPanel(false);

        battleUIManager.battleResultPanel.SetItemIcon(itemList, gold, exp);

        battleUIManager.battleResultPanel.gameObject.SetActive(true);
    }

    IEnumerator StartBattle()
    {
        while (battleStatus != BattleStatus.exit)
        {
            switch (battleStatus)
            {
                case BattleStatus.Init:
                    yield return Init();
                    break;
                case BattleStatus.decksetting:
                    yield return DeckSetting();
                    break;
                case BattleStatus.battlestart:
                    yield return BattleStart();
                    break;
                case BattleStatus.nextturn:
                    yield return NextTurn();
                    break;
                case BattleStatus.select:
                    yield return SelectSkill();
                    break;
                case BattleStatus.active:
                    yield return SkillActive();
                    break; 
                case BattleStatus.turnend:
                    yield return TurnEnd();
                    break;
                case BattleStatus.exit:
                    yield return BattleEnd();
                    break;
            }

            yield return null;
        }

    }

    IEnumerator Init()
    {
        battleUIManager.skillSelectPanel.activeSkillPanel(false);

        charSheet = GameManager.Instance.charSheet;

        setEnemy();

        currentDeckNumber = 0;
        SetDeckInit();

        battleStatus = BattleStatus.decksetting;
        yield return null;
    }

    IEnumerator DeckSetting()
    {
        yield return null;
    }

    IEnumerator BattleStart()
    {
        battleUIManager.BattleStartMessge(true);
        battleUIManager.deckButtons.SetActive(false);
        battleUIManager.ActiveButtons(false);
        buffIconViewObject.SetActive(false);
        SetBuffStatus();

        for (int i = 0; i < playerList.Count; i++)
        {
            allBattleCharList.Add(playerList[i]);
        }
        
        for(int i = 0; i < enemyList.Count; i++)
        {
            allBattleCharList.Add(enemyList[i]);
        }

        for (int i = 0; i < 9; i++)
        {
            playerTile[i].mapCollider.enabled = false;
        }

        SetOrderTurn();

        for (int i = 0; i < allBattleCharList.Count; i++)
        {
            allBattleCharList[i].charCollider.enabled = true;
        }

        yield return new WaitForSeconds(1.0f);

        battleUIManager.BattleStartMessge(false);

        currentTurnChar = charTurnList[0];

        battleUIManager.skipButton.gameObject.SetActive(true);
        battleUIManager.SetTurnOrderIcon(charTurnList);

        battleStatus = BattleStatus.nextturn;

        yield return null;
    }
    IEnumerator NextTurn()
    {
        if(currentTurnChar.playerSide)
        {
            battleUIManager.skillSelectPanel.activeSkillPanel(true);

            currentTurnChar.ActiveMaterial(true);
            battleUIManager.ActiveMaterial(true);

            for (int i = 0; i < battleUIManager.skillSelectPanel.skillCards.Length; i++)
            {
                if (currentTurnChar.battleEnemySkills[i] != null)
                {
                    battleUIManager.skillSelectPanel.skillCards[i].InitSkillCard(currentTurnChar.battleEnemySkills[i]);
                }
                else
                {
                    battleUIManager.skillSelectPanel.skillCards[i].gameObject.SetActive(false);
                }
            }

            battleStatus = BattleStatus.select;
        }
        else
        {
            CheckEnemySkillTarget();
            InitMaptile();

            battleStatus = BattleStatus.active;
        }

        yield return null;
    }

    IEnumerator SelectSkill()
    {
        yield return null;
    }

    IEnumerator SkillActive()
    {
        currentTurnChar.ActiveMaterial(false);
        battleUIManager.ActiveMaterial(false);
        battleUIManager.skillSelectPanel.activeSkillPanel(false);

        BattleCharacterDB charObjDB = DataBaseManager.Instance.battleCharacterDB.Get(currentTurnChar.charId);

        yield return cutInManager.LoadThumbNailAndPlay(charObjDB.id.ToString());

        if(battleUIManager.skipButton.isOn == false)
        {
            videoManager.Play(selectedCard.skillData.moviepath);

            yield return new WaitForSeconds(2.0f);

            videoManager.Stop();
        }

        var startPosition = currentTurnChar.transform.position;

        currentTurnChar.StartJumpChar(startPosition, targetChar.transform.position);
        yield return new WaitForSeconds(0.2f);

        currentTurnChar.PlayCharacterAnimation("BattleCharacterAttack");
        yield return effectManager.LoadEffectAndPlay(selectedCard.skillData.effectpath, targetChar.transform.position);

        int damage = currentTurnChar.atk * (selectedCard.skillData.atk / 100);

        targetChar.SetCurrentHP(damage);

        battleUIManager.damageTextObject.StartDamageTextAnimation(damage, targetChar.transform.position);

        if (targetChar.isAlive == false)
        {
            charTurnList.Remove(targetChar);

            TurnOrderIcon targetOrderIcon = null;

            foreach (TurnOrderIcon orderIcon in battleUIManager.turnOrderIcons)
            {
                if (orderIcon.charId == targetChar.charId)
                {
                    targetOrderIcon = orderIcon;
                }
            }

            if(targetOrderIcon != null)
            {
                battleUIManager.DeleteTurnOrderIcon(targetOrderIcon);
            }

            if (targetChar.playerSide == false)
            {
                gold += targetChar.charGold;
                exp += targetChar.charExp;
                if(targetChar.itemNumber > 0)
                {
                    itemList.Add(targetChar.itemNumber);
                }
            }
        }
        else
        {
            targetChar.PlayCharacterAnimation("BattleCharacterDamage");
        }

        currentTurnChar.EndJumpChar(targetChar.transform.position, startPosition);
        
        battleStatus = BattleStatus.turnend;

        yield return null;
    }

    IEnumerator TurnEnd()
    {
        bool ClearCheck = false;

        if(currentTurnChar.playerSide)
        {
            ClearCheck = CheckEnemeyAllDie();
        }
        else 
        {
            ClearCheck = CheckPlayerAllDie();
        }

        if (ClearCheck)
        {
            if (currentTurnChar.playerSide)
            {
                battleClearFlg = true;
            }
            else
            {
                battleClearFlg = false;
            }

            battleUIManager.skillSelectPanel.activeSkillPanel(false);
            battleUIManager.battleResultPanel.SetItemIcon(itemList, gold, exp);
            battleUIManager.battleResultPanel.gameObject.SetActive(true);

            battleStatus = BattleStatus.exit;
        }
        else
        {
            targetChar = null;
            selectedCard = null;
            currentTurnChar = charTurnList[1];

            battleUIManager.AddTurnOrderIcon(charTurnList[0]);
            battleUIManager.DeleteTurnOrderIcon();

            charTurnList.Add(charTurnList[0]);
            charTurnList.Remove(charTurnList[0]);

            battleStatus = BattleStatus.nextturn;
        }

        yield return null;
    }

    IEnumerator BattleEnd()
    {

        yield return null;
    }
}

