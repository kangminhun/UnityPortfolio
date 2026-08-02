using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyTxt : MonoBehaviour
{
    public Text[] texts;
    public Text[] copyTexts;
    public void Set()
    {
        for (int i = 0; i < copyTexts.Length; i++)
        {
            int sum = i;
            copyTexts[sum].text = texts[sum].text;
        }
    }
}
