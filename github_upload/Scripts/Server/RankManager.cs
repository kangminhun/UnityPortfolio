using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vimeo.SimpleJSON;

public class RankManager : MonoBehaviour
{
    [SerializeField] private Uichage uichage;
    private const string rankUrl = "https://your-server-domain.com/v1/goods/ranking";
    private const string myRankUrl = "https://your-server-domain.com/v1/goods/my-rank";
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Sprite[] rankImg;
    private List<GameObject> rankList;
    [SerializeField] private Text myTrophy;
    private int myRankingPoint;
    private void Start()
    {
        rankList = new List<GameObject>();
    }
    public void RankOpen()
    {
        uichage.UIViewControllerOpen("Ranking");
    }
    public void RankClose()
    {
        uichage.UIViewControllerClose("Ranking");
    }
    public void MY_Ranking()
    {
        StartCoroutine(MyRankingPoint());
    }
    private IEnumerator MyRankingPoint()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(myRankUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);
            // JSON 파싱
            JSONNode json = JSONClass.Parse(request.downloadHandler.text);
            myRankingPoint = json["data"]["rankingPoint"].AsInt;
            myTrophy.text = myRankingPoint.ToString();
            Debug.Log($"myRankingPoint : {myRankingPoint}");
        }
    }
    public void Rank()
    {
        StartCoroutine(RankSet());
    }
    private IEnumerator RankSet()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(rankUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);
            // JSON 파싱
            JSONNode json = JSONClass.Parse(request.downloadHandler.text);
            JSONArray listArray = json["list"].AsArray;

            List<JSONNode> sortedList = new List<JSONNode>(listArray.Count);
            foreach (JSONNode node in listArray)
            {
                sortedList.Add(node);
            }

            // ranking 값을 기준으로 정렬
            sortedList.Sort((a, b) => a["ranking"].AsInt.CompareTo(b["ranking"].AsInt));

            if (rankList.Count != 0)
            {
                for (int i = 0; i < rankList.Count; i++)
                {
                    Destroy(rankList[i]);
                }
            }
            if (sortedList.Count >= 20) 
            {
                for (int i = 0; i < Mathf.Min(20, sortedList.Count); i++)
                {
                    string member = sortedList[i]["member"];
                    int ranking = sortedList[i]["ranking"].AsInt;
                    int rankingPoint = sortedList[i]["rankingPoint"].AsInt;
                    GameObject createObj = Instantiate(rankPrefab, uichage.uIViewContainer.transform.Find("Ranking").gameObject.GetComponentInChildren<ScrollRect>().content);
                    if (ranking == 1)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[0];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else if (ranking == 2)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[1];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else if (ranking == 3)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[2];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[3];
                        createObj.GetComponent<UIShiny>().enabled = false;
                        createObj.GetComponent<Animator>().enabled = false;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-460, -3f, 0);
                    }
                    createObj.transform.Find("Name").GetComponent<Text>().text = member;
                    createObj.transform.Find("Ranking").GetComponent<Text>().text = ranking.ToString();
                    createObj.transform.Find("RankingPoint").GetComponent<Text>().text = rankingPoint.ToString();
                    rankList.Add(createObj);
                }
            }
            else
            {
                for (int i = 0; i < sortedList.Count; i++)
                {
                    string member = sortedList[i]["member"];
                    int ranking = sortedList[i]["ranking"].AsInt;
                    int rankingPoint = sortedList[i]["rankingPoint"].AsInt;
                    GameObject createObj = Instantiate(rankPrefab, uichage.uIViewContainer.transform.Find("Ranking").gameObject.GetComponentInChildren<ScrollRect>().content);
                    if (ranking == 1)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[0];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else if (ranking == 2)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[1];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else if (ranking == 3)
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[2];
                        createObj.GetComponent<UIShiny>().enabled = true;
                        createObj.GetComponent<Animator>().enabled = true;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-356, -3f, 0);
                    }
                    else
                    {
                        createObj.GetComponent<Image>().sprite = rankImg[3];
                        createObj.GetComponent<UIShiny>().enabled = false;
                        createObj.GetComponent<Animator>().enabled = false;
                        createObj.transform.Find("Ranking").transform.localPosition = new Vector3(-460, -3f, 0);
                    }
                    createObj.transform.Find("Name").GetComponent<Text>().text = member;
                    createObj.transform.Find("Ranking").GetComponent<Text>().text = ranking.ToString();
                    createObj.transform.Find("RankingPoint").GetComponent<Text>().text = rankingPoint.ToString();
                    rankList.Add(createObj);
                }
            }

            yield return new WaitForSeconds(10f);
            yield return StartCoroutine(RankSet());
        }
    }
}
