using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "S2VocaGame_Infomation", menuName = "S2VocaGame_Infomation")]
public class S2VocaGame_Infomation : ScriptableObject
{
    public int[] myids;
    public string[] buttontxts;
    public AudioClip[] effects;
    public AudioClip[] talkClips;
    public string[] description;
}
