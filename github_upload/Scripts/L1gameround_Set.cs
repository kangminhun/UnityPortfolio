using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set", menuName = "L1GameSet")]
public class L1gameround_Set : ScriptableObject
{
    public Sprite[] cardSet;

    public Sprite[] puzzleSet_1Round3x3;
    public Sprite puzzleSet_1RoundOriginImg;
    public Sprite puzzleSet_1RoundOriginTxt;

    public Sprite[] puzzleSet_2Round3x3;
    public Sprite puzzleSet_2RoundOriginImg;
    public Sprite puzzleSet_2RoundOriginTxt;

    public Sprite[] puzzleSet_3Round3x3;
    public Sprite puzzleSet_3RoundOriginImg;
    public Sprite puzzleSet_3RoundOriginTxt;

    public Sprite[] puzzleSet_4Round3x3;
    public Sprite puzzleSet_4RoundOriginImg;
    public Sprite puzzleSet_4RoundOriginTxt;

    public string[] g1_TypingTxts;
    public AudioClip[] g1_TypingSounds;

    public string[] g2_TypingTxts;
    public AudioClip[] g2_TypingSounds;
}
