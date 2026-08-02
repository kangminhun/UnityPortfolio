using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public Gamemanager manager;
    public void HintClick()
    {
        gameObject.SetActive(false);
        manager.Hint();
    }
    public void InitializeHint()
    {
        gameObject.SetActive(true);
    }
}
