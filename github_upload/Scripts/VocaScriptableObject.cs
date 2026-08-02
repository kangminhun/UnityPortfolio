using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Voca",fileName ="voca")]
public class VocaScriptableObject : ScriptableObject
{
    public string[] exampleStringDatas;
    public string explanationStringData;
    public AudioClip[] audioClips;
    public int type;
}
