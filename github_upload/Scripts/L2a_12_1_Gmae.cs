using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2a_12_1_Gmae : MonoBehaviour
{

    [Header("Head")]
    public ListScroll listScroll;

    private int buttonIndex;
    private int quizIndex;
    public Button[] wordbuttons;
    public Button[] Imagebuttons;
    public Button[] quizbuttons;
    public GameObject answer;
    public Sprite[] answerimages;
    private int mychoiceWord;
    private int mychoiceImage;
    private bool wordclickLook;
    private bool imageclickUnLook;
    private bool quizclickUnLook;
    public AudioClip[] wordEffects;
    public AudioClip[] alphabetEffects;
    public AudioClip[] quizSound;
    public int maxindex;
    private int count;
    public int roundCount;
    public int quizCount;

    [Space(10)]
    [Header("Shake UI")]
    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    private Vector3 originalPosition;
    public AudioSource gameAudio;

    [Space(10)]
    [Header("Timer")]
    public Text timerTxt;
    public float timeLimit = 60f; // 1분(60초)
    private float currentTime;
    private bool isTimerRunning;

    [Space(10)]
    [Header("Result UI")]
    //public GameObject startUi;
    public GameObject result;
    public Button resultButton;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;
    public GameObject mainAnimator;

    private List<int> wordchoiceNum;
    private List<int> imagechoiceNum;
    public void OnEnable()
    {
        wordchoiceNum = new List<int>();
        imagechoiceNum = new List<int>();
        result.SetActive(false);
        for (int i = 0; i < wordbuttons.Length; i++)
        {
            wordbuttons[i].gameObject.SetActive(false);
            Imagebuttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < roundCount; i++)
        {
            wordbuttons[i].gameObject.SetActive(true);
            Imagebuttons[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < quizbuttons.Length; i++)
        {
            quizbuttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < quizCount; i++)
        {
            quizbuttons[i].gameObject.SetActive(true);
        }
        wordclickLook = false;
        quizclickUnLook = true;
        imageclickUnLook = true;
        count = 0;
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public void ReStart()
    {
        for (int i = 0; i < roundCount; i++)
        {
            wordbuttons[i].gameObject.SetActive(true);
            Imagebuttons[i].gameObject.SetActive(true);
        }
        wordclickLook = false;
        quizclickUnLook = true;
        imageclickUnLook = true;
        count = 0;
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public void RoundUp()
    {
        for (int i = 0; i < roundCount; i++)
        {
            wordbuttons[i].gameObject.SetActive(false);
            Imagebuttons[i].gameObject.SetActive(false);
        }
        for (int i = roundCount; i < wordbuttons.Length; i++)
        {
            wordbuttons[i].gameObject.SetActive(true);
            Imagebuttons[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < quizCount; i++)
        {
            quizbuttons[i].gameObject.SetActive(false);
        }
        for (int i = quizCount; i < quizbuttons.Length; i++)
        {
            quizbuttons[i].gameObject.SetActive(true);
        }
    }
    public void ButtonClick(int num)
    {
        if (!wordclickLook)
        {
            buttonIndex = num;
            wordclickLook = true;
            quizclickUnLook = false;
            gameAudio.clip = wordEffects[mychoiceWord];
            gameAudio.Play();
        }
    }
    public void Click(int num)
    {
        mychoiceWord = num;
        wordchoiceNum.Add(num);
    }
    public void imageButtonNum(int num)
    {
        mychoiceImage = num;
        imagechoiceNum.Add(num);
    }
    public void QuizClick(int num)
    {
        if (!quizclickUnLook)
        {
            gameAudio.clip = quizSound[num];
            gameAudio.Play();
            if (num == buttonIndex)
            {
                quizclickUnLook = true;
                quizIndex = num;
                for (int i = 0; i < wordbuttons.Length; i++)
                {
                    if (i != mychoiceWord)
                    {
                        wordbuttons[i].gameObject.SetActive(false);
                    }
                }
                for (int i = 0; i < quizCount; i++)
                {
                    if (i != num)
                        quizbuttons[i].gameObject.SetActive(false);
                }
                mainAnimator.GetComponent<Animator>().SetTrigger(mychoiceWord.ToString());
                StartCoroutine(Delay(mainAnimator.GetComponent<Animator>()));
            }
            else
            {
                StartCoroutine(ShakeUIElement(quizbuttons[num].GetComponent<RectTransform>(), 0));
            }
        }
    }
    private IEnumerator Delay(Animator animator)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        answer.GetComponent<Image>().sprite = answerimages[mychoiceWord];
        wordbuttons[mychoiceWord].gameObject.SetActive(false);
        quizbuttons[buttonIndex].gameObject.SetActive(false);
        answer.SetActive(true);
        imageclickUnLook = false;
        animator.SetTrigger("END");
    }
    public void ImageClick(int num)
    {
        if (!imageclickUnLook)
        {
            if (quizIndex == num && mychoiceWord == mychoiceImage)
            {
                if (count < roundCount)
                {
                    for (int i = 0; i < quizCount; i++)
                    {
                        quizbuttons[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < roundCount; i++)
                    {
                        if (!wordchoiceNum.Contains(i))
                            wordbuttons[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = roundCount; i < wordbuttons.Length; i++)
                    {
                        if (!wordchoiceNum.Contains(i))
                            wordbuttons[i].gameObject.SetActive(true);
                    }
                    for (int i = quizCount; i < quizbuttons.Length; i++)
                    {
                        quizbuttons[i].gameObject.SetActive(true);
                    }
                }

                wordbuttons[mychoiceWord].gameObject.SetActive(false);
                Imagebuttons[mychoiceImage].gameObject.SetActive(false);

                answer.SetActive(false);
                gameAudio.clip = alphabetEffects[mychoiceImage];
                gameAudio.Play();
                wordclickLook = false;
                quizclickUnLook = true;
                imageclickUnLook = true;
                count++;
                if (count == roundCount)
                {
                    RoundUp();
                }
                else if (count >= maxindex)
                {
                    for (int i = 0; i < wordbuttons.Length; i++)
                    {
                        wordbuttons[i].gameObject.SetActive(false);
                        Imagebuttons[i].gameObject.SetActive(false);
                    }
                    for (int i = 0; i < quizbuttons.Length; i++)
                    {
                        quizbuttons[i].gameObject.SetActive(false);
                    }
                    StartCoroutine(SuccessDelay());
                }
            }
            else
            {
                imageclickUnLook = true;
                StartCoroutine(ShakeUIElement(Imagebuttons[mychoiceImage].GetComponent<RectTransform>(), 1));
            }
        }
    }
    private IEnumerator ShakeUIElement(RectTransform uiElement, int state)
    {
        float elapsedTime = 0f;
        originalPosition = uiElement.anchoredPosition;
        while (elapsedTime < shakeDuration)
        {
            // 좌우로 흔들림 효과를 주기 위해 삼각함수(sin)를 사용합니다.
            float xOffset = Mathf.Sin(Time.time * Mathf.PI * 2f * 5f) * shakeDistance;
            Vector3 newPosition = originalPosition + new Vector3(xOffset, 0f, 0f);
            uiElement.anchoredPosition = newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 돌아갑니다.
        uiElement.anchoredPosition = originalPosition;
        if (state == 1)
            imageclickUnLook = false;
        else
            quizclickUnLook = false;
    }
    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timerTxt.text = ((int)(currentTime)).ToString();
            // 필요한 경우 UI를 currentTime으로 업데이트합니다
            if ((int)currentTime <= 10)
            {
                timerTxt.color = Color.red;
            }
            else
            {
                timerTxt.color = Color.white;
            }
            yield return null;
        }

        if (currentTime <= 0f)
        {
            // 타이머가 만료되면 실패 애니메이션 함수를 호출합니다
            FailureAnimation();
        }
    }
    public IEnumerator SuccessDelay()
    {
        yield return new WaitForSeconds(.5f);
        isTimerRunning = false;
        StopCoroutine(TimerCoroutine());
        timerTxt.text = ((int)(currentTime)).ToString();
        SuccessAnimation();
    }
    private void SuccessAnimation()
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
        gameAudio.clip = resultAudioClips[0];
        gameAudio.Play();
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
        result.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameAudio.clip = resultAudioClips[1];
        gameAudio.Play();
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
    public void End()
    {
        listScroll.CloseUi();
    }
}
