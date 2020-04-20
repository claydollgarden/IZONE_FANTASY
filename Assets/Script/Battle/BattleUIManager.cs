using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleUIManager : MonoBehaviour
{
    public GameObject unitSelectUIPanel;
    public GameObject battleStartMessageObj;
    public GameObject deckButtons;
    public TurnOrderIcon turnOrderPrefab;
    public GameObject turnOrderPanelTransform;
    public DamageTextObject damageTextObject;

    public Toggle skipButton;

    public SkillSelectPanel skillSelectPanel;

    public List<TurnOrderIcon> turnOrderIcons = new List<TurnOrderIcon>();

    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(false);
        unitSelectUIPanel.transform.localPosition = new Vector3(500.0f, unitSelectUIPanel.transform.localPosition.y, unitSelectUIPanel.transform.localPosition.z);
    }

    public void DeckSetCompleteClicked()
    {
        Debug.Log("DeckSetCompleteClicked");
        //SceneManager.LoadScene("MapScene");
        if(BattleManager.Instance.playerList.Count > 0 && BattleManager.Instance.playerList.Count <= 6)
        {
            unitSelectUIPanel.transform.localPosition = new Vector3(1420.0f, unitSelectUIPanel.transform.localPosition.y, unitSelectUIPanel.transform.localPosition.z);
            BattleManager.Instance.battleStatus = BattleManager.BattleStatus.battlestart;
        }
    }

    public void BattleStartMessge(bool flg)
    {
        battleStartMessageObj.SetActive(flg);
    }

    public void SetTurnOrderIcon(List<BattleCharacter> TurnOrderCharList)
    {
        for(int i = 0; i < TurnOrderCharList.Count; i++)
        {
            AddTurnOrderIcon(TurnOrderCharList[i]);
        }
    }

    public void ActiveMaterial(bool flg)
    {
        turnOrderIcons[0].charBG.material.SetFloat("_Outline", flg ? 1f : 0);
        turnOrderIcons[0].charBG.material.SetColor("_OutlineColor", Color.green);
        turnOrderIcons[0].charBG.material.SetFloat("_OutlineSize", flg ? 20.0f : 0.0f);
    }

    public void AddTurnOrderIcon(BattleCharacter TurnOrderChar)
    {
        TurnOrderIcon turnOrderIcon = Instantiate(turnOrderPrefab);
        turnOrderIcon.transform.parent = turnOrderPanelTransform.transform;

        turnOrderIcon.charIcon.sprite = TurnOrderChar.charImage.sprite;
        turnOrderIcon.transform.localScale = new Vector3(250.0f, 250.0f, 1.0f);

        turnOrderIcons.Add(turnOrderIcon);
    }

    public void DeleteTurnOrderIcon()
    {
        Destroy(turnOrderIcons[0].gameObject);
        turnOrderIcons.Remove(turnOrderIcons[0]);
    }
}
