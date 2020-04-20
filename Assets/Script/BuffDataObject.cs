using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffDataObject : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Image iconImage;
    public Image backGround;
    public Text buffName;
    public Text buffDesc;

    public SingleBuffDB buffData;

    enum StatusName : byte
    {
        HP = 0,
        ATK,
        DEF,
        SPD
    }

    public void InitBuffDataObject(SingleBuffDB getBuffData)
    {
        buffData = getBuffData;

        iconImage.sprite = VResourceLoad.Load<Sprite>("BuffIcon/" + buffData.iconpath);
        buffName.text = buffData.name;

        string descText;

        StatusName status = (StatusName)buffData.status;
        descText = status.ToString();

        descText = descText + " : +" + (buffData.effect % 100).ToString() + "%" ;

        buffDesc.text = descText;

        ActiveDescThumbnail(false);
    }

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {
        ActiveDescThumbnail(true);
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        ActiveDescThumbnail(false);
    }

    public void ActiveDescThumbnail(bool flg)
    {
        backGround.enabled = flg;
        buffName.enabled = flg;
        buffDesc.enabled = flg;
    }

    public void DeleteObject()
    {
        Destroy(this);
    }
}
