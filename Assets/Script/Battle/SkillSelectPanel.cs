using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectPanel : MonoBehaviour
{
    public SkillCard[] skillCards;

    public void activeSkillPanel(bool flg)
    {
        this.gameObject.SetActive(flg);
    }

    public void ActiveSkillCards(bool flg)
    {
        for (int i = 0; i < skillCards.Length; i++)
        {
            skillCards[i].SelectedCard(flg);
        }

    }
}
