using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Game2Quiz", menuName = "Game2Quiz")]
public class ReviewGame2Quiz : ScriptableObject
{
    public string answer;
    public Sprite mySprite;
    public RuntimeAnimatorController animator;
}
