using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_U1_Game2 : MonoBehaviour
{
    public ListScroll listScroll;

    public RectTransform enamyStartPoint;
    public RectTransform chameleonStartPoint;
    public RectTransform chameleonEndPoint;
    public RectTransform bg;
    public Vector3[] points;
    public GameObject chameleon;
    public GameObject eagle;
    public GameObject owl;
    public RectTransform appearancePoint;
    public RectTransform enemyEndPoint;
    public Vector3 startPoint;
    [SerializeField]
    private bool hide;

    [Space(5)]
    public Image[] chameleonColorImgs;
    public Sprite[] startImg;
    public Sprite[] yImg;
    public Sprite[] bImg;
    public Sprite[] gImg;
    [SerializeField]
    private Sprite[] choseImgs;
    // 0 -> Body , 1 -> 왼쪽 앞발 , 2 -> 왼쪽 뒷발 , 3 -> 오른쪽 앞발, 4 -> 오른쪽 뒷발

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
        ReStart();
    }
    public void ReStart()
    {
        result.SetActive(false);
        //startUi.SetActive(true);
        StartCoroutine(GameSet());
    }
    public void End()
    {
        listScroll.CloseUi();
    }
    public IEnumerator GameSet()
    {
        eagle.GetComponent<RectTransform>().localPosition = enamyStartPoint.localPosition;
        owl.GetComponent<RectTransform>().localPosition = enamyStartPoint.localPosition;
        chameleon.GetComponent<RectTransform>().localPosition = chameleonStartPoint.localPosition;

        chameleon.GetComponent<Animator>().enabled = true;
        bg.GetComponent<RectTransform>().localPosition = startPoint;
        choseImgs = new Sprite[chameleonColorImgs.Length];
        for (int i = 0; i < chameleonColorImgs.Length; i++)
        {
            chameleonColorImgs[i].sprite = startImg[i];
        }

        choseImgs = yImg;
        yield return StartCoroutine(Move(0));
        chameleon.GetComponent<Animator>().enabled = false;
        yield return StartCoroutine(EnemyMovement(eagle, appearancePoint,2f));
        yield return new WaitForSeconds(1.5f);
        if (!hide)
        {
            Debug.Log("죽음");
            FailureAnimation();
            yield break;
        }
        else
        {
            Debug.Log("숨음");
            yield return StartCoroutine(EnemyMovement(eagle, enemyEndPoint,2f));
        }
        chameleon.GetComponent<Animator>().enabled = true;
        choseImgs = bImg;
        yield return StartCoroutine(Move(1));
        chameleon.GetComponent<Animator>().enabled = false;
        yield return StartCoroutine(EnemyMovement(owl, appearancePoint,2f));
        yield return new WaitForSeconds(1.5f);
       
        if (!hide)
        {
            Debug.Log("죽음");
            FailureAnimation();
            yield break;
        }
        else
        {
            Debug.Log("숨음");
            yield return StartCoroutine(EnemyMovement(owl, enemyEndPoint,2f));
        }
        chameleon.GetComponent<Animator>().enabled = true;
        choseImgs = gImg;
        yield return StartCoroutine(Move(2));
        chameleon.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(1.5f);

        if (!hide)
        {
            Debug.Log("죽음");
            FailureAnimation();
            yield break;
        }
        else
        {
            Debug.Log("숨음");
            yield return StartCoroutine(Ending());
        }
    }
    public IEnumerator Ending()
    {
        chameleon.GetComponent<Animator>().enabled = true;
        yield return StartCoroutine(Move(3));
        yield return StartCoroutine(EnemyMovement(chameleon, chameleonEndPoint,1f));
        SuccessAnimation();
    }
    public IEnumerator ColorChange(Sprite[] sprites)
    {

        for (int i = 0; i < chameleonColorImgs.Length; i++)
        {
            chameleonColorImgs[i].sprite = sprites[i];
        }
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < chameleonColorImgs.Length; i++)
        {
            chameleonColorImgs[i].sprite = startImg[i];
        }
    }

    public void HideButton()
    {
        StartCoroutine(HideButtonCoroutine());
    }

    public IEnumerator HideButtonCoroutine()
    {
        hide = true;
        StartCoroutine(ColorChange(choseImgs));
        yield return new WaitForSeconds(10f);
        hide = false;
    }

    public IEnumerator EnemyMovement(GameObject enemyObj,RectTransform point, float time)
    {
        // 이동에 사용할 시간 (초)
        float duration = time;

        // 시작 시간
        float startTime = Time.time;

        // 시작 위치
        Vector3 start = enemyObj.GetComponent<RectTransform>().localPosition;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        Vector3 target = new Vector3(point.localPosition.x, point.localPosition.y, 0);

        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = (Time.time - startTime) / duration;
            enemyObj.GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, target, t);

            // 한 프레임 대기
            yield return null;
        }
        enemyObj.GetComponent<RectTransform>().localPosition = target;

    }
    public IEnumerator Move(int num)
    {
        // 이동에 사용할 시간 (초)
        float duration = 10.0f;

        // 시작 시간
        float startTime = Time.time;

        // 시작 위치
        Vector3 start = bg.localPosition;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        Vector3 target = new Vector3(points[num].x, 0, 0);

        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = (Time.time - startTime) / duration;
            bg.localPosition = Vector3.Lerp(start, target, t);

            // 한 프레임 대기
            yield return null;
        }
        bg.localPosition = target;
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
}
