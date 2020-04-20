﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCharInfoPanel : MonoBehaviour
{
    public Image thumbnailImage;

    public Text nameText;
    public Text atkText;
    public Text defText;
    public Text speedText;
    public Text descText;

    public Text skillName1;
    public Text skillPower1;
    public Text skillDesc1;

    public Text skillName2;
    public Text skillPower2;
    public Text skillDesc2;

    public Text skillName3;
    public Text skillPower3;
    public Text skillDesc3;

    public void SetActiveObject(bool flg)
    {
        gameObject.SetActive(flg);
    }

    public void SetData(int battleCharNumber)
    {
        var sprite = VResourceLoad.Load<Sprite>("CharThumbnail/" + battleCharNumber.ToString());

        thumbnailImage.sprite = sprite;

        var charDB = DataBaseManager.Instance.battleCharacterDB.Get(battleCharNumber, false);
        nameText.text = charDB.name;
        atkText.text = charDB.atk.ToString();
        defText.text = charDB.def.ToString();
        descText.text = charDB.description;
        speedText.text = charDB.speed.ToString();

        var charSkillDB = DataBaseManager.Instance.battleEnemySkillDB.Get(charDB.skill[0]);

        skillName1.text = charSkillDB.name;
        skillPower1.text = (charSkillDB.atk * 0.01f).ToString() + "배";
        skillDesc1.text = charSkillDB.desc;

        charSkillDB = DataBaseManager.Instance.battleEnemySkillDB.Get(charDB.skill[1]);

        skillName2.text = charSkillDB.name;
        skillPower2.text = (charSkillDB.atk * 0.01f).ToString() + "배";
        skillDesc2.text = charSkillDB.desc;

        charSkillDB = DataBaseManager.Instance.battleEnemySkillDB.Get(charDB.skill[2]);

        skillName3.text = charSkillDB.name;
        skillPower3.text = (charSkillDB.atk * 0.01f).ToString() + "배";
        skillDesc3.text = charSkillDB.desc;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}