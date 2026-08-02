using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigt_Left_Retun : MonoBehaviour
{
    public GameObject witch;
    public GameObject Bg;
    public RectTransform endTarget;
    public RectTransform startTarget;
    public float speed;
    private float x;
    private bool isMovingToEnd = true; // Flag to track the direction

    public void Update()
    {
        if (isMovingToEnd)
        {
            if (Bg.GetComponent<RectTransform>().localPosition.x < endTarget.localPosition.x)
            {
                x += Time.deltaTime * speed;
                Bg.GetComponent<RectTransform>().localPosition = new Vector3(x, Bg.GetComponent<RectTransform>().localPosition.y, Bg.GetComponent<RectTransform>().localPosition.z);
                witch.transform.rotation = Quaternion.Euler(0, 0, 0); // Rotate witch to 0 degrees when moving to the end point
            }
            else
            {
                isMovingToEnd = false;
                x = endTarget.localPosition.x;
                Bg.GetComponent<RectTransform>().localPosition = new Vector3(x, Bg.GetComponent<RectTransform>().localPosition.y, Bg.GetComponent<RectTransform>().localPosition.z);
            }
        }
        else
        {
            if (Bg.GetComponent<RectTransform>().localPosition.x > startTarget.localPosition.x)
            {
                x -= Time.deltaTime * speed;
                Bg.GetComponent<RectTransform>().localPosition = new Vector3(x, Bg.GetComponent<RectTransform>().localPosition.y, Bg.GetComponent<RectTransform>().localPosition.z);
                witch.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate witch to 180 degrees when moving to the start point
            }
            else
            {
                isMovingToEnd = true;
                x = startTarget.localPosition.x;
                Bg.GetComponent<RectTransform>().localPosition = new Vector3(x, Bg.GetComponent<RectTransform>().localPosition.y, Bg.GetComponent<RectTransform>().localPosition.z);
            }
        }
    }

}
