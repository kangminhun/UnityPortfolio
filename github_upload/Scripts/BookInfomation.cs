using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="book",menuName ="bookinfomation")]
public class BookInfomation : ScriptableObject
{
    public Sprite[] bookPage;
    public Sprite[] answerPage;
    public int[] answerPageNumbers;
}
