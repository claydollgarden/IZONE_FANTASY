using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public BattleEnemySkillDB skillData;
    public SpriteRenderer skillIcon;
    public Text skillName;
    public Text skillDesc;

    public GameObject selectedCardImage;

    public void InitSkillCard(BattleEnemySkillDB skillDB)
    {
        this.gameObject.SetActive(true);
        skillData = skillDB;
        skillName.text = skillData.name;
        skillDesc.text = skillData.desc;

        SelectedCard(false);
    }

    public void SelectedCard(bool flg)
    {
        selectedCardImage.SetActive(flg);
    }
}
