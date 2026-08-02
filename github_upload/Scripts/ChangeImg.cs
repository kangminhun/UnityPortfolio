using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImg : MonoBehaviour
{
    public Image mainImg;
    public Sprite[] sprites;
    private int Imgindex;
    public void OnEnable()
    {
        Imgindex = 0;
        mainImg.sprite = sprites[0];
    }
    public void Next()
    {
        if (Imgindex < sprites.Length - 1)
        {
            Imgindex++;
            mainImg.sprite = sprites[Imgindex];
        }
        else
            return;
    }
    public void Previous()
    {
        if (Imgindex > 0)
        {
            Imgindex--;
            mainImg.sprite = sprites[Imgindex];
        }
        else
            return;
    }
}
