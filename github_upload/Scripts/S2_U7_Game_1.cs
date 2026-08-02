using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_U7_Game_1 : MonoBehaviour
{
    public ListScroll listScroll;
    private Animator decibelAni;
    public GameObject decibel;
    public AudioSource audioPlayer;
    public AudioClip[] clips;
    public GameObject soundButtonParent;
    public GameObject[] soundButtons;
    public GameObject[] questionMarkButtons;
    private int myChoice;
    private bool click;
    private int successIndex;


    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간

    private Vector3 originalPosition;

    [Header("Result")]
    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;

    [Header("Particle")]
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;
    public void OnEnable()
    {
        for (int i = 0; i < questionMarkButtons.Length; i++)
        {
            questionMarkButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            questionMarkButtons[i].GetComponent<Button>().enabled = true;
        }
        result.gameObject.SetActive(false);
        GameSet();
    }
    public void GameSet()
    {
        successIndex = 0;
        myChoice = 0;
        Transform parentTransform = soundButtonParent.transform; // 이 스크립트가 부모 오브젝트에 연결되어 있다고 가정합니다.
        int childCount = parentTransform.childCount;
        Transform[] children = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = parentTransform.GetChild(i);
        }

        // 자식 오브젝트들을 무작위로 섞기
        for (int i = 0; i < childCount; i++)
        {
            int randomIndex = Random.Range(i, childCount);
            Transform temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // 자식 오브젝트들의 순서를 바꾸기
        for (int i = 0; i < childCount; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
    public void Decibel(int num)
    {
        myChoice = num;
        decibelAni = decibel.GetComponent<Animator>();
        decibelAni.SetTrigger(num.ToString());
        switch(num)
        {
            case 40:
                soundButtons[0].GetComponent<Animator>().SetTrigger("On");
                audioPlayer.clip = clips[0];
                audioPlayer.Play();
                break;
            case 70:
                soundButtons[1].GetComponent<Animator>().SetTrigger("On");
                audioPlayer.clip = clips[1];
                audioPlayer.Play();
                break;
            case 100:
                soundButtons[2].GetComponent<Animator>().SetTrigger("On");
                audioPlayer.clip = clips[2];
                audioPlayer.Play();
                break;
            case 120:
                soundButtons[3].GetComponent<Animator>().SetTrigger("On");
                audioPlayer.clip = clips[3];
                audioPlayer.Play();
                break;
            case 130:
                soundButtons[4].GetComponent<Animator>().SetTrigger("On");
                audioPlayer.clip = clips[4];
                audioPlayer.Play();
                break;
            case 140:
                audioPlayer.clip = clips[5];
                audioPlayer.Play();
                break;
        }
    }
    public void QuestionMarkButton(int data)
    {
        if (!click)
        {
            int dataindex = 0;
            switch (data)
            {
                case 40:
                    dataindex = 0;
                    break;
                case 70:
                    dataindex = 1;
                    break;
                case 100:
                    dataindex = 2;
                    break;
                case 120:
                    dataindex = 3;
                    break;
                case 130:
                    dataindex = 4;
                    break;
                case 140:
                    dataindex = 5;
                    break;
            }
            if (myChoice == data)
            {
                switch (dataindex)
                {
                    case 0:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                    case 1:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                    case 2:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                    case 3:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                    case 4:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                    case 5:
                        questionMarkButtons[dataindex].transform.GetChild(0).gameObject.SetActive(true);
                        questionMarkButtons[dataindex].GetComponent<Button>().enabled = false;
                        break;
                }
                successIndex++;
                if(successIndex==6)
                {
                    Debug.Log("성공");
                    SuccessAnimation();
                }
            }
            else
            {
                StartCoroutine(ShakeUIElement(questionMarkButtons[dataindex].GetComponent<RectTransform>()));
            }
        }
    }
    private IEnumerator ShakeUIElement(RectTransform uiElement)
    {
        click = true;
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
    private void SuccessAnimation()
    {
        // 성공 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 성공 이미지나 파티클 효과를 재생할 수 있습니다
        //isTimerRunning = false;
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
}
