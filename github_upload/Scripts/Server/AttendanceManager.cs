using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vimeo.SimpleJSON;

public class AttendanceManager : MonoBehaviour
{
    [SerializeField] Uichage uichage;
    [SerializeField] private RectTransform[] points;
    [SerializeField] private GameObject target;
    [SerializeField] private Text todayText;
    [SerializeField] private GameObject main;
    public int count;
    public void AttendanceOpen()
    {
        uichage.UIViewControllerOpen("Attendance");
    }
    public void AttendanceClose()
    {
        main.transform.Find("Daily").gameObject.SetActive(false);
        target.GetComponent<RectTransform>().localPosition = new Vector3(points[count - 1].localPosition.x, 426, 0);
        uichage.UIViewControllerClose("Attendance");
    }
    public void DailyGiftButton()
    {
        main.transform.Find("Daily").gameObject.SetActive(true);
        int data = main.transform.Find("Daily").transform.Find("Map").transform.childCount;
        for (int i = 0; i < data; i++)
        {
            StartCoroutine(main.transform.Find("Daily").transform.Find("Map").transform.GetChild(i).GetComponent<Rotation_valueMovement>().RotateCoroutine());
        }
       StartCoroutine(TargetMovement());
    }
    public void Attendance()
    {
        StartCoroutine(AttendanceSet());
    }
    private IEnumerator TargetMovement()
    {
        while (target.GetComponent<RectTransform>().localPosition.y > points[count -1].localPosition.y)
        {
            float newY = Mathf.MoveTowards(target.GetComponent<RectTransform>().localPosition.y, points[count - 1].localPosition.y, Time.deltaTime * 250);
            target.GetComponent<RectTransform>().localPosition = new Vector3(points[count - 1].localPosition.x, newY, 0);

            yield return null;
        }
    }
    public IEnumerator AttendanceSet()
    {
        string url = "https://your-server-domain.com/v1/attendance/data?point=100";
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "");

        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공");
        }
        StartCoroutine(WeekAttendanceSet());
    }
    public IEnumerator WeekAttendanceSet()
    {
        string url = "https://your-server-domain.com/v1/attendance/weekly";
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        JSONNode json = JSONClass.Parse(request.downloadHandler.text);
        // "data" 객체에서 "point" 키에 해당하는 값을 가져옴
        int attendanceCount = json["data"]["attendanceCount"].AsInt;

        DateTime currentDate = DateTime.Now;
        Debug.Log(currentDate);
        todayText.text= currentDate.ToString("yyyy/MM/dd");
        count = attendanceCount;
        Debug.Log(count);
        target.GetComponent<RectTransform>().localPosition = new Vector3(points[count].localPosition.x, 426, 0);
    }
}
