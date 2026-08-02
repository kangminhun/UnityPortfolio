using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2Game1 : MonoBehaviour
{
    public ListScroll listScroll;
    public Image midImg;
    public string myChoice;
    public int myID;
    public ReviewGame1Quiz[] quizs;
    public List<int> randomList;
    public GameObject[] words;
    public AudioSource effectSound;
    public AudioClip[] effects;

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

    public GameObject aButton;
    public GameObject bButton;
    public void OnEnable()
    {
        midImg.gameObject.SetActive(false);
        result.SetActive(false);
        for (int i = 0; i < words.Length; i++)
        {
            words[i].SetActive(false);
        }
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
        for (int i = 0; i < quizs.Length;)
        {
            randomIndex = Random.Range(0, quizs.Length);
            if (!randomList.Contains(randomIndex))
            {
                randomList.Add(randomIndex);
                i++;
            }
        }
        for (int i = 0; i < words.Length; i++)
        {
            words[i].SetActive(false);
        }
        Setting();
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public void Setting()
    {
        if (randomList.Count > 0)
        {
            midImg.GetComponent<Image>().sprite = quizs[randomList[0]].mySprite;
            midImg.gameObject.SetActive(true);
            myChoice = quizs[randomList[0]].answer;
            myID = quizs[randomList[0]].myID;
            randomList.Remove(randomList[0]);
        }
    }
    public void Click(string str)
    {
        if (str == "L")
        {
            if(myChoice== "Left")
            {
                if(myID == 1)
                {
                    StartCoroutine(SoundDelay(effects[0]));
                    StartCoroutine(SuccessDelay(words[0]));
                }
                else
                {
                    StartCoroutine(SoundDelay(effects[1]));
                    StartCoroutine(SuccessDelay(words[1]));
                }
            }
            else if(myChoice == "Right")
            {
                return;
            }
        }
        else if (str == "R")
        {
            if (myChoice == "Left")
            {
                return;
            }
            else if (myChoice == "Right")
            {
                if (myID == 1)
                {
                    StartCoroutine(SoundDelay(effects[2]));
                    StartCoroutine(SuccessDelay(words[2]));
                }
                else
                {
                    StartCoroutine(SoundDelay(effects[3]));
                    StartCoroutine(SuccessDelay(words[3]));
                }
            }
        }
    }
    public IEnumerator SoundDelay(AudioClip clip)
    {
        effectSound.clip = effects[4];
        effectSound.Play();
        while(effectSound.isPlaying)
        {
            yield return null;
        }
        effectSound.clip = clip;
        effectSound.Play();
    }
    public IEnumerator SuccessDelay(GameObject word)
    {
        midImg.gameObject.SetActive(false);
        word.SetActive(true);
        Animator animator = word.GetComponent<Animator>();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + .5f);
        if (randomList.Count > 0)
        {
            Setting();
            midImg.gameObject.SetActive(true);
        }
        else
        {
            midImg.gameObject.SetActive(false);
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
            if((int)currentTime <= 10)
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
