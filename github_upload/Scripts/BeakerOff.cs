using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerOff : MonoBehaviour
{
    public GameObject beaker;
    public void Off()
    {
        beaker.SetActive(false);
    }
    public void On()
    {
        beaker.SetActive(true);
    }
}
