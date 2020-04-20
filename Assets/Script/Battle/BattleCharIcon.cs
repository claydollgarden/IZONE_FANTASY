using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleCharIcon : MonoBehaviour
{
    public int charId;
    public Image charIcon;
    public Text charName;

    public BattleCharacterDB battleCharDB;

    public bool isSelected = false;

    public void Init(List<int> checkId = null)
    {
        battleCharDB = DataBaseManager.Instance.battleCharacterDB.Get(charId, false);
        if(checkId != null)
        {
            SetDeckIcon(CheckId(checkId));
        }

        charName.text = battleCharDB.name;
    }

    public void SetDeckIcon(bool selected)
    {
        isSelected = selected;

        charIcon.color = isSelected ? Color.gray : Color.white;
    }

    public bool CheckId(List<int> checkId)
    {
        return checkId.Contains(battleCharDB.id);
    }
}