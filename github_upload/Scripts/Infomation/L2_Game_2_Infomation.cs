using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game_2_Infomation",fileName = "Infomation")]
public class L2_Game_2_Infomation : ScriptableObject
{
    public Sprite[] tileImgs;
    public string[] sentencetxts;
    public string[] answers;
    public AudioClip[] audioClips;
}
