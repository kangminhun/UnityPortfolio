using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2VocaManager : MonoBehaviour
{
    public S2VocaInfomation[] s2Vocas;
    public GameObject[] imageStagePrefabs;
    public VocaScript voca;
    public void Setting(int num)
    {
        voca.imageStagePrefab = imageStagePrefabs[num];
        voca.vocas = s2Vocas[num].vocas;
        voca.names = s2Vocas[num].names;
        voca.GaemStart();
    }
}
