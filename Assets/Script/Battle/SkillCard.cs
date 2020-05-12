using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public BattleEnemySkillDB skillData;
    public SpriteRenderer skillIcon;
    public SpriteRenderer skillCardImage;
    public Text skillName;
    public Text skillDesc;
    public Text skillCoolTime;

    public GameObject selectedCardImage;

    public void InitSkillCard(BattleEnemySkillDB skillDB)
    {
        this.gameObject.SetActive(true);
        skillData = skillDB;
        skillName.text = skillData.name;
        skillDesc.text = skillData.desc;
        skillCoolTime.text = skillData.cooltime.ToString();

        if(skillData.cooltime != 0)
        {
            skillCardImage.color = Color.gray;
        }
        else
        {
            skillCardImage.color = Color.white;
        }

        SelectedCard(false);
    }

    public void SelectedCard(bool flg)
    {
        selectedCardImage.SetActive(flg);
    }

    public void SetSkillCoolTime(int time)
    {
        skillCoolTime.text = time.ToString();
    }
}
