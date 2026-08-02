using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_Game_3_Manager : MonoBehaviour
{
    public L2Game3 game3;
    public L2_Game_3_Infomation[] infomation;
    public void Setting(int num)
    {
        game3.buttonImgs = infomation[num].buttonsImg;
        game3.reviewGame = infomation[num].quiz;
        game3.effects = infomation[num].effects;
        game3.gameObject.SetActive(true);
    }
}
