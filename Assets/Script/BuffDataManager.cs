using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDataManager : MonoBehaviour
{
    public List<SingleBuffDB> currentBuffDB = new List<SingleBuffDB>();
    public List<SingleBuffDB> activeBuffDBList = new List<SingleBuffDB>();

    public void SetBuffDataBase()
    {
        currentBuffDB = DataBaseManager.Instance.singleBuffDB.GetAll();

        CleanCheckcount();
    }

    public void CleanCheckcount()
    {
        foreach (SingleBuffDB targetList in currentBuffDB)
        {
            targetList.checkcount = 0;
        }

        for (int i = activeBuffDBList.Count - 1; i >= 0; i--)
        {
            activeBuffDBList.RemoveAt(i);
        }
    }

    public void CheckBuffCharNumber(int charNumber)
    {
        foreach (SingleBuffDB targetList in currentBuffDB )
        {
            for (int i = 0; i < targetList.target.Length; i++)
            {
                if (charNumber == targetList.target[i])
                {
                    targetList.checkcount++;
                }
            }
        }
    }

    public void DisCountNumber(int charNumber)
    {
        foreach (SingleBuffDB targetList in currentBuffDB)
        {
            for (int i = 0; i < targetList.target.Length; i++)
            {
                if (charNumber == targetList.target[i])
                {
                    targetList.checkcount--;
                }
            }
        }
    }

    public List<SingleBuffDB> GetCurrentBuffList()
    {
        foreach (SingleBuffDB targetList in currentBuffDB)
        {
            if (targetList.checkcount == targetList.count)
            {
                Debug.Log("targetList : " + targetList.name);
                activeBuffDBList.Add(targetList);
            }
        }

        return activeBuffDBList;
    }

    public bool CheckSameBuff(SingleBuffDB checkBuff)
    {
        for(int i = 0; i < activeBuffDBList.Count; i++)
        {
            if(activeBuffDBList[i].id == checkBuff.id)
            {
                return true;
            }
        }
        return false;
    }
}
