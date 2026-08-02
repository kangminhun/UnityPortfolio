using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_U1_Game3 : MonoBehaviour
{
    /*
    클릭시 -> 로봇 팔이 잡아서 당겨온다? -> 옆에 icon의 색을 원래 색으로 변경 (현재 111/111/111/255)
    bg는 천천히 위로 이동 -> 버튼 클릭시 잠깐 멈춤 -> 로봇 팔의 애니메이션 종료 후 다시 이동 -> 끝까지 오면 종료? or 다 찾으면 종료
     */
    public RectTransform bg;
    public RectTransform endPoint;
    public RectTransform startPoint;
    public float bgMoveTime;
    public GameObject[] icons;
    public GameObject[] buttons;
    public GameObject robotL;
    public GameObject robotR;
    public float bgStartPoint;


    private bool remove;
    private bool stop;
    private int count;
    private int buttonNumber;
    [SerializeField]
    private Vector3 robotLStartPoint;
    [SerializeField]
    private Vector3 robotRStartPoint;
    public void OnEnable()
    {
        GameSet();
    }
    public void GameSet()
    {
        robotL.GetComponent<RectTransform>().localPosition = robotLStartPoint;
        robotR.GetComponent<RectTransform>().localPosition = robotRStartPoint;
        buttonNumber = 0;
        count = 0;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
            buttons[i].GetComponent<Button>().enabled = true;
            if (i == 1 || i == 3)
            {
                buttons[i].GetComponent<CloudMovement>().enabled = true;
            }
        }
        StartCoroutine(Move(endPoint));
    }
    public void Click(int num)
    {
        if (!stop)
        {
            buttonNumber = num;
            count++;
            icons[num].GetComponent<Image>().color = Color.white;
            buttons[buttonNumber].GetComponent<Button>().enabled = false;
            if (num == 1 || num == 3)
            {
                buttons[num].GetComponent<CloudMovement>().enabled=false;
            }
            stop = true;
        }
    }
    public IEnumerator Move(RectTransform point)
    {
        // 이동에 사용할 시간 (초)
        float duration = bgMoveTime;

        // 시작 위치
        Vector3 start = bg.localPosition;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        Vector3 target = new Vector3(0, point.localPosition.y, 0);

        float elapsedTime = 0f; // 경과 시간을 저장하는 변수

        while (elapsedTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = elapsedTime / duration;
            bg.localPosition = Vector3.Lerp(start, target, t);

            if (stop)
            {
                if(buttons[buttonNumber].GetComponent<RectTransform>().localPosition.x > 0)
                {
                    yield return StartCoroutine(RobotMove(robotR.GetComponent<RectTransform>(), buttons[buttonNumber].GetComponent<RectTransform>().localPosition, bgStartPoint - bg.localPosition.y));
                    remove = true;
                    buttons[buttonNumber].SetActive(false);
                    yield return StartCoroutine(RobotMove(robotR.GetComponent<RectTransform>(), robotRStartPoint, 0));
                    remove = false;
                }
                else
                {
                    yield return StartCoroutine(RobotMove(robotL.GetComponent<RectTransform>(), buttons[buttonNumber].GetComponent<RectTransform>().localPosition, bgStartPoint - bg.localPosition.y));
                    remove = true;
                    buttons[buttonNumber].SetActive(false);
                    yield return StartCoroutine(RobotMove(robotL.GetComponent<RectTransform>(), robotLStartPoint, 0));
                    remove = false;
                }
                if( count == icons.Length )
                {
                    Debug.Log("성공");
                    yield break;
                }
                stop = false;
            }
            else
            {   
                // 한 프레임 대기
                yield return null;
                // 경과 시간 업데이트
                elapsedTime += Time.deltaTime;
            }
        }

        bg.localPosition = target;

        if (target.y == endPoint.localPosition.y)
            yield return StartCoroutine(Move(startPoint));
        else
            yield return StartCoroutine(Move(endPoint));
    }
    public IEnumerator RobotMove(RectTransform robot, Vector3 point , float minus)
    {
        Debug.Log(minus);
        float duration = 2f;

        // 시작 위치
        Vector3 start = robot.localPosition;
        Vector3 target;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        //if (!remove )//&& (buttonNumber == 1 || buttonNumber == 3))
        target = new Vector3(point.x, point.y - minus, 0);
        //else
        //    target = new Vector3(point.x, point.y, 0);

        float elapsedTime = 0f; // 경과 시간을 저장하는 변수

        while (elapsedTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = elapsedTime / duration;
            robot.localPosition = Vector3.Lerp(start, target, t);
            // 한 프레임 대기
            yield return null;
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
        }
        robot.localPosition = target;
        Debug.Log(target);
    }
}
