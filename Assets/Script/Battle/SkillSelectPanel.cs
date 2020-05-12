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

    public void SkillCoolTimeSet()
    {
        for (int i = 0; i < skillCards.Length; i++)
        {
            if(skillCards[i].skillData.cooltime != 0)
            {
                skillCards[i].skillData.cooltime--;
                skillCards[i].SetSkillCoolTime(skillCards[i].skillData.cooltime);
            }
        }
    }
}
