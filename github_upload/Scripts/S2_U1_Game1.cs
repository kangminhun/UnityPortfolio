using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_U1_Game1 : MonoBehaviour
{
    public ListScroll listScroll;

    public RectTransform bg;
    public Vector3[] points;
    private RectTransform startPoint;

    public Button[] animalButton;
    public GameObject gameUi;
    public Animation tommyAnimation;
    private int moveindex=0;
    private int count=0;
    private bool clickRock;
    private bool moving;

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
        for (int i = 0; i < gameUi.transform.childCount; i++)
        {
            gameUi.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < animalButton.Length; i++)
        {
            if (i >= 3)
                animalButton[i].enabled = false;
            else
                animalButton[i].enabled = true;
        }
        if (startPoint!=null)
        {
            bg = startPoint;
        }
        Setting();
        StartCoroutine(Move(0));
        moveindex = 0;
        count = 0;
    }
    public void Setting()
    {
        startPoint = bg;
    }
    public IEnumerator Move(int num)
    {
        clickRock = true;
        // 이동에 사용할 시간 (초)
        float duration = 2.0f;

        // 시작 시간
        float startTime = Time.time;

        // 시작 위치
        Vector3 start = bg.localPosition;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        Vector3 target = new Vector3(bg.localPosition.x, points[num].y, 0);

        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = (Time.time - startTime) / duration;
            bg.localPosition = Vector3.Lerp(start, target, t);

            // 한 프레임 대기
            yield return null;
        }
        bg.localPosition = target;
        clickRock = false;
    }
    public void Click(int num)
    {
        int sum = 0;
        if (!clickRock)
        {
            animalButton[num].enabled = false;
            count++;
            if (count == 3)
            {
                sum = 0;
                moveindex++;
                //gameUi.transform.GetChild(sum).gameObject.SetActive(true);
                for (int i = 3; i < 5; i++)
                {
                    animalButton[i].enabled = true;
                }

                moving = true; // 해당 동물을 전부 찍으면 이동 활성화
            }
            else if (count == 5)
            {
                sum = 1;
                moveindex++;
                //gameUi.transform.GetChild(sum).gameObject.SetActive(true);
                animalButton[5].enabled = true;

                moving = true;
            }
            else if (count == 6)
            {
                sum = 2;
                moveindex++;
                //gameUi.transform.GetChild(sum).gameObject.SetActive(true);

                moving = true;
            }
            StartCoroutine(Delay(sum,num));
        }
    }
    public IEnumerator Delay(int sum, int num)
    {
        clickRock = true;
        tommyAnimation.Play("TommyAni");
        while (tommyAnimation.IsPlaying("TommyAni"))
        {
            yield return null;
        }
        clickRock = false;
        gameUi.transform.GetChild(num).gameObject.SetActive(true);
        if (sum != 2 && moving)
        {
            StartCoroutine(Move(moveindex));
        }
        else if (sum == 2)
        {
            SuccessAnimation();
        }
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
}
