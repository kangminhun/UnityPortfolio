using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class S2VocaGame : MonoBehaviour
{
    public ListScroll listScroll;
    public Button[] buttons;
    public int[] ids;

    public RectTransform spawnPoint;
    public GameObject quizPrefab;
    private GameObject quiz;

    public string[] buttontxts;
    public List<int> answers;
    private int myChoice;
    private List<int> randomList;

    public TextMeshProUGUI timerTxt;
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
    public AudioClip[] effectSoundClips;
    public AudioClip successEffect;
    public AudioClip failEffect;
    private bool click;

    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간

    public Animator mainAnimator;
    public GameObject greenLight;
    public GameObject redLight;
    public GameObject bar;
    public GameObject maskObj;

    private bool first;
    public void OnEnable()
    {
        result.SetActive(false);
    }
    public void ReStart()
    {
        result.SetActive(false);
        GameSet();
    }
    public void GameSet()
    {
        if (quiz != null)
        {
            Destroy(quiz);
        }
        quiz = Instantiate(quizPrefab, spawnPoint.transform);
        quiz.transform.SetAsFirstSibling();
        quiz.gameObject.GetComponent<Image>().enabled = false;
        randomList = new List<int>();
        int randomIndex = 0;
        for (int i = 0; i < ids.Length;)
        {
            randomIndex = Random.Range(0, ids.Length);
            if (!randomList.Contains(randomIndex))
            {
                randomList.Add(randomIndex);
                i++;
            }
        }
        greenLight.SetActive(false);
        redLight.SetActive(false);
        currentTime = timeLimit;
        click = false;
        first = true;
        Setting();
    }
    IEnumerator OnAnimatorDelay()
    {
        mainAnimator.SetTrigger("On");
        yield return new WaitForSeconds(mainAnimator.GetCurrentAnimatorClipInfo(0).Length + .5f);
        quiz.transform.GetChild(myChoice).gameObject.SetActive(true);
        first = false;
    }
    public void Setting()
    {
        bar.SetActive(false);
        maskObj.GetComponent<RectTransform>().offsetMax = new Vector2(maskObj.GetComponent<RectTransform>().offsetMax.x, -110);
        int randomIndex = 0;

        myChoice = ids[randomList[0]];

        quiz.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0f);
        quiz.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0f);
        quiz.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
        quiz.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        quiz.GetComponent<Animator>().SetTrigger(myChoice.ToString());
        for (int i = 0; i < quiz.transform.childCount; i++)
        {
            quiz.transform.GetChild(i).gameObject.SetActive(false);
        }

        randomList.Remove(randomList[0]);
        answers = new List<int>();
        for (int i = 0; i < ids.Length; i++)
        {
            int sum = i;
            answers.Add(sum);
        }
        Shuffle(answers);
        StartTyping(myChoice);
        randomIndex = Random.Range(0, 3);
        for (int i = 0; i < buttons.Length; i++)
        {
            int sum = i;

            if (sum == randomIndex)
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

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[0].ToLower();
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 1)
            {

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[1].ToLower();
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 2)
            {

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[2].ToLower();
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 3)
            {

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[3].ToLower();
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 4)
            {

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[4].ToLower();
            }
            else if (buttons[sum].gameObject.GetComponent<L2G3ButtonInfomation>().answer == 5)
            {

                buttons[sum].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttontxts[5].ToLower();
            }
        }
        if (first)
            StartCoroutine(OnAnimatorDelay());
        else
            quiz.transform.GetChild(myChoice).gameObject.SetActive(true);
    }
    public void Click(int num)
    {
        if (!click)
        {
            click = true;
            if (buttons[num].gameObject.GetComponent<L2G3ButtonInfomation>().answer == myChoice)
            {
                Debug.Log("정답");

                effectSound.clip = effectSoundClips[myChoice];
                effectSound.Play();

                greenLight.SetActive(true);
                redLight.SetActive(false);

                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].enabled = true;
                    buttons[i].GetComponent<Image>().color = Color.white;
                }


                StartCoroutine(Delay());
            }
            else
            {
                effectSound.clip = failEffect;
                effectSound.Play();
                greenLight.SetActive(false);
                redLight.SetActive(true);
                buttons[num].enabled = false;
                buttons[num].GetComponent<Image>().color = Color.gray;
                click = false;
                Debug.Log("오답");
            }
        }
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
    IEnumerator Delay()
    {
        while (effectSound.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        effectSound.clip = successEffect;
        effectSound.Play();

        if (randomList.Count != 0)
        {
            bar.SetActive(true);

            yield return StartCoroutine(AnimateTopValue());

            Setting();
        }
        else
        {
            bar.SetActive(true);

            yield return StartCoroutine(AnimateTopValue());

            for (int i = 0; i < quiz.transform.childCount; i++)
            {
                quiz.transform.GetChild(i).gameObject.SetActive(false);
            }

            isTimerRunning = false;
            timerTxt.text = ((int)(currentTime)).ToString();
            SuccessAnimation();
        }
    }
    IEnumerator AnimateTopValue()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2)
        {
            float currentTop = Mathf.Lerp(110, -815, elapsedTime / 2);
            maskObj.GetComponent<RectTransform>().offsetMax = new Vector2(maskObj.GetComponent<RectTransform>().offsetMax.x, currentTop);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final value is set correctly
        maskObj.GetComponent<RectTransform>().offsetMax = new Vector2(maskObj.GetComponent<RectTransform>().offsetMax.x, -815);
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
    public string[] dialogue;
    public AudioClip[] talkClips;

    public void StartTyping(int num)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(TypingStart(dialogue[num],num));
        }
        else
            return;
    }

    IEnumerator TypingStart(string talk,int num)
    {
        timerTxt.fontSize = 25;
        timerTxt.color=Color.white;
        isTimerRunning = false;
        timerTxt.text = null;

        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");

        for (int i = 0; i < talk.Length; i++)
        {
            timerTxt.text += talk[i];
            yield return new WaitForSeconds(0.05f);
        }
        effectSound.clip = talkClips[num];
        effectSound.Play();
        while (effectSound.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        click = false;
        timerTxt.fontSize = 100;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
}
