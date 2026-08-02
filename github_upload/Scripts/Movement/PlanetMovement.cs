using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public float speed;
    private float y;
    public RectTransform endTarget;
    public RectTransform startTarget;
    public bool re;
    public void Start()
    {
        y = GetComponent<RectTransform>().localPosition.y;
    }
    void Update()
    {
        if (!re)
        {
            if (GetComponent<RectTransform>().localPosition.y > endTarget.localPosition.y)
            {
                y -= Time.deltaTime * speed;
                GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, y, GetComponent<RectTransform>().localPosition.z);
            }
            else
            {
                y = startTarget.localPosition.y;
                GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, y, GetComponent<RectTransform>().localPosition.z);
            }
        }
        else
        {
            if (GetComponent<RectTransform>().localPosition.y < endTarget.localPosition.y)
            {
                y += Time.deltaTime * speed;
                GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, y, GetComponent<RectTransform>().localPosition.z);
            }
            else
            {
                y = startTarget.localPosition.y;
                GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, y, GetComponent<RectTransform>().localPosition.z);
            }
        }
    }
}
