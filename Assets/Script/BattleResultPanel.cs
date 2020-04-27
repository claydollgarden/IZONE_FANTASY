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

    public void SetItemIcon(List<int> battleItemIds, int gold,int exp)
    {
        goldText.text = gold.ToString();
        expText.text = exp.ToString();

        foreach (int itemId in battleItemIds)
        {
            BattleItemIcon itemObj = Instantiate(itemPrefab);
            itemObj.transform.SetParent(itemViewContent.transform, false);

            var itemIconDB = DataBaseManager.Instance.itemDB.Get(itemId);
            itemObj.iconImage.sprite = VResourceLoad.Load<Sprite>("UI/" + itemIconDB.imagepath);
        }
    }
}
