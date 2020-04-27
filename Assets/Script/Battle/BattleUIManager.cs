using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleUIManager : MonoBehaviour
{
    public GameObject battleStartMessageObj;
    public GameObject deckButtons;
    public TurnOrderIcon turnOrderPrefab;
    public GameObject turnOrderPanelTransform;
    public DamageTextObject damageTextObject;

    public Button[] buttons;

    public Toggle skipButton;

    public SkillSelectPanel skillSelectPanel;
    public BattleResultPanel battleResultPanel;

    public List<TurnOrderIcon> turnOrderIcons = new List<TurnOrderIcon>();

    // Start is called before the first frame update
    void Start()
    {
        battleResultPanel.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }

    public void BattleStartMessge(bool flg)
    {
        battleStartMessageObj.SetActive(flg);
    }

    public void ActiveButtons(bool flg)
    {
        foreach(Button btn in buttons)
        {
            btn.gameObject.SetActive(flg);
        }
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
        turnOrderIcon.charId = TurnOrderChar.charId;

        turnOrderIcons.Add(turnOrderIcon);
    }

    public void DeleteTurnOrderIcon(TurnOrderIcon charIcon = null)
    {
        if(charIcon == null)
        {
            Destroy(turnOrderIcons[0].gameObject);
            turnOrderIcons.Remove(turnOrderIcons[0]);
        }
        else
        {
            Destroy(charIcon.gameObject);
            turnOrderIcons.Remove(charIcon);
        }
    }
}
