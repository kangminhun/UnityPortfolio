using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPointionY : MonoBehaviour
{
    public GameObject target;
    private Vector3 myTransform;
    private float myTransformY;
    // Update is called once per frame
    private void Start()
    {
        myTransformY = GetComponent<RectTransform>().localPosition.y - target.GetComponent<RectTransform>().localPosition.y;
    }
    void Update()
    {
        myTransform.y = target.GetComponent<RectTransform>().localPosition.y;
        GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, myTransform.y + myTransformY, 0);
    }
}
