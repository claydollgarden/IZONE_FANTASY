using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float fScaleWidth = (float)Screen.height / 2;
        Vector3 vecButtonPos = GetComponent<RectTransform>().localPosition;
        vecButtonPos.y = -fScaleWidth;
        GetComponent<RectTransform>().localPosition = new Vector3 (vecButtonPos.x, vecButtonPos.y, vecButtonPos.z);
    }
}
