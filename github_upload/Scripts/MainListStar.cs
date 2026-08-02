using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainListStar : MonoBehaviour
{
    public float lerpSpeed = 0.5f; // 알파값 변경 속도

    private float alpha;
    private Color newColor;

    private void Start()
    {
        newColor = Color.white;
        alpha = GetComponent<Image>().color.a;
        StartCoroutine(LerpAlpha());
    }

    private IEnumerator LerpAlpha()
    {
        while (GetComponent<Image>().color.a > 0)
        {
            alpha -= Time.deltaTime * lerpSpeed;
            newColor.a = alpha;
            GetComponent<Image>().color = newColor;
            yield return null;
        }

        while (GetComponent<Image>().color.a < 1)
        {
            alpha += Time.deltaTime * lerpSpeed;
            newColor.a = alpha;
            GetComponent<Image>().color = newColor;
            yield return null;
        }
        yield return StartCoroutine(LerpAlpha());
    }
}
