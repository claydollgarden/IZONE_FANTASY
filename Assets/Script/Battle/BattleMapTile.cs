using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapTile : MonoBehaviour
{
    public int mapId;
    public BoxCollider2D mapCollider;
    public GameObject activeSelectImage;
    public GameObject targetImage;

    public void SetActiveSelectImage(bool flg)
    {
        activeSelectImage.SetActive(flg);
    }

    public void SetActiveTargetImage(bool flg)
    {
        targetImage.SetActive(flg);
    }

}
