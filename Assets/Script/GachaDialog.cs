using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaDialog : MonoBehaviour
{
    public Text PriceText;
    public Text CategoryText;
    public Text CountText;

    public GameObject confirmButton;

    public void Start()
    {
    }

    public void SetActive(bool flg)
    {
        gameObject.SetActive(flg);
    }

    public void SetDialog(int gachaCount, int price, int gachaCategory)
    {
        PriceText.text = price.ToString() + "gold를 소비하여";

        string category;

        switch(gachaCategory)
        {
            case 1:
                category = "COLORIZ";
                break;
            case 2:
                category = "Sukito";
                break;
            case 3:
                category = "HEARTIZ";
                break;
            default:
                category = "COLORIZ";
                break;
        }

        CategoryText.text = category + "뽑기를";
        CountText.text = gachaCount.ToString() + "번 뽑으시겠습니까";

        SetActive(true);
        confirmButton.SetActive(true);
    }

    public void SetNoMoneyDialog()
    {
        PriceText.text = "";
        CategoryText.text = "돈이 부족합니다";
        CountText.text = "";

        SetActive(true);
        confirmButton.SetActive(false);
    }
}
