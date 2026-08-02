using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class L2Game2 : MonoBehaviour
{
    public ListScroll listScroll;

    //public Sprite[] backgroundImgs;
    public GameObject IsButtons;
    public GameObject areButtons;
    public GameObject titlImg;
    public Sprite[] titlImgs;
    public GameObject sentenceImg;
    public string[] sentencetxts;
    public string[] answers;

    private int count;
    private Button[] buttons;
    private bool returnButton;
    private bool sentenceImgClick;

    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;

    public GameObject[] isButtons_Ch;
    public GameObject[] areButtons_Ch;

    public AudioSource sentenceAudio;
    public AudioClip[] sentenceClips;
    private bool stop;
    public void OnEnable()
    {
        result.SetActive(false);
    }
    public void GameSet()
    {
        count = 0;
        returnButton = false;
        sentenceImgClick = false;
        IsButtons.SetActive(true);
        areButtons.SetActive(false);
        sentenceImg.SetActive(false);
        titlImg.GetComponent<Image>().sprite = titlImgs[0];
        titlImg.SetActive(true);
        for (int i = 0; i < sentenceImg.transform.childCount; i++)
        {
            sentenceImg.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < isButtons_Ch.Length; i++)
        {
            isButtons_Ch[i].gameObject.SetActive(false);

        }
        for (int i = 0; i < areButtons_Ch.Length; i++)
        {
            areButtons_Ch[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < IsButtons.transform.childCount; i++)
        {
            if (IsButtons.transform.GetChild(i).GetComponent<Button>() != null)
            {
                IsButtons.transform.GetChild(i).GetComponent<Button>().enabled = true;
                IsButtons.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = answers[0];
            }
            else
            {
                IsButtons.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = answers[1];
            }
        }
        for (int i = 0; i < areButtons.transform.childCount; i++)
        {
            if (areButtons.transform.GetChild(i).GetComponent<Button>() != null)
            {
                areButtons.transform.GetChild(i).GetComponent<Button>().enabled = true;
                areButtons.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = answers[1];
            }
            else
            {
                areButtons.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = answers[0];
            }
        }
    }
    public void Click(int num)
    {
        if (returnButton)
        {
            buttons = areButtons.transform.GetComponentsInChildren<Button>();
        }
        else
        {
            buttons = IsButtons.transform.GetComponentsInChildren<Button>();
        }
        if (count < 3)
        {
            count++;
            buttons[num].transform.GetChild(1).gameObject.SetActive(true);
            buttons[num].enabled = false;
            sentenceAudio.clip = sentenceClips[4];
            sentenceAudio.Play();
        }
        if (count == 3)
        {
            StartCoroutine(ReturnButton(buttons[num]));
        }
    }
    public IEnumerator ReturnButton(Button button)
    {
        Animator animator = button.transform.GetChild(1).gameObject.GetComponent<Animator>();
        if (!returnButton)
        {
            returnButton = true;
            count = 0;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + .5f);
            IsButtons.SetActive(false);
            areButtons.SetActive(true);
            titlImg.GetComponent<Image>().sprite = titlImgs[1];
        }
        else
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + .5f);
            areButtons.SetActive(false);
            //GetComponent<Image>().sprite = backgroundImgs[1];
            sentenceImg.SetActive(true);
            sentenceImg.GetComponent<TextMeshProUGUI>().text = sentencetxts[0];
            titlImg.GetComponent<Image>().sprite = titlImgs[0];
            sentenceAudio.clip = sentenceClips[0];
            sentenceAudio.Play();
        }
    }
    public void SentenceButtonClick()
    {
        if (!stop)
            StartCoroutine(SentenceButton());
    }
    public IEnumerator SentenceButton()
    {
        stop = true;
        if (!sentenceImgClick)
        {
            sentenceAudio.clip = sentenceClips[1];
            sentenceAudio.Play();
            StringChange(sentenceImg.GetComponent<TextMeshProUGUI>().text, 0);
            while (sentenceAudio.isPlaying)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);

            sentenceImg.GetComponent<TextMeshProUGUI>().text = sentencetxts[1];
            titlImg.GetComponent<Image>().sprite = titlImgs[1];
            sentenceAudio.clip = sentenceClips[2];
            sentenceAudio.Play();
            sentenceImgClick = true;
        }
        else
        {
            sentenceAudio.clip = sentenceClips[3];
            sentenceAudio.Play();
            StringChange(sentenceImg.GetComponent<TextMeshProUGUI>().text, 1);
            while (sentenceAudio.isPlaying)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            SuccessAnimation();
        }
        yield return null;
        stop = false;
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
        resultAudio.clip = resultAudioClips[0];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Success");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[0];
        resultButton.onClick.AddListener(() => End());
        DataBase.instance.PointManager.PointUp(100);
        result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = "100";
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
    public void StringChange(string str, int num)
    {
        // 패턴을 찾기 (대소문자 무시)
        int startIndex;
        string pattern = answers[num];

        // 단어가 문장의 첫 번째로 오는지 확인
        if (str.StartsWith(pattern, StringComparison.OrdinalIgnoreCase))
        {
            startIndex = 0;
        }
        else
        {
            // 단어 앞뒤에 공백이 있는지 확인
            pattern = "\\b" + Regex.Escape(pattern) + "\\b";
            Match match = Regex.Match(str, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                startIndex = match.Index;
            }
            else
            {
                // 일치하는 부분이 없을 때
                Debug.Log("일치하는 부분이 없습니다.");
                return;
            }
        }

        // 일치하는 부분의 길이
        int length = answers[num].Length;

        // 새로운 문자열 만들기
        string coloredStr = str.Substring(0, startIndex) +
                            "<color=green>" + str.Substring(startIndex, length) + "</color>" +
                            str.Substring(startIndex + length);

        sentenceImg.GetComponent<TextMeshProUGUI>().text = coloredStr;
    }
}
