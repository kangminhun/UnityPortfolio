using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game 1 v2",fileName ="info")]
public class L2_Game_1_v2_Infomation : ScriptableObject
{
    public Sprite[] Words_Image_Sprite;
    public Sprite[] wordSprites;
    public Sprite[] iamges;
    public AudioClip[] clips;

    [Space(10)]
    public AudioClip[] alphabetClips;
    public AudioClip[] quizSound;
    public Sprite[] alphabets;
    public Sprite[] quizImages;

    [Space(10)]
    public bool reverse;
    public bool mixdata;
    public List<int> mixNumber;
    public Sprite[] quizChildSprites;
}
