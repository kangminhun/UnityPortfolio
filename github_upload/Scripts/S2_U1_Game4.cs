using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_U1_Game4 : MonoBehaviour
{
    public RectTransform bg;
    public RectTransform blackBg;
    public RectTransform player;
    public Vector3 bg_StartPoint;
    public Vector3 bg_EtartPoint;
    public Vector3 blackBg_StartPoint;
    public Vector3 blackBg_EndPoint;

    public GameObject[] rounds;

    private bool hit;
    private int count;
    private bool stop;
    public void OnEnable()
    {
        GameSet();
    }
    public void GameSet()
    {
        count = 0;
        stop = false;
        hit = false;

        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }
        player.transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(Move(bg_EtartPoint,bg_StartPoint, bg));
        StartCoroutine(Move(blackBg_EndPoint, blackBg_StartPoint, blackBg));

        rounds[count].SetActive(true);
        for (int j = 0; j < rounds.Length; j++)
        {
            for (int i = 0; i < rounds[j].transform.childCount; i++)
            {
                if (rounds[j].transform.GetChild(i).GetComponent<PlanetMovement>() != null)
                    rounds[j].transform.GetChild(i).GetComponent<PlanetMovement>().enabled = true;
            }
        }
    }
    public void MoveButton(string str)
    {
        if (!hit)
        {// 이동 거리를 조절하려면 이 변수를 변경하세요.
            float moveDistance = 100f; // 필요에 따라 조절하세요.

            Vector3 currentPosition = player.localPosition;

            if (str == "Right")
            {
                // "Right" 버튼이 눌렸을 때, 플레이어를 오른쪽으로 이동시키며 최대 위치를 넘지 않도록 합니다.
                Vector3 newPosition = new Vector3(
                    Mathf.Min(currentPosition.x + moveDistance, 898f),
                    currentPosition.y,
                    currentPosition.z
                );
                player.localPosition = newPosition;
            }
            else if (str == "Left")
            {
                // "Left" 버튼이 눌렸을 때, 플레이어를 왼쪽으로 이동시키며 최소 위치를 넘지 않도록 합니다.
                Vector3 newPosition = new Vector3(
                    Mathf.Max(currentPosition.x - moveDistance, -898f),
                    currentPosition.y,
                    currentPosition.z
                );
                player.localPosition = newPosition;
            }
        }
    }
    public void Hit()
    {
        hit = true;
        Debug.Log("실패");
    }

    public IEnumerator Move(Vector3 endPoint,Vector3 startPoint,RectTransform moving_Target)
    {
        if (stop)
            yield break;
        // 이동에 사용할 시간 (초)
        float duration = 30f;

        // 시작 시간
        float startTime = Time.time;

        // 시작 위치
        Vector3 start = moving_Target.localPosition;

        // 목표 위치 (points 배열 중 첫 번째 점의 위치)
        Vector3 target = new Vector3(0, endPoint.y, 0);

        while (Time.time - startTime < duration)
        {
            // 현재 시간에 따라 보간된 위치 계산
            float t = (Time.time - startTime) / duration;
            moving_Target.localPosition = Vector3.Lerp(start, target, t);
            if(hit)
            {
                yield break;
            }
            // 한 프레임 대기
            yield return null;
        }
        moving_Target.localPosition = target;
        moving_Target.localPosition = startPoint;
        if (moving_Target == bg)
        {
            Ending();
        }
        yield return Move(endPoint,startPoint, moving_Target);
    }
    public void Ending()
    {
        if (count != player.transform.childCount - 1)
        {
            player.transform.GetChild(count).gameObject.SetActive(false);
            rounds[count].SetActive(false);
            count++;
            rounds[count].SetActive(true);
            player.transform.GetChild(count).gameObject.SetActive(true);
        }
        else
        {
            stop = true;
            Debug.Log("성공");
        }
    }
}
