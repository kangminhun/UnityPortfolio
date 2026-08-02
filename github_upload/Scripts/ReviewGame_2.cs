using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewGame_2 : MonoBehaviour
{
    public Uichage sc2;
    public Button[] buttons;
    public ReviewGame2Quiz[] quizs;

    public Button leftBtn;
    public Image[] leftImgs;
    public int leftCount;

    public Button midBtn;
    public Image[] midImgs;
    public int midCount;

    public Button rightBtn;
    public Image[] rightImgs;
    public int rightCount;

    public List<int> randomList;

    public string myChoice;
    public GameObject myChoiceImg;
    private int count;
    private bool clickOn;
    private Sprite mySprite;

    public Slider timerSlider;
    private float timeLimit = 60f; // 1분(60초)
    private float currentTime;
    private bool isTimerRunning;

    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;

    public AudioSource failEffectBgm;
    public Sprite[] resultButtonImg;
    public ParticleSystem goldParticle;
    public void OnEnable()
    {
        result.SetActive(false);
        myChoiceImg.SetActive(false);
        GameStart();
    }
    public void ReStart()
    {
        result.SetActive(false);
        GameStart();
    }
    public void GameStart()
    {
        randomList = new List<int>();
        leftBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.RemoveAllListeners();
        leftBtn.onClick.AddListener(() => Choice("Left"));
        rightBtn.onClick.AddListener(() => Choice("Right"));
        midBtn.onClick.AddListener(() => Choice("Mid"));
        count = 0;
        rightCount = 0;
        leftCount = 0;
        midCount = 0;
        int randomIndex = 0;
        for (int i = 0; i < quizs.Length;)
        {
            randomIndex = Random.Range(0, quizs.Length);
            if (!randomList.Contains(randomIndex))
            {
                randomList.Add(randomIndex);
                i++;
            }
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
        for (int i = 0; i < leftImgs.Length; i++)
        {
            leftImgs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < rightImgs.Length; i++)
        {
            rightImgs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < midImgs.Length; i++)
        {
            midImgs[i].gameObject.SetActive(false);
        }
        Setting();

        currentTime = timeLimit;
        isTimerRunning = true;

        // 타이머 코루틴을 시작합니다
        timerSlider.value = 1f;
        StartCoroutine(TimerCoroutine());
    }
    public void Setting()
    {
        if (count == 4 || count ==8)
        {
            randomList.RemoveRange(0, 4);
        }
        for (int k = 0; k < 4; k++)
        {
            int sum = k;
            buttons[sum].gameObject.SetActive(true);
            buttons[sum].GetComponent<Image>().sprite = quizs[randomList[sum]].mySprite;
            buttons[sum].onClick.AddListener(() => Click(quizs[randomList[sum]], buttons[sum]));
        }
    }
    public void Click(ReviewGame2Quiz quiz, Button button)
    {
        myChoice = quiz.answer;
        mySprite = quiz.mySprite;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }

        button.gameObject.SetActive(false);
        myChoiceImg.GetComponent<Animator>().runtimeAnimatorController = quiz.animator;
        myChoiceImg.SetActive(true);
        for (int i = 0;i < myChoiceImg.transform.GetChild(0).transform.childCount; i++)
        {
            myChoiceImg.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(true);
        }
        myChoiceImg.GetComponent<Image>().sprite = mySprite;
        count++;
        clickOn = true;
    }
    public void Choice(string str)
    {
        if (clickOn)
        {
            if (str == "Left")
            {
                if (myChoice == str)
                {
                    myChoiceImg.SetActive(false);
                    LeftClick();
                }
                else
                {
                    failEffectBgm.Play ();
                    return;
                }
            }
            else if (str == "Mid")
            {
                if (myChoice == str)
                {
                    myChoiceImg.SetActive(false);
                    MidClick();
                }
                else
                {
                    failEffectBgm.Play();
                    return;
                }
            }
            else if (str == "Right")
            {
                if (myChoice == str)
                {
                    myChoiceImg.SetActive(false);
                    RightClick();
                }
                else
                {
                    failEffectBgm.Play();
                    return;
                }
            }
        }

    }
    public void LeftClick()
    {
        clickOn = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
        leftImgs[leftCount].sprite = mySprite;
        leftImgs[leftCount].gameObject.SetActive(true);
        leftCount++;
        if (count == 4 || count == 8)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
            Setting();
        }
        else if (count == 12)
        {
            if (isTimerRunning)
            {
                isTimerRunning = false; // 타이머 정지
                SuccessAnimation(); // 성공 애니메이션 호출
            }
        }
    }
    public void MidClick()
    {
        clickOn = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
        midImgs[midCount].sprite = mySprite;
        midImgs[midCount].gameObject.SetActive(true);
        midCount++;
        if (count == 4 || count == 8)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
            Setting();
        }
        else if (count == 12)
        {
            if (isTimerRunning)
            {
                isTimerRunning = false; // 타이머 정지
                SuccessAnimation(); // 성공 애니메이션 호출
            }
        }
    }
    public void RightClick()
    {
        clickOn = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
        rightImgs[rightCount].sprite = mySprite;
        rightImgs[rightCount].gameObject.SetActive(true);
        rightCount++;
        if (count == 4 || count == 8)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
            Setting();
        }
        else if (count == 12)
        {
            if (isTimerRunning)
            {
                isTimerRunning = false; // 타이머 정지
                SuccessAnimation(); // 성공 애니메이션 호출
            }
        }
    }
    public void End()
    {
        sc2.Back();
    }
    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timerSlider.value = currentTime / timeLimit;
            // 필요한 경우 UI를 currentTime으로 업데이트합니다

            yield return null;
        }

        if (currentTime <= 0f)
        {
            // 타이머가 만료되면 실패 애니메이션 함수를 호출합니다
            FailureAnimation();
        }
    }
    private void SuccessAnimation()
    {
        // 성공 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 성공 이미지나 파티클 효과를 재생할 수 있습니다
        Debug.Log("성공");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        StartCoroutine(GoldParticleOn());
        result.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        resultAudio.clip = resultAudioClips[0];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Success");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[0];
        resultButton.onClick.AddListener(() => End());
        DataBase.instance.PointManager.PointUp(100);
        result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = "100";
    }
    private void FailureAnimation()
    {
        // 실패 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 실패 이미지나 메시지를 표시할 수 있습니다
        Debug.Log("실패");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        result.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        resultAudio.clip = resultAudioClips[1];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Fail");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[1];
        resultButton.onClick.AddListener(() => ReStart());
    }
    public IEnumerator GoldParticleOn()
    {
        goldParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        goldParticle.gameObject.SetActive(true);
    }
}
