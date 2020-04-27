using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaDialog : MonoBehaviour
{
    public Text PriceText;

    public Text CountText;

    public void Start()
    {
    }

    public void SetActive(bool flg)
    {
        gameObject.SetActive(flg);
    }

    public void SetDialog(int gachaCount, int price)
    {
        Debug.Log("SetDialog");
        PriceText.text = price.ToString() + "gold를 소비하여";
        CountText.text = gachaCount.ToString() + "연차를 하시겠습니까";

        SetActive(true);
    }
}
