using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Information", menuName ="ListButton")]
public class ListButtonInformation : ScriptableObject
{
    public Sprite mainUi;
    public Sprite[] unitButtonImgs;
    public AudioClip clip;
    public int[] openUnitNumbers;
}
