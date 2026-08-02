using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_1_Infomation",menuName = "Game_1_Infomation")]
public class L2_Game_1_Infomation : ScriptableObject
{
    public ReviewGame1Quiz[] quiz;
    public Sprite[] wordsImg;
    public Sprite[] wordChildsimg;
    public Sprite[] buttonsImg;
    public AudioClip[] effects;
}
