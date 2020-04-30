using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPanel : MonoBehaviour
{
    public Text goldText;
    public Text expText;

    public GameObject itemViewContent;

    public BattleItemIcon itemPrefab;

    public List<int> itemIcons = new List<int>();

    public void SetItemIcon(List<int> battleItemIds, int gold,int exp)
    {
        goldText.text = gold.ToString();
        expText.text = exp.ToString();

        if(itemIcons != battleItemIds)
        {
            for(int i = itemIcons.Count; i < battleItemIds.Count; i++)
            {
                BattleItemIcon itemObj = Instantiate(itemPrefab);
                itemObj.transform.SetParent(itemViewContent.transform, false);

                var itemIconDB = DataBaseManager.Instance.itemDB.Get(battleItemIds[i]);
                itemObj.iconImage.sprite = VResourceLoad.Load<Sprite>("UI/" + itemIconDB.imagepath);
            }

            itemIcons = battleItemIds;
        }
    }
}
