using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    public CardGameEnemyInfomation[] infomations;
    public CardGameEnemyInfomation[] infomations_boss;
    public MyCardGame cardGame;
    public MyCardGame bossGame;

    public GameObject roundGame;
    public GameObject boss;

    public Image stageImg;
    public Image roundNumber;
    public Image backgroundImg;
    public Image round_vsImage;
    public Image boss_vsImage;
    public Image roundBackgound;
    public Image bossBackgound;
    public Sprite[] roundNumberImgs;
    public Sprite[] stageImgs;
    public Sprite[] backgroundImgs;
    public Sprite[] vsImages;
    public Sprite[] mainBackgoundImgs;
    public Sprite[] mainBackgoundImgs_boss;
    public Sprite[] bossRoundNumber;

    public GameObject startUi;
    public void Mode(string str)
    {
        if(str == "boss")
        {
            bossGame.clearImg.SetActive(false);
            roundGame.SetActive(false);
            boss.SetActive(true);
        }
        else
        {
            roundGame.SetActive(true);
            boss.SetActive(false);
        }
    }
    public void GameSet(int num)
    {
        if (DataBase.instance.CardGameRoundManager.myRound_Index == 6)
        {
            DataBase.instance.CardGameRoundManager.myRound_Index -= 1;
            DataBase.instance.CardGameRoundManager.roundScore = num % 5;
            roundNumber.sprite = roundNumberImgs[num];
            stageImg.sprite = stageImgs[num / 5];
            backgroundImg.sprite = backgroundImgs[num / 5];
            round_vsImage.sprite = vsImages[num / 5];
            roundBackgound.sprite = mainBackgoundImgs[num / 5];

            startUi.SetActive(true);
            gameObject.SetActive(true);
            cardGame.enemyCardList = infomations[num].items;
            cardGame.GameStart();
        }
        else
        {
            DataBase.instance.CardGameRoundManager.roundScore = num % 5;
            roundNumber.sprite = roundNumberImgs[num];
            stageImg.sprite = stageImgs[num / 5];
            backgroundImg.sprite = backgroundImgs[num / 5];
            round_vsImage.sprite = vsImages[num / 5];
            roundBackgound.sprite = mainBackgoundImgs[num / 5];

            startUi.SetActive(true);
            gameObject.SetActive(true);
            cardGame.enemyCardList = infomations[num].items;
            cardGame.GameStart();
        }
    }
    public void BossSet(int num)
    {
        DataBase.instance.CardGameRoundManager.roundScore = 10;
        roundNumber.sprite = bossRoundNumber[num];
        stageImg.sprite = stageImgs[num];
        backgroundImg.sprite = backgroundImgs[num];
        boss_vsImage.sprite = vsImages[num];
        bossBackgound.sprite = mainBackgoundImgs_boss[num];

        startUi.SetActive(true);
        gameObject.SetActive(true);
        bossGame.enemyCardList = infomations_boss[num].items;
        bossGame.GameStart();
    }
    public void StartUiOFF()
    {
        startUi.SetActive(false);
    }
    public void End()
    {
        gameObject.SetActive(false);
    }
}
