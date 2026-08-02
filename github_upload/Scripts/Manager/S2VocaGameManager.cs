using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2VocaGameManager : MonoBehaviour
{
    public S2VocaGame game3;
    public S2VocaGame_Infomation[] infomation;
    public GameObject[] quizPrefabs;
    public void Setting(int num)
    {
        game3.quizPrefab = quizPrefabs[num];
        game3.buttontxts = infomation[num].buttontxts;
        game3.ids = infomation[num].myids;
        game3.effectSoundClips = infomation[num].effects;
        game3.dialogue = infomation[num].description;
        game3.talkClips= infomation[num].talkClips;
        game3.gameObject.SetActive(true);
        game3.GameSet();
    }
}
