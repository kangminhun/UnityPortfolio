using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L2_U4_Game_1 : MonoBehaviour
{
    [Header("Head")]
    public ListScroll listScroll;

    [Space(10)]
    [Header("Main")]
    public Image[] alphabets;
    public Sprite[] alphabetSprites;
    public Sprite[] wordSprite; //정답 이미지 완성본
    public Image[] images;
    public Sprite[] imagesSprites;

    public GameObject choice_parents;
    public Image choice_Alphabet; // 앞부분 이미지
    public Image choice_Word; // 표기 될 왼성본 이미지
    public Image choice_Img; // 마지막에 완성된 단어랑 같이 뜰 이미지
    public Image quiz; // 뒷 부분 이미지
    public Sprite[] quizSprites; // 뒷 부분 이미지들
    public AudioClip[] quizSound;

    public AudioClip[] wordEffects;
    public AudioClip[] alphabetEffects;
    public AudioClip successSound;
    private int myChoice;
    private int myChoice2;
    private bool wordclickLook;
    private bool imageclickUnLook;
    private int count;
    private List<int> quizList;
    private Dictionary<int, int> usedNumbersCount;

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
    private List<int> overlapInt;

    [SerializeField] private List<GameObject> overlapList;

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
        overlapList = new List<GameObject>();
        overlapInt = new List<int>();
        usedNumbersCount = new Dictionary<int, int>();
        quizList = new List<int>();
        choice_Alphabet.gameObject.SetActive(false);
        choice_Word.gameObject.SetActive(false);
        choice_Img.gameObject.SetActive(false);
        count = 0;
        myChoice = 0;
        wordclickLook = false;
        imageclickUnLook = false;

        for (int i = 0; i < alphabets.Length; i++)
        {
            int sum = i;
            alphabets[sum].gameObject.SetActive(true);
            alphabets[sum].sprite = alphabetSprites[sum];
            alphabets[sum].GetComponent<L2_Game_1_V2_myID>().myID = sum;
            alphabets[sum].GetComponent<L2_Game_1_V2_myID>().myID2 = 1000;
            images[sum].gameObject.SetActive(true);
            images[sum].sprite = imagesSprites[sum];
            images[sum].GetComponent<L2_Game_1_V2_myID>().myID = sum;
            for (int j = 0; j < alphabetEffects.Length; j++)
            {
                int sum2 = j;
                if (alphabetEffects[sum] == alphabetEffects[sum2] && sum != sum2)
                {
                    Debug.Log($"{alphabetEffects[sum2]}와 {alphabetEffects[sum]} 중복");
                    overlapInt.Add(sum);
                    overlapInt.Add(sum2);
                    overlapList.Add(alphabets[sum].gameObject);
                    alphabets[sum].GetComponent<L2_Game_1_V2_myID>().myID = sum;
                    alphabets[sum].GetComponent<L2_Game_1_V2_myID>().myID2 = sum2;
                }
            }
        }
        alphabets[0].GetComponentInParent<GridLayoutGroup>().enabled = true;
        images[0].GetComponentInParent<GridLayoutGroup>().enabled = true;
        wordclickLook = true;
        imageclickUnLook = true;
        StartCoroutine(RanbomSetting());
        quiz.sprite = quizSprites[quizList[count]];
        quiz.gameObject.SetActive(true);
        gameAudio.clip = quizSound[quizList[count]];
        gameAudio.Play();
        currentTime = timeLimit;
        isTimerRunning = true;
        StartCoroutine(TimerCoroutine());
    }
    public IEnumerator RanbomSetting()
    {
        int randomIndex = 0;
        for (int i = 0; i < alphabets.Length; i++)
        {
            randomIndex = Random.Range(0, alphabets.Length);
            alphabets[randomIndex].transform.SetAsFirstSibling();
        }
        randomIndex = 0;
        for (int i = 0; i < images.Length; i++)
        {
            randomIndex = Random.Range(0, images.Length);
            images[randomIndex].transform.SetAsFirstSibling();
        }
        for (int i = 0; i < 4; i++)
        {
            do
            {
                randomIndex = Random.Range(0, 2);
            } while (usedNumbersCount.TryGetValue(randomIndex, out var count) && count >= 2);

            if (!usedNumbersCount.ContainsKey(randomIndex))
            {
                usedNumbersCount[randomIndex] = 1;
            }
            else
            {
                usedNumbersCount[randomIndex]++;
            }

            quizList.Add(randomIndex);
        }
        yield return new WaitForSeconds(.5f);
        wordclickLook = false;
        imageclickUnLook = false;
        alphabets[0].GetComponentInParent<GridLayoutGroup>().enabled = false;
        images[0].GetComponentInParent<GridLayoutGroup>().enabled = false;
    }
    public void WordClick(int num)
    {
        if (!wordclickLook)
        {
            if (alphabets[num].GetComponent<L2_Game_1_V2_myID>().myID2 != 1000)
            {
                int putIndex;
                if(num < 2)
                {
                    putIndex = 0;
                }
                else
                {
                    putIndex = 1;
                }
                if (quizList[count] == 0)
                {
                    if (overlapList.Count > 1)
                    {
                        if (overlapList[putIndex].GetComponent<L2_Game_1_V2_myID>().myID == 0)
                        {
                            myChoice = 0;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 0)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else if(overlapList[putIndex].GetComponent<L2_Game_1_V2_myID>().myID2 == 0)
                        {
                            myChoice = 0;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 0)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else
                        {
                            gameAudio.clip = alphabetEffects[num];
                            gameAudio.Play();
                            StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                        }
                    }
                    else
                    {
                        if (overlapList[0].GetComponent<L2_Game_1_V2_myID>().myID == 0)
                        {
                            myChoice = 0;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 0)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else
                        {
                            gameAudio.clip = alphabetEffects[num];
                            gameAudio.Play();
                            StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                        }
                    }
                }
                else if (quizList[count] == 1)
                {
                    if (overlapList.Count > 1)
                    {
                        if (overlapList[putIndex].GetComponent<L2_Game_1_V2_myID>().myID == 2)
                        {
                            myChoice = 2;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 2)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else if (overlapList[putIndex].GetComponent<L2_Game_1_V2_myID>().myID2 == 2)
                        {
                            myChoice = 2;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 2)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else
                        {
                            gameAudio.clip = alphabetEffects[num];
                            gameAudio.Play();
                            StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                        }
                    }
                    else
                    {
                        if (overlapList[0].GetComponent<L2_Game_1_V2_myID>().myID == 2)
                        {
                            myChoice = 2;
                            StartCoroutine(WordClickDelay(true, num));
                            for (int i = 0; i < overlapList.Count; i++)
                            {
                                if (overlapList[i].GetComponent<L2_Game_1_V2_myID>().myID == 2)
                                {
                                    overlapList.Remove(overlapList[i]);
                                }
                            }
                        }
                        else
                        {
                            gameAudio.clip = alphabetEffects[num];
                            gameAudio.Play();
                            StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                        }
                    }
                }
            }
            else
            {
                myChoice = alphabets[num].GetComponent<L2_Game_1_V2_myID>().myID;
                if (quizList[count] == 0)
                {
                    if (myChoice == 0 || myChoice == 1)
                    {
                        StartCoroutine(WordClickDelay(true, num));
                    }
                    else
                    {
                        gameAudio.clip = alphabetEffects[num];
                        gameAudio.Play();
                        StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                    }
                }
                else if (quizList[count] == 1)
                {
                    if (myChoice == 2 || myChoice == 3)
                    {
                        StartCoroutine(WordClickDelay(true, num));
                    }
                    else
                    {
                        StartCoroutine(ShakeUIElement(alphabets[num].GetComponent<RectTransform>(), 0, num));
                    }
                }
            }
        }
        else
            return;
    }
    public IEnumerator WordClickDelay(bool bol, int number)
    {
        wordclickLook = bol;

        if (alphabets[number].GetComponent<L2_Game_1_V2_myID>().myID2 != 1000)
        {
            gameAudio.clip = alphabetEffects[myChoice];
            gameAudio.Play();
        }
        else
        {
            gameAudio.clip = alphabetEffects[number];
            gameAudio.Play();
        }

        while (gameAudio.isPlaying)
        {
            yield return null;
        }


        Vector3 targetPosition = new Vector3(-204, -202, 0);
        Vector3 alphabetsPosition = alphabets[number].GetComponent<RectTransform>().localPosition;
        // Set the speed at which the object moves
        float moveSpeed = 10f;

        while (Vector3.Distance(alphabets[number].GetComponent<RectTransform>().localPosition, targetPosition) > 0.5f)
        {
            // Move the object towards the target position using Vector3.Lerp
            alphabets[number].GetComponent<RectTransform>().localPosition = Vector3.Lerp(alphabets[number].GetComponent<RectTransform>().localPosition, targetPosition, Time.deltaTime * moveSpeed);
            yield return null; // Wait for the next frame
        }

        alphabets[number].gameObject.SetActive(false);
        alphabets[number].GetComponent<RectTransform>().localPosition = alphabetsPosition;
        choice_Alphabet.sprite = alphabets[number].sprite;
        choice_Alphabet.gameObject.SetActive(true);
        choice_parents.GetComponent<Animator>().SetTrigger("Alphabet");


        yield return new WaitForSeconds(1f);

        gameAudio.clip = successSound;
        gameAudio.Play();
        choice_Alphabet.gameObject.SetActive(false);
        quiz.gameObject.SetActive(false);
        choice_Word.sprite = wordSprite[myChoice];
        choice_Word.gameObject.SetActive(true);
        imageclickUnLook = bol;
        choice_parents.GetComponent<Animator>().SetTrigger("Alphabet_End");
    }
    public void ImageClick(int num)
    {
        if (imageclickUnLook)
        {
            if (images[num].GetComponent<L2_Game_1_V2_myID>().myID2 != 1000)
            {
                if (images[num].GetComponent<L2_Game_1_V2_myID>().myID == myChoice)
                {
                    choice_Img.sprite = images[num].sprite;
                    choice_Img.gameObject.SetActive(true);
                    images[num].gameObject.SetActive(false);
                    choice_parents.GetComponent<Animator>().SetTrigger("Word_+_Image");
                    StartCoroutine(ImageClickDelay(false, num));
                }
                else
                {
                    imageclickUnLook = false;
                    StartCoroutine(ShakeUIElement(images[num].GetComponent<RectTransform>(), 1, num));

                }
            }
            else
            {
                if (images[num].GetComponent<L2_Game_1_V2_myID>().myID == myChoice)
                {
                    choice_Img.sprite = images[num].sprite;
                    choice_Img.gameObject.SetActive(true);
                    images[num].gameObject.SetActive(false);
                    choice_parents.GetComponent<Animator>().SetTrigger("Word_+_Image");
                    StartCoroutine(ImageClickDelay(false, num));
                }
                else if (images[num].GetComponent<L2_Game_1_V2_myID>().myID == myChoice2)
                {
                    choice_Img.sprite = images[num].sprite;
                    choice_Img.gameObject.SetActive(true);
                    images[num].gameObject.SetActive(false);
                    choice_parents.GetComponent<Animator>().SetTrigger("Word_+_Image");
                    StartCoroutine(ImageClickDelay(false, num));
                }
                else
                {
                    imageclickUnLook = false;
                    StartCoroutine(ShakeUIElement(images[num].GetComponent<RectTransform>(), 1, num));

                }
            }
        }
        else
            return;
    }
    public IEnumerator ImageClickDelay(bool bol, int number)
    {
        imageclickUnLook = bol;

        yield return new WaitForSeconds(1f);
        gameAudio.clip = wordEffects[number];
        gameAudio.Play();
        count++;

        yield return new WaitForSeconds(1f);

        if (count == 4)
        {
            StartCoroutine(SuccessDelay());
        }
        else
        {
            wordclickLook = bol;
            choice_Word.gameObject.SetActive(false);
            choice_Img.gameObject.SetActive(false);
            quiz.sprite = quizSprites[quizList[count]];
            quiz.gameObject.SetActive(true);
            gameAudio.clip = quizSound[quizList[count]];
            gameAudio.Play();
            choice_parents.GetComponent<Animator>().SetTrigger("Word_+_Image_End");
        }
    }

    private IEnumerator ShakeUIElement(RectTransform uiElement, int state, int number)
    {
        //state
        // 0 = word
        // 1 = inmage
        if (state == 0)
        {
            gameAudio.clip = alphabetEffects[number];
            gameAudio.Play();
            while (gameAudio.isPlaying)
            {
                yield return null;
            }
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
        if (state == 1)
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
