using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelImg : MonoBehaviour
{
    public float speed;
    private float index;
    void Update()
    {
        index += Time.deltaTime * speed;
        transform.localRotation = Quaternion.Euler(0, 0, index);
    }

}
