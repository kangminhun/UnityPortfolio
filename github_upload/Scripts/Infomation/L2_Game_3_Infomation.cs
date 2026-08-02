using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_3_Infomation", menuName = "Game_3_Infomation")]
public class L2_Game_3_Infomation : ScriptableObject
{
    public ReviewGame1Quiz[] quiz;
    public Sprite[] buttonsImg;
    public AudioClip[] effects;
}
