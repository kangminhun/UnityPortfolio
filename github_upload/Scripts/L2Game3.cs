using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2Game3 : MonoBehaviour
{
    public ListScroll listScroll;
    public Button[] buttons;
    public ReviewGame1Quiz[] reviewGame;
    public Image quizImg;
    public Sprite[] buttonImgs;
    public List<int> answers;
    private int myChoice;
    private List<int> randomList;

    public Text timerTxt;
    public float timeLimit = 60f; // 1분(60초)
    private float currentTime;
    private bool isTimerRunning;

    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;

    public AudioSource effectSound;
    public AudioClip[] effects;
    private bool click;

    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간

    private Vector3 originalPosition;
    public void OnEnable()
    {
        result.SetActive(false);
        GameSet();
    }
    public void ReStart()
    {
        result.SetActive(false);
        GameSet();
    }
    public void GameSet()
    {
        randomList = new List<int>();
        int randomIndex = 0;
        for (int i = 0; i < reviewGame.Length;)
        {
            randomIndex = Random.Range(0, reviewGame.Length);
            if (!randomList.Contains(randomIndex))
            {
                randomList.Add(randomIndex);
                i++;
            }
        }
        Setting();
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public void Setting()
    {
        click = false;
        int randomIndex = 0;
        quizImg.sprite = reviewGame[randomList[0]].mySprite;
        myChoice = reviewGame[randomList[0]].myID;
        randomList.Remove(randomList[0]);
        answers = new List<int>();
        for (int i = 0; i < reviewGame.Length; i++)
        {
            int sum = i;
            answers.Add(sum);
        }
        Shuffle(answers);
        randomIndex = Random.Range(0, 3);
        for (int i = 0; i < buttons.Length; i++)
        {
            int sum = i;
            buttons[sum].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            if (sum  == randomIndex)
            {
                buttons[randomIndex].gameObject.GetComponent<L2G3ButtonInfomation>().answer = myChoice;

            }
            else
            {
                answers.Remove(myChoice);
                buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer = answers[sum];
            }

            if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 0)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[1];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[0];
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 1)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[2];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[1];
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 2)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[3];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[2];
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 3)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[4];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[3];
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 4)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[5];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[4];
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 5)
            {
                if (sum == randomIndex)
                {
                    effectSound.clip = effects[6];
                    effectSound.Play();
                }
                buttons[sum].gameObject.GetComponent<Image>().sprite = buttonImgs[5];
            }
        }
    }
    public void Click(int num)
    {
        if (!click)
        {
            click = true;
            if (buttons[num].gameObject.GetComponent<L2G3ButtonInfomation>().answer == myChoice)
            {
                Debug.Log("정답");
                effectSound.clip = effects[0];
                effectSound.Play();


                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].enabled = true;
                    buttons[i].GetComponent<Image>().color = Color.white;
                }


                StartCoroutine(Delay(buttons[num]));
            }
            else
            {
                buttons[num].enabled = false;
                buttons[num].GetComponent<Image>().color = Color.gray;
                StartCoroutine(ShakeUIElement(buttons[num].GetComponent<RectTransform>()));
                Debug.Log("오답");
            }
        }
    }
    private IEnumerator ShakeUIElement(RectTransform uiElement)
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
        click = false;
    }
    void Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    IEnumerator Delay(Button button)
    {
        if (randomList.Count != 0)
        {
            button.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Animator animator = button.transform.GetChild(0).gameObject.GetComponent<Animator>();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + .5f);
            Setting();
        }
        else
        {
            button.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Animator animator = button.transform.GetChild(0).gameObject.GetComponent<Animator>();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + .5f);
            Debug.Log("끝");
            StopCoroutine(TimerCoroutine());
            timerTxt.text = ((int)(currentTime)).ToString();
            SuccessAnimation();
        }
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
    private void SuccessAnimation()
    {
        // 성공 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 성공 이미지나 파티클 효과를 재생할 수 있습니다
        isTimerRunning = false;
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
    public void End()
    {
        listScroll.CloseUi();
    }
}
