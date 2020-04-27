using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour
{
    public Animator charAnimator;
    public SpriteRenderer charImage;
    public SpriteRenderer hpImage;

    public BoxCollider2D charCollider;
    public int charId;
    public int charPosition;
    public bool playerSide;

    public string charName;
    public int hp;
    public int atk;
    public int def;
    public int speed;
    public int charnumber;

    public int currentHp;

    public int charExp;
    public int charGold;
    public int itemNumber;

    public bool isAlive = true;

    public List<BattleEnemySkillDB> battleEnemySkills = new List<BattleEnemySkillDB>();

    enum BuffStatus : byte
    {
        HP = 0,
        ATK,
        DEF,
        SPD
    }

    public void enemyCharInit(Sprite image, int charNumber)
    {
        charImage.sprite = image;
        charId = charNumber;
        charCollider.enabled = false;
        hpImage.transform.localPosition = new Vector3(1.5f, 0, 0);

        var charDB = DataBaseManager.Instance.battleEnemyStatusDB.Get(charId, false);

        charName = charDB.name;
        hp = charDB.hp;
        currentHp = hp;
        atk = charDB.atk;
        def = charDB.def;
        speed = charDB.speed;
        playerSide = false;
        charExp = charDB.exp;
        charGold = Random.Range(charDB.gold[0], charDB.gold[1]);

        if(Random.Range(0, 100) < charDB.itempercent)
        {
            itemNumber = charDB.item;
        }
        else
        {
            itemNumber = 0;
        }

        Debug.Log("itemNumber : " + itemNumber);

        isAlive = SetCurrentHP(0);

        for (int i = 0; i < charDB.enemyskill.Length; i++)
        {
            battleEnemySkills.Add(DataBaseManager.Instance.battleEnemySkillDB.Get(charDB.enemyskill[i]));
        }
    }

    public void playerCharInit(Sprite image, int charNumber)
    {
        charImage.sprite = image;
        charId = charNumber;
        charCollider.enabled = false;
        hpImage.transform.localPosition = new Vector3(-1.5f, 0, 0);

        var charDB = DataBaseManager.Instance.battleCharacterDB.Get(charId, false);

        int level = (GameManager.Instance.userData.myCharactersList[charId] / 150);

        charName = charDB.name;
        hp = ((charDB.hp * (10 + level)) / 10);
        currentHp = hp;
        atk = ((charDB.atk * (10 + level)) / 10);
        def = ((charDB.def * (10 + level)) / 10);
        speed = ((charDB.speed * (10 + level)) / 10);
        charnumber = charDB.namenumber;
        playerSide = true;

        isAlive = SetCurrentHP(0);

        for (int i = 0; i < charDB.skill.Length; i++)
        {
            battleEnemySkills.Add(DataBaseManager.Instance.battleEnemySkillDB.Get(charDB.skill[i]));
        }
    }

    public void SetBuffStatus(int buffNumber, int buffPower)
    {
        switch ((BuffStatus)buffNumber)
        {
            case BuffStatus.HP:
                hp = hp * buffPower / 100;
                currentHp = hp;
                break;
            case BuffStatus.ATK:
                atk = atk * buffPower / 100;
                break;
            case BuffStatus.DEF:
                def = def * buffPower / 100;
                break;
            case BuffStatus.SPD:
                speed = speed * buffPower / 100;
                break;
        }
    }

    public void ActiveMaterial(bool flg)
    {
        charImage.material.SetFloat("_Outline", flg ? 1f : 0);
        charImage.material.SetColor("_OutlineColor", Color.green);
        charImage.material.SetFloat("_OutlineSize", flg ? 20.0f : 0.0f);
    }

    public bool SetCurrentHP(int DamaegeCount)
    {
        currentHp = currentHp - DamaegeCount;

        if (currentHp > 0)
        {
            hpImage.transform.localScale = new Vector3 ((float)currentHp / hp, 1 , 1);
            return true;
        }
        else
        {
            PlayCharacterAnimation("BattleCharacterDie");
            isAlive = false;
        }

        return false;
    }

    public void PlayCharacterAnimation(string animationName)
    {
        charAnimator.Play(animationName);
    }

    public void StartJumpChar(Vector3 start, Vector3 end)
    {
        float margin;
        if (playerSide)
        {
            margin = -2.0f;
        }
        else
        {
            margin = 2.0f;
        }

        var endPosition = new Vector3(end.x + margin, end.y, end.z);
        StartCoroutine(JumpCoroutine(start, endPosition));
    }

    public void EndJumpChar(Vector3 start, Vector3 end)
    {
        float margin;
        if (playerSide)
        {
            margin = -2.0f;
        }
        else
        {
            margin = 2.0f;
        }

        var startPosition = new Vector3(start.x + margin, start.y, start.z);

        StartCoroutine(JumpCoroutine(startPosition, end));
    }

    public IEnumerator JumpCoroutine(Vector3 start, Vector3 end)
    {
        float time = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            var t = elapsedTime / time;
            var res = Vector3.Lerp(start, end, t);
            res.y += Mathf.Sin(t * Mathf.PI);
            transform.position = res;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
