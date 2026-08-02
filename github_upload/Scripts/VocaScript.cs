using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VocaScript : MonoBehaviour
{
    public TextMeshProUGUI explanationTxt;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI[] exampleTxts;
    public VocaScriptableObject[] vocas;
    public AudioSource audioSource;
    private AudioClip[] audioClips;


    public float moveSpeed = 2.0f;
    private Vector3[] targetPositions;
    private int currentTargetIndex = 0;
    public GameObject arrow;

    public GameObject[] types;

    public GameObject[] mainCh;
    public Animator mainAni;
    public Button[] buttons;
    public Image[] buttonImgs;
    public Sprite[] buttonSprites;
    public GameObject[] buttonparents;
    [HideInInspector] public string[] names;
    public TextMeshProUGUI[] nametexts;
    public GameObject imageStagePrefab;
    private GameObject imageStage;
    private int prenumber;
    private int pretypeNumber;
    public void GaemStart()
    {
        if (imageStage != null)
        {
            Destroy(imageStage);
        }
        imageStage = Instantiate(imageStagePrefab, gameObject.transform.Find("Bg_1").gameObject.transform);
        imageStage.transform.SetAsFirstSibling();

        mainAni = imageStage.GetComponent<Animator>();
        mainCh = new GameObject[imageStage.transform.childCount];
        for (int i = 0; i < imageStage.transform.childCount; i++)
        {
            int sum = i;
            mainCh[sum] = imageStage.transform.GetChild(sum).gameObject;
        }
        for (int i = 0; i < nametexts.Length; i++)
        {
            nametexts[i].text = names[i];
        }
        targetPositions = new Vector3[]
          {
            new Vector3(9, -186, 0),
            new Vector3(6, -237, 0),
            new Vector3(2, -293, 0),
            new Vector3(-1, -342, 0),
            new Vector3(-5, -390, 0)
          };
        for (int i = 0; i < buttons.Length; i++)
        {
            int sum = i;
            buttons[sum].onClick.RemoveAllListeners();
            buttons[sum].onClick.AddListener(() => Click(sum, vocas[sum].type));
            buttonImgs[sum].GetComponent<Image>().sprite = buttonSprites[vocas[sum].type];
        }
        for (int i = 0; i < mainCh.Length; i++)
        {
            mainCh[i].SetActive(false);
        }
        for (int i = 0; i < buttonparents.Length; i++)
        {
            buttonparents[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vocas.Length; i++)
        {
            int sum = i;
            buttonparents[sum].gameObject.SetActive(true);
        }
        MoveToTarget(targetPositions[currentTargetIndex]);
        Click(0, vocas[0].type);
    }
    IEnumerator TypingStart(TextMeshProUGUI txt, string talk)
    {
        txt.text = null;

        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");

        for (int i = 0; i < talk.Length; i++)
        {
            txt.text += talk[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void Click(int number, int typeNumber)
    {
        StopAllCoroutines();
        prenumber = number;
        pretypeNumber = typeNumber;
        audioClips = new AudioClip[5];
        nameTxt.text = names[number];
        for (int i = 0; i < mainCh.Length; i++)
        {
            mainCh[i].SetActive(false);
        }
        mainCh[number].SetActive(true);

        StartCoroutine(TypingStart(explanationTxt, vocas[number].explanationStringData));
        for (int i = 0; i < exampleTxts.Length; i++)
        {
            int sum = i;
            StartCoroutine(TypingStart(exampleTxts[sum], vocas[number].exampleStringDatas[sum]));
        }
        ArrowMovement(number);
        audioClips = vocas[number].audioClips;

        for (int i = 0; i < types.Length; i++)
        {
            types[i].SetActive(false);
        }
        types[typeNumber].SetActive(true);
        audioSource.clip = audioClips[0];
        audioSource.Play();
        mainAni.SetTrigger(number.ToString());
    }
    public void AudioPlay(int num)
    {
        audioSource.clip = audioClips[num];
        audioSource.Play();
    }
    public void ArrowMovement(int num)
    {
        if (num >= 0 && num < targetPositions.Length)
        {
            currentTargetIndex = num;
            //StopAllCoroutines();
            StartCoroutine(MoveToTarget(targetPositions[currentTargetIndex]));
        }
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        Vector3 startPosition = arrow.GetComponent<RectTransform>().localPosition;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        float distanceCovered = 0;

        while (distanceCovered < journeyLength)
        {
            float distanceMoved = (Time.time - startTime) * moveSpeed;
            distanceCovered = Mathf.Min(distanceMoved, journeyLength);
            float fractionOfJourney = distanceCovered / journeyLength;

            arrow.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }
    }
}
