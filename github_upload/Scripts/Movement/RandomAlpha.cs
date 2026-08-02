using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomAlpha : MonoBehaviour
{
    public Image[] imagesArray;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;
    public float minChangeDuration = 1f;
    public float maxChangeDuration = 5f;

    void Start()
    {
        foreach (Image image in imagesArray)
        {
            StartCoroutine(ChangeAlphaSmoothly(image));
        }
    }

    IEnumerator ChangeAlphaSmoothly(Image image)
    {
        while (true)
        {
            float targetAlpha = Random.Range(minAlpha, maxAlpha);
            float currentAlpha = GetCurrentAlpha(image);

            float changeDuration = Random.Range(minChangeDuration, maxChangeDuration);
            float startTime = Time.time;

            while (Time.time - startTime < changeDuration)
            {
                float t = (Time.time - startTime) / changeDuration;

                Color currentColor = image.color;
                currentColor.a = Mathf.Lerp(currentAlpha, targetAlpha, t);
                image.color = currentColor;

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(.5f, 1.5f)); // 변화 후 랜덤한 시간 대기
        }
    }

    float GetCurrentAlpha(Image image)
    {
        return image.color.a;
    }
}

