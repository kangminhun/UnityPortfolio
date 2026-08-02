using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SetParent : MonoBehaviour
{
    public float speed;
    public float x;
    private float index;
    void Update()
    {
        index -= Time.deltaTime * speed;
        transform.localRotation = Quaternion.Euler(x, 0, index);
    }
}
