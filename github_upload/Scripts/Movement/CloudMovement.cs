using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed;
    private float x;
    public RectTransform endTarget;
    public RectTransform startTarget;
    public void Start()
    {
        x = GetComponent<RectTransform>().localPosition.x;
    }
    void Update()
    {
        if (speed > 0)
        {
            if (GetComponent<RectTransform>().localPosition.x < endTarget.localPosition.x)
            {
                x += Time.deltaTime * speed;
                GetComponent<RectTransform>().localPosition = new Vector3(x, GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);
            }
            else
            {
                x = startTarget.localPosition.x;
                GetComponent<RectTransform>().localPosition = new Vector3(startTarget.localPosition.x, GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);
            }
        }
        else if(speed < 0)
        {
            if (GetComponent<RectTransform>().localPosition.x > startTarget.localPosition.x)
            {
                x += Time.deltaTime * speed;
                GetComponent<RectTransform>().localPosition = new Vector3(x, GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);
            }
            else
            {
                x = endTarget.localPosition.x;
                GetComponent<RectTransform>().localPosition = new Vector3(endTarget.localPosition.x, GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);
            }
        }
    }
}
