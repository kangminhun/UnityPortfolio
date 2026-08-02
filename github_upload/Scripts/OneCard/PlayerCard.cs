using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    public int opponentCardValue;
    public Image opponentCardImg;
    public Sprite[] opponentCardSprites;
    public Transform diePoint;
    public Transform waitPoint;
    public Transform contentPoint;
    public GameObject[] cards;
    public List<OneCardVelue> oneCardList = new List<OneCardVelue>();
    public Gamemanager manager;
    [HideInInspector]
    public bool draging;
    private int gameRound;
    List<int> numberList = new List<int>();
    public void Shuffle()
    {
        manager.InitializeOneCardHint();
        gameRound = 0;
        numberList.Clear();
        for (int i = 0; i < 4;)
        {
            opponentCardValue = Random.Range(1, 5);
            if(!numberList.Contains(opponentCardValue))
            {
                numberList.Add(opponentCardValue);
                i++;
            }
        }
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<OneCardVelue>().cardStage = OneCardVelue.stage.Idle;
            cards[i].gameObject.GetComponent<RectTransform>().SetParent(contentPoint);
        }
        Initialize();
    }
    public void ImgeSetting(int num)
    {
        opponentCardImg.sprite = opponentCardSprites[numberList[num] - 1];
    }
    public void Initialize()
    {
        opponentCardValue = numberList[gameRound];
        ImgeSetting(gameRound);
    }
    public void HintClick()
    {
        bool use = false;
        for (int i = 0; i < cards.Length; i++)
        {
            if(cards[i].GetComponent<OneCardVelue>().cardValue == opponentCardValue)
            {
                if(cards[i].GetComponent<OneCardVelue>().cardStage == OneCardVelue.stage.Idle&& !use)
                {
                    StartCoroutine(Hint(cards[i].GetComponent<OneCardVelue>()));
                    use = true;
                }
            }
        }
    }
    public void ChooseCard()
    {
        List<OneCardVelue> c = new List<OneCardVelue>();
        int num = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<OneCardVelue>().cardValue == opponentCardValue)
            {
                num++;
                if (cards[i].GetComponent<OneCardVelue>().cardStage == OneCardVelue.stage.Down)
                    c.Add(cards[i].GetComponent<OneCardVelue>());
            }
            else
            {
                if (cards[i].GetComponent<OneCardVelue>().cardStage == OneCardVelue.stage.Down)
                    cards[i].GetComponent<OneCardVelue>().cardStage = OneCardVelue.stage.Idle;
            }
        }
        oneCardList = c;
        if (c.Count==num && num!=0)
        {
            CardSelectionSuccessful(c);
        }
        else if(num != 0)
        {
            CardSelectionWaiting(c);
        }
    }
    public void CardSelectionSuccessful(List<OneCardVelue> c)
    {
        for (int i = 0; i < c.Count; i++)
        {
            c[i].GetComponent<OneCardVelue>().cardStage = OneCardVelue.stage.Out;
            c[i].gameObject.GetComponent<RectTransform>().SetParent(diePoint);
            c[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
            c[i].GetComponent<OneCardVelue>().enabled = false;
        }
        if (gameRound < 3)
        {
            gameRound++;
            Initialize();
        }
        else
        {
            //StartCoroutine(manager.Before_Success(3));
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].GetComponent<OneCardVelue>().cardStage = OneCardVelue.stage.Idle;
                cards[i].gameObject.GetComponent<RectTransform>().SetParent(contentPoint);
                cards[i].GetComponent<OneCardVelue>().enabled = true;
            }
        }
    }
    public void CardSelectionWaiting(List<OneCardVelue> c)
    {
        for (int i = 0; i < c.Count; i++)
        {
            c[i].gameObject.GetComponent<RectTransform>().SetParent(waitPoint);
            c[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
    }
    public IEnumerator Hint(OneCardVelue c)
    {
        c.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSeconds(0.2f);
        c.GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.2f);
        c.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSeconds(0.2f);
        c.GetComponent<Image>().color = Color.white;
    }
    public void OnDrag()
    {
        opponentCardImg.GetComponent<Animator>().enabled = true;
    }
    public void OffDrag()
    {
        opponentCardImg.GetComponent<Animator>().enabled = false;
        opponentCardImg.GetComponent<Animator>().Rebind();
    }
}
