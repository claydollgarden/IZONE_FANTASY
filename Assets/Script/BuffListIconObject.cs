using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffListIconObject : MonoBehaviour
{
    public Image buffImage;
    public Text name;
    public Text effectPower;
    public Text desc;
    public Text illustname;

    enum StatusName : byte
    {
        HP = 0,
        ATK,
        DEF,
        SPD
    }

    // Start is called before the first frame update
    public void SetIcon(SingleBuffDB buffDB)
    {
        buffImage.sprite = VResourceLoad.Load<Sprite>("BuffIcon/" + buffDB.iconpath);
        name.text = buffDB.name;

        string descText;

        StatusName status = (StatusName)buffDB.status;
        descText = status.ToString();

        descText = descText + " : +" + (buffDB.effect % 100).ToString() + "%";

        effectPower.text = descText;

        desc.text = buffDB.description;
        illustname.text = buffDB.illust;









    }
}
