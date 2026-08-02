using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_Game_1_v2_Manager : MonoBehaviour
{
    public L2_Game_1_v2_Infomation[] game_1_V2_Infomations;
    public L2Game1_v2 game1_V2;
    public L2_U4_Game_1 u4_Game_1;
    public L2_U10_Game_1 u10_Game_1;
    public void Setting(int num)
    {
        if (game1_V2 != null)
        {
            game1_V2.words_Image_Sprite = game_1_V2_Infomations[num].Words_Image_Sprite;
            game1_V2.wordSprites = game_1_V2_Infomations[num].wordSprites;
            game1_V2.imagesSprites = game_1_V2_Infomations[num].iamges;
            game1_V2.effects = game_1_V2_Infomations[num].clips;
            game1_V2.GameSet();
        }
        else if (u4_Game_1 != null)
        {
            u4_Game_1.quizSprites = game_1_V2_Infomations[num].quizImages;
            u4_Game_1.wordSprite = game_1_V2_Infomations[num].wordSprites;
            u4_Game_1.alphabetSprites = game_1_V2_Infomations[num].alphabets;
            u4_Game_1.imagesSprites = game_1_V2_Infomations[num].iamges;
            u4_Game_1.wordEffects = game_1_V2_Infomations[num].clips;
            u4_Game_1.alphabetEffects = game_1_V2_Infomations[num].alphabetClips;
            u4_Game_1.quizSound = game_1_V2_Infomations[num].quizSound;
            u4_Game_1.GameSet();
        }
        else if (u10_Game_1 != null)
        {
            if(!game_1_V2_Infomations[num].mixdata)
            {
                u10_Game_1.mixNumber = new List<int>();
            }
            else
            {
                u10_Game_1.mixNumber = game_1_V2_Infomations[num].mixNumber;
                u10_Game_1.quizChildSprites = game_1_V2_Infomations[num].quizChildSprites;
            }
            u10_Game_1.reverse = game_1_V2_Infomations[num].reverse;
            u10_Game_1.quizSprites = game_1_V2_Infomations[num].quizImages;
            u10_Game_1.wordSprite = game_1_V2_Infomations[num].wordSprites;
            u10_Game_1.alphabetSprites = game_1_V2_Infomations[num].alphabets;
            u10_Game_1.imagesSprites = game_1_V2_Infomations[num].iamges;
            u10_Game_1.wordEffects = game_1_V2_Infomations[num].clips;
            u10_Game_1.alphabetEffects = game_1_V2_Infomations[num].alphabetClips;
            u10_Game_1.quizSound = game_1_V2_Infomations[num].quizSound;
            u10_Game_1.GameSet();
        }
    }
}
