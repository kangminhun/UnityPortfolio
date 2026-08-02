using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_Game_2_Manager : MonoBehaviour
{
    public L2Game2 game2;
    public L2_Game_2_Infomation[] game_2_Infomations;
    public void Setting(int num)
    {
        game2.titlImgs = game_2_Infomations[num].tileImgs;
        game2.sentencetxts = game_2_Infomations[num].sentencetxts;
        game2.answers = game_2_Infomations[num].answers;
        game2.sentenceClips = game_2_Infomations[num].audioClips;
        game2.GameSet();
    }
}
