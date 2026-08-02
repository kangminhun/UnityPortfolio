using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Vimeo.Player;

public class Gamemanager : MonoBehaviour
{
    public ListScroll listScroll;
    #region ī�����
    public Sprite[] cardFace;
    public L1gameround_Set[] cardSet;
    public Sprite cardBack;
    public GameObject[] cards;
    public GameObject cardParents;

    private int _matches = 4;
    [HideInInspector]
    public int roundNumber = 0;

    public AudioClip touchSound;
    public AudioSource gameAudio;


    public GameObject gamesUi;

    public Sprite[] puzzleHintImgs;
    public Hint[] cardHints;
    public Hint[] puzzleHints;
    public Hint[] oneCardHints;

    public GameObject[] puzzleBtns;

    [HideInInspector]
    public int hintCount;

    [SerializeField]
    private RectTransform point;
    [SerializeField]
    private Vector2[] rects;

    public VideoPlayer player;
    public string[] backgroundVideoUrls;
    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;

    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;

    public float timeLimit = 60f; // 1분(60초)
    private float currentTime;
    private bool isTimerRunning;
    public Text playTimeTxt;
    public Coroutine timeTextCoroutine;
    public GameObject fade;
    private bool ready;
    private IEnumerator Start()
    {
        rects = new Vector2[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            rects[i] = cards[i].GetComponent<RectTransform>().localPosition;
            cards[i].GetComponent<RectTransform>().localPosition = point.localPosition;
            cards[i].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);
        }
        gameAudio.clip = touchSound;
        yield return new WaitForEndOfFrame();
    }
    public void InitializeCards()
    {
        playTimeTxt.text = "60";
        timeTextCoroutine = StartCoroutine(TimerCoroutine());

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<RectTransform>().localPosition = rects[i];
            cards[i].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
            cards[i].GetComponent<Button>().enabled=true;
        }

        List<GameObject> value_1_Card = new List<GameObject>();
        List<GameObject> value_2_Card = new List<GameObject>();
        List<GameObject> value_3_Card = new List<GameObject>();
        List<GameObject> value_4_Card = new List<GameObject>();

        _matches = cards.Length / 2;
        List<int> numberList = new List<int>();
        for (int id = 0; id < 2; id++)
        {
            for (int i = 1; i < 5;)
            {

                int choice = 0;
                choice = Random.Range(0, cards.Length);
                if (!numberList.Contains(choice))
                {
                    numberList.Add(choice);
                    cards[choice].GetComponent<cardScript>().cardValue = i;
                    cards[choice].GetComponent<cardScript>().initialized = true;
                    i++;
                }
            }
        }
        foreach (GameObject c in cards)
        {
            switch (c.GetComponent<cardScript>().cardValue)
            {
                case 1:
                    value_1_Card.Add(c);
                    break;
                case 2:
                    value_2_Card.Add(c);
                    break;
                case 3:
                    value_3_Card.Add(c);
                    break;
                case 4:
                    value_4_Card.Add(c);
                    break;
            }
        }
        //Value 1��
        if (value_1_Card.Count == 2)
        {
            value_1_Card[0].GetComponent<cardScript>().setupGraphics(1);
            value_1_Card[1].GetComponent<cardScript>().setupGraphics(5);
        }

        //Value 2��
        if (value_2_Card.Count == 2)
        {
            value_2_Card[0].GetComponent<cardScript>().setupGraphics(2);
            value_2_Card[1].GetComponent<cardScript>().setupGraphics(6);
        }

        //Value 3��
        if (value_3_Card.Count == 2)
        {
            value_3_Card[0].GetComponent<cardScript>().setupGraphics(3);
            value_3_Card[1].GetComponent<cardScript>().setupGraphics(7);
        }

        //Value 4��
        if (value_4_Card.Count == 2)
        {
            value_4_Card[0].GetComponent<cardScript>().setupGraphics(4);
            value_4_Card[1].GetComponent<cardScript>().setupGraphics(8);
        }

        for (int i = 0; i < cardHints.Length; i++)
        {
            cardHints[i].InitializeHint();
        }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        switch (roundNumber)
        {
            case 0:
                return cardFace[i - 1]; //0~7
            case 1:
                return cardFace[(i - 1) + 8]; //8~15
            case 2:
                return cardFace[(i - 1) + 16]; //16~23
            case 3:
                return cardFace[(i - 1) + 24]; //24~31
        }
        return null;
    }

    public void checkCards()
    {
        List<int> c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<cardScript>().state == 0)
                c.Add(i);
        }

        if (c.Count == 2)
            cardComparison(c);
        gameAudio.Play();
    }

    void cardComparison(List<int> c)
    {
        cardScript.DO_NOT = true;

        int x = 1;

        if (cards[c[0]].GetComponent<cardScript>().cardValue == cards[c[1]].GetComponent<cardScript>().cardValue)
        {
            x = 2;
            _matches--;
            StartCoroutine(CardCollect(c[0], c[1]));
            if (_matches == 0)
            {
                isTimerRunning = false;
                StopCoroutine(timeTextCoroutine);
                SuccessAnimation();
            }
        }


        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<cardScript>().state = x;
            cards[c[i]].GetComponent<cardScript>().falseCheck();
        }
    }
    public void Hint()
    {
        if (gamesUi.GetComponent<Transform>().GetChild(0).gameObject.activeSelf)
        {
            List<GameObject> hintCard = new List<GameObject>();
            List<int> cardValue = new List<int>();
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].GetComponent<cardScript>().state == 1)
                {
                    hintCard.Add(cards[i]);
                    cardValue.Add(cards[i].GetComponent<cardScript>().cardValue);
                }
            }
            cardValue.Sort();

            StartCoroutine(HintCoroutine(hintCard, cardValue));
        }
        else if (gamesUi.GetComponent<Transform>().GetChild(1).gameObject.activeSelf)
        {
            GetComponent<PuzzleManager>().StartCoroutine(GetComponent<PuzzleManager>().GameStart());
        }
        else if(gamesUi.GetComponent<Transform>().GetChild(2).gameObject.activeSelf)
        {
            GetComponent<PlayerCard>().HintClick();
        }
        hintCount++;
    }
    public void StageClear()
    {
        StartCoroutine(UiChange(false, 9999));
    }
    public void StartStage(int index)
    {
        StartCoroutine(UiChange(true, index));
    }
    IEnumerator HintCoroutine(List<GameObject> cards, List<int> value)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].GetComponent<cardScript>().cardValue == value[0])
            {
                cards[i].GetComponent<cardScript>().HintCard();
            }
        }
        yield return null;
    }
    public IEnumerator UiChange(bool value, int index)
    {
        yield return StartCoroutine(UiChangAni(value, index));
    }
    IEnumerator UiChangAni(bool value, int index)
    {
        gamesUi.SetActive(value);
        if (index != 9999)
        {
            ChangeGame(index);
        }
        yield return new WaitForSeconds(0.7f);
    }

    IEnumerator CardCollect(int num_1, int num_2)
    {
        float time = 0;
        while (time < 1)
        {
            time += 1 * Time.deltaTime;
            cards[num_1].GetComponent<RectTransform>().localPosition = Vector2.Lerp(cards[num_1].GetComponent<RectTransform>().localPosition, point.localPosition, 5f * Time.deltaTime);
            cards[num_1].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);
            cards[num_2].GetComponent<RectTransform>().localPosition = Vector2.Lerp(cards[num_2].GetComponent<RectTransform>().localPosition, point.localPosition, 5f * Time.deltaTime);
            cards[num_2].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);

            yield return null;
        }
        // yield return new WaitForSeconds(5f);
    }
    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            playTimeTxt.text = ((int)(currentTime)).ToString();
            // 필요한 경우 UI를 currentTime으로 업데이트합니다
            if ((int)currentTime <= 10)
            {
                playTimeTxt.color = Color.red;
            }
            else
            {
                playTimeTxt.color = Color.white;
            }
            yield return null;
        }

        if (currentTime <= 0f)
        {
            // 타이머가 만료되면 실패 애니메이션 함수를 호출합니다
            FailureAnimation();
        }
    }
    public void SuccessAnimation()
    {
        // 성공 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 성공 이미지나 파티클 효과를 재생할 수 있습니다
        Debug.Log("성공");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        scene_FX_Confetti1.Play();
        StartCoroutine(GoldParticleOn());
        result.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        resultAudio.clip = resultAudioClips[0];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Success");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[0];
        resultButton.onClick.AddListener(() => BackButton());
        DataBase.instance.PointManager.PointUp((3 - hintCount) * 100 + 100);
        result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = $"{(3 - hintCount) * 100 + 100}";
    }
    public void FailureAnimation()
    {
        // 실패 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 실패 이미지나 메시지를 표시할 수 있습니다
        Debug.Log("실패");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        resultAudio.clip = resultAudioClips[1];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Fail");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[1];
        resultButton.onClick.AddListener(() => BackButton());
    }
    public IEnumerator GoldParticleOn()
    {
        goldParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        goldParticle.gameObject.SetActive(true);
    }
    IEnumerator CardShuffle()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetActive(true);
        }
        for (int i = 0; i < cards.Length; i++)
        {
            float time = 0;
            while (time < 1)
            {
                time += 2 * Time.deltaTime;
                cards[i].GetComponent<RectTransform>().localPosition = Vector2.Lerp(cards[i].GetComponent<RectTransform>().localPosition, rects[i],8f * Time.deltaTime);
                cards[i].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                cards[i].GetComponent<Button>().enabled = false;
                yield return null;
            }
        }
        InitializeCards();
    }
    public void PuzzleHintImageChange(int data)
    {
        for (int i = 0; i < puzzleHints.Length; i++)
        {
            puzzleHints[i].gameObject.GetComponent<Image>().sprite = puzzleHintImgs[data];
        }
    }
    #endregion
    public void StartButton()
    {
        if (roundNumber < 4)
            StartCoroutine(CardShuffle());
        else if (roundNumber >= 4 && roundNumber < 8)
            StartCoroutine(GetComponent<PuzzleManager>().InitializeTile());
    }
    int roundIndex=0;
    public void Unit(int num)
    {
        if (roundIndex < 4)
        {
            cardFace = cardSet[num].cardSet;
        }
        else if (roundIndex >= 4 && roundIndex < 8)
        {

            GetComponent<PuzzleManager>().tileSprites_3x3 = cardSet[num].puzzleSet_1Round3x3;
            GetComponent<PuzzleManager>().originImg = cardSet[num].puzzleSet_1RoundOriginImg;
            GetComponent<PuzzleManager>().originEngImg = cardSet[num].puzzleSet_1RoundOriginTxt;

            GetComponent<PuzzleManager>().twoRound_tileSprites_3x3 = cardSet[num].puzzleSet_2Round3x3;
            GetComponent<PuzzleManager>().twoRound_originImg = cardSet[num].puzzleSet_2RoundOriginImg;
            GetComponent<PuzzleManager>().twoRound_originEngImg = cardSet[num].puzzleSet_2RoundOriginTxt;

            GetComponent<PuzzleManager>().threeRound_tileSprites_3x3 = cardSet[num].puzzleSet_3Round3x3;
            GetComponent<PuzzleManager>().threeRound_originImg = cardSet[num].puzzleSet_3RoundOriginImg;
            GetComponent<PuzzleManager>().threeRound_originEngImg = cardSet[num].puzzleSet_3RoundOriginTxt;

            GetComponent<PuzzleManager>().fourRound_tileSprites_3x3 = cardSet[num].puzzleSet_4Round3x3;
            GetComponent<PuzzleManager>().fourRound_originImg = cardSet[num].puzzleSet_4RoundOriginImg;
            GetComponent<PuzzleManager>().fourRound_originEngImg = cardSet[num].puzzleSet_4RoundOriginTxt;
        }
    }
   
    public void Round(int stage)
    {
        playTimeTxt.text = "60";
        GetComponent<PuzzleManager>().playTimeTxt.text = "60";
        StopAllCoroutines();
        GetComponent<PuzzleManager>().StopAllCoroutines();
        roundIndex = stage;

        Unit(Uichage.unit);

        currentTime = timeLimit;
        isTimerRunning = true;
        result.gameObject.SetActive(false);
        roundNumber = stage;
        hintCount = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetActive(false);
        }
        for (int i = 0; i < puzzleBtns.Length; i++)
        {
            puzzleBtns[i].SetActive(false);
        }
        switch (stage)
        {
            case 0:
                StartStage(0);
                break;
            case 1:
                StartStage(0);
                break;
            case 2:
                StartStage(0);
                break;
            case 3:
                StartStage(0);
                break;
            case 4:
                player.url = backgroundVideoUrls[0];
                PuzzleStageSetting(0);
                break;
            case 5:
                player.url = backgroundVideoUrls[1];
                PuzzleStageSetting(1);
                break;
            case 6:
                player.url = backgroundVideoUrls[2];
                PuzzleStageSetting(2);
                break;
            case 7:
                player.url = backgroundVideoUrls[3];
                PuzzleStageSetting(3);
                break;
        }
    }
    /// <summary>
    /// 0 = Memory Game => ī�������
    /// 1 = Puzzle Game
    /// </summary>
    /// <param name="index"></param>
    public void ChangeGame(int index)
    {
        for (int i = 0; i < gamesUi.transform.childCount; i++)
        {
            gamesUi.transform.GetChild(i).gameObject.SetActive(false);
        }
        gamesUi.transform.GetChild(index).gameObject.SetActive(true);
        StartButton();
    }
    public void BackButton()
    {
        //카드게임 백버튼 오류 해결 코드
        if (roundNumber < 4)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].GetComponent<RectTransform>().localPosition = point.localPosition;
                cards[i].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);
                cards[i].GetComponent<Image>().sprite = cardBack;
            }
        }
        listScroll.CloseUi();
    }
    #region �������
    public void PuzzleStageSetting(int index)
    {
        player.prepareCompleted -= OnVideoLoaded;
        player.prepareCompleted += OnVideoLoaded;

        StartCoroutine(VideoReady(index));
    }
    public void OnVideoLoaded(VideoPlayer vp)
    {
        ready = true;
    }
    IEnumerator VideoReady(int num)
    {
        fade.SetActive(true);
        GetComponent<PuzzleManager>().FolderPathName(num);
        GetComponent<PuzzleManager>().delayImg.gameObject.SetActive(true);
        PuzzleHintImageChange(num);
        StartStage(1);

        yield return new WaitForSeconds(1f);

        player.Play();

        while(!ready)
        {
            yield return null;
        }


        ready = false;
        fade.SetActive(false);
    }
    public void InitializePuzzleHint()
    {
        for (int i = 0; i < puzzleHints.Length; i++)
        {
            puzzleHints[i].gameObject.SetActive(true);
        }
    }
    #endregion
    public void InitializeOneCardHint()
    {
        for (int i = 0; i < oneCardHints.Length; i++)
        {
            oneCardHints[i].gameObject.SetActive(true);
        }
    }
}
