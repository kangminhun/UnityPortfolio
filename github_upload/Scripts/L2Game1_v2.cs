using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2Game1_v2 : MonoBehaviour
{
    [Header("Head")]
    public ListScroll listScroll;

    [Space(10)]
    [Header("Main")]
    public Image[] words;
    public Sprite[] wordSprites;
    public Sprite[] words_Image_Sprite;
    public Image[] images;
    public Sprite[] imagesSprites;
    public Image choice;
    public AudioClip[] effects;
    private int myChoice;
    private int wordindex;
    private bool wordclickLook;
    private bool imageclickUnLook;
    private int count;

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
    public AudioSource gameAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;

    [Space(10)]
    [Header("Shake UI")]
    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    private Vector3 originalPosition;
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
        choice.gameObject.SetActive(false);
        count = 0;
        myChoice = 0;
        wordclickLook = false;
        imageclickUnLook = false;

        for (int i = 0; i < words.Length; i++)
        {
            int sum = i;
            words[sum].gameObject.SetActive(true);
            words[sum].sprite = wordSprites[sum];
            words[sum].GetComponent<L2_Game_1_V2_myID>().myID = sum;
            images[sum].gameObject.SetActive(true);
            images[sum].sprite = imagesSprites[sum];
            images[sum].GetComponent<L2_Game_1_V2_myID>().myID = sum;
        }
        words[0].GetComponentInParent<GridLayoutGroup>().enabled = true;
        images[0].GetComponentInParent<GridLayoutGroup>().enabled = true;
        wordclickLook = true;
        imageclickUnLook = true;
        StartCoroutine(RanbomSetting());
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public IEnumerator RanbomSetting()
    {
        int randomIndex = 0;
        for (int i = 0; i < words.Length; i++)
        {
            randomIndex = Random.Range(0, words.Length);
            words[randomIndex].transform.SetAsFirstSibling();
        }
        randomIndex = 0;
        for (int i = 0; i < images.Length; i++)
        {
            randomIndex = Random.Range(0, images.Length);
            images[randomIndex].transform.SetAsFirstSibling();
        }
        yield return new WaitForSeconds(.5f);
        wordclickLook = false;
        imageclickUnLook = false;
        words[0].GetComponentInParent<GridLayoutGroup>().enabled = false;
        images[0].GetComponentInParent<GridLayoutGroup>().enabled = false;
    }
    public void WordClick(int num)
    {
        if (!wordclickLook)
        {
            wordindex = num;
            wordclickLook = true;
            imageclickUnLook = true;
            myChoice = words[num].GetComponent<L2_Game_1_V2_myID>().myID;
            words[num].GetComponent<Image>().sprite = words_Image_Sprite[num];
            gameAudio.clip = effects[num];
            gameAudio.Play();
            //choice.sprite = words[num].sprite;
            //choice.gameObject.SetActive(true);
        }
        else
            return;
    }
    public void ImageClick(int num)
    {
        if (imageclickUnLook)
        {
            imageclickUnLook = false;
            if (images[num].GetComponent<L2_Game_1_V2_myID>().myID == myChoice)
            {
                wordclickLook = false;
                gameAudio.clip = effects[num];
                gameAudio.Play();
                words[wordindex].gameObject.SetActive(false);
                images[num].gameObject.SetActive(false);
                //choice.gameObject.SetActive(false);
                count++;
                if (count == 6)
                {
                    StartCoroutine(SuccessDelay());
                }
            }
            else
            {
               StartCoroutine(ShakeUIElement(images[num].GetComponent<RectTransform>(),num));
            }
        }
        else
            return;
    }
    IEnumerator ImageClickDelay()
    {
        //애니메이션 추가해서 모션추가
        yield return null;
    }
    private IEnumerator ShakeUIElement(RectTransform uiElement, int number)
    {
        gameAudio.clip = effects[number];
        gameAudio.Play();

        while(gameAudio.isPlaying)
        {
            yield return null;
        }

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
        imageclickUnLook = true;
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
