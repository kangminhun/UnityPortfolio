using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2_Game_1_Manager : MonoBehaviour
{
    public L2Game1 game1;
    public L2_Game_1_Infomation[] infomation;
    public void Setting(int num)
    {
        game1.aButton.GetComponent<Image>().sprite = infomation[num].buttonsImg[0];
        game1.bButton.GetComponent<Image>().sprite = infomation[num].buttonsImg[1];
        game1.quizs = infomation[num].quiz;
        for (int i = 0; i < game1.words.Length; i++)
        {
            game1.words[i].GetComponent<Image>().sprite = infomation[num].wordsImg[i];
            for (int j = 0; j < game1.words[i].transform.childCount; j++)
            {
                game1.words[i].transform.GetChild(j).gameObject.GetComponent<Image>().sprite = infomation[num].wordChildsimg[i];
            }
        }
        game1.effects = infomation[num].effects;
        game1.gameObject.SetActive(true);
    }
}
