using System.Collections;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vimeo.SimpleJSON;

public class PointManager : MonoBehaviour
{
    private string myPointUrl= "https://your-server-domain.com/v1/goods/my-goods";
    [HideInInspector] public int myPoint;
    public Text myPointTxt;
    [HideInInspector] public int myDiamond;
    public Text myDiamondTxt;
    public Text userNameTxt;
    public void MyPoint(string token)
    {
        StartCoroutine(Point(token));
    }
    public IEnumerator Point(string token)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(myPointUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);
            // JSON 파싱
            JSONNode json = JSONClass.Parse(request.downloadHandler.text);

            // "data" 객체에서 "point" 키에 해당하는 값을 가져옴
            int pointValue = json["data"]["point"].AsInt;   
            int diaValue = json["data"]["diamond"].AsInt;
            string username = json["data"]["member"];

            // 가져온 값 출력
            myPoint = pointValue;
            myPointTxt.text = myPoint.ToString();
            myDiamond = diaValue;
            myDiamondTxt.text = myDiamond.ToString();
            userNameTxt.text = username;
            DataBase.instance.LoginManager.NoticeSetting();
            DataBase.instance.CopyTxt.Set();
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Point(token));
        }
    }
    public void PointUp(int point)
    {
        DataBase.instance.WebRequestManager.PointRequest(point);
        myPointTxt.text = myPoint.ToString();
        Debug.Log("Point up");
    }
}
