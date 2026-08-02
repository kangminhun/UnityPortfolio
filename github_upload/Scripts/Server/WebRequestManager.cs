using System;
using System.Collections;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vimeo.SimpleJSON;

[System.Serializable]
public class PasswordData
{
    public string username;
    public string password;
}
[System.Serializable]
public class NewUserJoin
{
    public string contact;
    public string dateBirth;
    public string name;
    public string password;
    public string passwordRe;
    public string username;
}
[System.Serializable]
public class Find_UserID
{
    public string contact;
    public string name;
}
[System.Serializable]
public class PutPassword
{
    public string changePassword;
    public string changePasswordRe;
    public string currentPassword;
}
[System.Serializable]
public class Notice
{
    public string content;
    public string title;
}
[System.Serializable]
public class Card
{
    public string cardName;
    public string cardType;
    public int diamond;
    public string memo;
    public int point;
    public int sid;
    public int star;
    public int healthPoint;
    public int power;
}
[System.Serializable]
public class ResponseData
{
    public bool isPasswordMatch;
}
[System.Serializable]
public class UpgradeUser
{
    public string contact;
    public string name;
}
public enum UserType
{
    user,
    admin
}

public class WebRequestManager : MonoBehaviour
{
    private const string loginUserServerUrl = "https://your-server-domain.com/v1/member/login/app/user"; // 회원 로그인 서버의 URL로 변경해야 합니다
    private const string loginAdminServerUrl = "https://your-server-domain.com/v1/member/login/app/admin"; // 관리자 로그인 서버의 URL로 변경해야 합니다
    private const string joinServerUrl = "https://your-server-domain.com/v1/member/join"; // 회원가입 서버의 URL로 변경해야 합니다
    private const string IDCheckUrl = "https://your-server-domain.com/v1/member/new-username";
    private const string findServerUrl = "https://your-server-domain.com/v1/member/find-username"; // 아이디 찾기
    private const string ChangePasswordServerUrl = "https://your-server-domain.com/v1/member/password"; // 비번 변경
    private const string pointModify = "https://your-server-domain.com/v1/goods/point";
    private const string noticeUrl = "https://your-server-domain.com/v1/notice/data";
    private const string noticeAllUrl = "https://your-server-domain.com/v1/mail/all";
    private const string cardCreateUrl = "https://your-server-domain.com/v1/card/data";
    private const string cardBuyUrl = "https://your-server-domain.com/v1/card-purchase/card-id";
    private const string cardAllUrl= "https://your-server-domain.com/v1/card/all";
    private const string upgradeUrl = "https://your-server-domain.com/v1/member/premium";
    private const string myTypeUrl = "https://your-server-domain.com/v1/member/is-premium";
    private const string allmemberInfoURl = "https://your-server-domain.com/v1/member/all";
    private const string zoomUrl = "https://your-server-domain.com/v1/zoom/create-meeting";
    private const string zoomJoinUrl = "https://your-server-domain.com/v1/zoom/join-meeting";
    private const string myClassUrl = "https://your-server-domain.com/v1/open-unit/my";
    private const string myCardGame = "https://your-server-domain.com/v1/card-game/my";
    private const string cardGaemRound = "https://your-server-domain.com/v1/card-game/round";
    private const string cardGaemStage = "https://your-server-domain.com/v1/card-game/stage";

    public GameObject resultUI;
    public Sprite[] resultUI_Imgs;
    [HideInInspector] public bool loginSuccess;
    [HideInInspector] public string token;
    private string loginServerUrl;
    public Toggle loginToggle;
    public UserType type;
    [HideInInspector] public string myMemberID;
    [HideInInspector] public int l1openIndex;
    [HideInInspector] public int l2openIndex;
    [HideInInspector] public int s1openIndex;
    [HideInInspector] public int s2openIndex;
    [HideInInspector] public int smopenIndex;
    public void PasswordRequest(string userInputUsername, string userInputPassword)
    {

        PasswordData data = new PasswordData
        {
            username = userInputUsername,
            password = userInputPassword
        };

        string jsonData = JsonUtility.ToJson(data);

        StartCoroutine(Login(jsonData));
    }
    public void PointRequest(int point)
    {
        StartCoroutine(PointUp(point));
    }
    public IEnumerator PointUp(int point)
    {
        string urlWithParams = $"{pointModify}?point={point}";

        // UnityWebRequest 생성
        UnityWebRequest request = UnityWebRequest.Put(urlWithParams, "");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        // 요청 보내기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PUT request successful");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
    private IEnumerator Login(string jsonData)
    {
        if (!loginToggle.isOn)
        {
            loginServerUrl = loginUserServerUrl;
            type = UserType.user;
        }
        else
        {
            loginServerUrl = loginAdminServerUrl;
            type = UserType.admin;
        }
        // UnityWebRequest를 사용하여 서버에 POST 요청을 보냄
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(loginServerUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                JSONNode json = JSONClass.Parse(request.downloadHandler.text);

                string username = json["data"]["name"];
                myMemberID = json["data"]["memberId"];

                Token(request.downloadHandler.text);
                resultUI.GetComponentInChildren<Text>().text = "Login Successful";

                StartCoroutine(myType());

                DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(false);
                DataBase.instance.uis[0].transform.GetChild(3).gameObject.SetActive(true);

                DataBase.instance.uis[0].transform.GetChild(3).transform.GetComponentInChildren<Text>().text = username;
                loginSuccess = true;
                //DataBase.instance.LoginManager.test.text = request.downloadHandler.text;
            }
            else
            {
                resultUI.SetActive(true);
                resultUI.GetComponent<Image>().sprite = resultUI_Imgs[1];
                resultUI.GetComponentInChildren<Text>().text = "Login failed";
                DataBase.instance.AudioManager.AudioPlay(0);
                DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(false);
                DataBase.instance.uis[0].transform.GetChild(2).gameObject.SetActive(true);
                DataBase.instance.LoginManager.clickH.SetActive(true);
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
                //DataBase.instance.LoginManager.test.text= request.downloadHandler.text;
            }
        }
    }
    public void LoginSuccess()
    {
        if (!loginToggle.isOn)
        {
            DataBase.instance.PointManager.MyPoint(token);
            DataBase.instance.RankManager.Rank();
            DataBase.instance.RankManager.MY_Ranking();
        }
    }
    private IEnumerator myType()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(myTypeUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                JSONNode json = JSONClass.Parse(request.downloadHandler.text);

                string datePremium = json["data"]["datePremium"];
                string memberGroup = json["data"]["memberGroup"];

                if (memberGroup == "유료회원")
                {
                    DataBase.instance.LoginManager.memberType = Member_group.ROLE_PREMIUM;
                    StartCoroutine(MyClass("LITERACY_1"));
                    StartCoroutine(MyClass("LITERACY_2A"));
                    StartCoroutine(MyClass("SCIENCE_1"));
                    StartCoroutine(MyClass("SCIENCE_2"));
                    StartCoroutine(MyClass("MATH"));
                }
                else if (memberGroup == "테스트")
                    DataBase.instance.LoginManager.memberType = Member_group.ROLE_TEST;
                else
                    DataBase.instance.LoginManager.memberType = Member_group.ROLE_USER;

                if (datePremium != "-")
                    DataBase.instance.LoginManager.date_premium = DateTime.ParseExact(datePremium, "yyyy-MM-dd", null);
            }
            else
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    private IEnumerator MyClass(string path)
    {
        string url = myClassUrl + "?" + "subjectName=" + path;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                switch (path)
                {
                    case "LITERACY_1":
                        l1openIndex = int.Parse(request.downloadHandler.text);
                        break;
                    case "LITERACY_2A":
                        l2openIndex = int.Parse(request.downloadHandler.text);
                        break;
                    case "SCIENCE_1":
                        s1openIndex = int.Parse(request.downloadHandler.text);
                        break;
                    case "SCIENCE_2":
                        s2openIndex = int.Parse(request.downloadHandler.text);
                        break;
                    case "MATH":
                        smopenIndex = int.Parse(request.downloadHandler.text);
                        break;
                }
            }
            else
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void ResultOFF()
    {
        resultUI.SetActive(false);
        DataBase.instance.uis[0].transform.GetChild(2).gameObject.SetActive(false);
        if (resultUI.GetComponentInChildren<Text>().text == "Login Successful")
        {
            DataBase.instance.uis[0].transform.GetChild(3).gameObject.SetActive(true);
        }
    }
    #region 회원가입
    public void ID_Duplication_Check(string userInputUsername)
    {
        StartCoroutine(IDCheck(userInputUsername));
    }
    private IEnumerator IDCheck(string userInputUsername)
    {
        string url = IDCheckUrl + $"?username={userInputUsername}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (userInputUsername.Length >= 8)
                {
                    if (request.downloadHandler.text == "true")
                    {
                        DataBase.instance.LoginManager.id_Not_Duplicated = true;
                        resultUI.SetActive(true);
                        resultUI.GetComponent<Image>().sprite = resultUI_Imgs[0];
                        resultUI.GetComponentInChildren<Text>().text = "ID Available";
                        DataBase.instance.AudioManager.AudioPlay(0);
                    }
                    else
                    {
                        DataBase.instance.LoginManager.id_Not_Duplicated = false;
                        resultUI.SetActive(true);
                        resultUI.GetComponent<Image>().sprite = resultUI_Imgs[1];
                        resultUI.GetComponentInChildren<Text>().text = "ID duplicated";
                        DataBase.instance.AudioManager.AudioPlay(1);
                    }
                }
                else
                {
                    resultUI.SetActive(true);
                    resultUI.GetComponent<Image>().sprite = resultUI_Imgs[1];
                    resultUI.GetComponentInChildren<Text>().text = "Enter at least 8 characters";
                    DataBase.instance.AudioManager.AudioPlay(1);
                }
            }
            else
            {
                DataBase.instance.AudioManager.AudioPlay(0);
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void Join(string userInputContact, string userInputDateBirth, string userInputName, string userInputPassword, string userInputPasswordRe, string userInputUsername)
    {
        NewUserJoin data = new NewUserJoin
        {
            contact = userInputContact,
            dateBirth = userInputDateBirth,
            name = userInputName,
            password = userInputPassword,
            passwordRe = userInputPasswordRe,
            username = userInputUsername
        };
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(NewUser(jsonData));
    }
    private IEnumerator NewUser(string jsonData)
    {
        // UnityWebRequest를 사용하여 서버에 POST 요청을 보냄
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(joinServerUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("만들기 성공");
                DataBase.instance.AudioManager.AudioPlay(1);
            }
            else
            {
                DataBase.instance.AudioManager.AudioPlay(0);
                Debug.Log("실패");
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    #endregion
    #region 아이디 찾기
    public void Find(string userInputContact, string userInputName)
    {
        Find_UserID data = new Find_UserID
        {
            contact = userInputContact,
            name = userInputName
        };

        string jsonData = JsonUtility.ToJson(data);

        StartCoroutine(Find_ID(jsonData));
    }
    private IEnumerator Find_ID(string jsonData)
    {
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(findServerUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                resultUI.SetActive(true);
                resultUI.GetComponentInChildren<Text>().text = request.downloadHandler.text;
            }
            else
            {
                DataBase.instance.AudioManager.AudioPlay(0);
                Debug.Log("실패");
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void SearchMember(string userInputName, string userInputContact, string pointType, int quantity)
    {
        StartCoroutine(Member_information(userInputName, userInputContact, pointType, quantity,0));
    }
    private IEnumerator Member_information(string userInputName, string userInputContact, string pointType, int quantity,int password)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(allmemberInfoURl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                JSONNode json = JSONClass.Parse(request.downloadHandler.text);

                Debug.LogWarning(request.downloadHandler.text);

                JSONArray listArray = json["list"].AsArray;

                // "data" 객체에서 "point" 키에 해당하는 값을 가져옴          
                for (int i = 0; i < listArray.Count; i++)
                {
                    int id = listArray[i]["id"].AsInt;
                    string contact = listArray[i]["contact"];
                    string name = listArray[i]["name"];


                    if (contact == userInputContact && name == userInputName)
                    {
                        DataBase.instance.MailManager.Send_items(pointType, quantity, id);
                    }
                }
            }
            else
            {
                DataBase.instance.AudioManager.AudioPlay(0);
                Debug.Log("실패");
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    #endregion
    #region 비밀번호 변경
    public void Change(string userInputChangePassword, string userInputChangePasswordRe, string userInputCurrentPassword)
    {
        PutPassword data = new PutPassword
        {
            changePassword = userInputChangePassword,
            changePasswordRe = userInputChangePasswordRe,
            currentPassword = userInputCurrentPassword
        };
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(ChangePassword(jsonData));
    }
    private IEnumerator ChangePassword(string jsonData)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(ChangePasswordServerUrl, jsonData))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                loginSuccess = false;
                DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(true);
                DataBase.instance.uis[0].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                DataBase.instance.AudioManager.AudioPlay(0);
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    #endregion
    #region 공지사항
    public void NoticeRequest(string title, string content)
    {
        Notice data = new Notice
        {
            content = content,
            title = title
        };
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(CreateNotice(jsonData));
    }
    private IEnumerator CreateNotice(string jsonData)
    {
        // UnityWebRequest를 사용하여 서버에 POST 요청을 보냄
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(noticeUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log(request.downloadHandler.text);
            else
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void Notice()
    {
        StartCoroutine(Get_a_Notice());
    }
    public IEnumerator Get_a_Notice()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(noticeAllUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            JSONNode json = JSONClass.Parse(request.downloadHandler.text);

            JSONArray listArray = json["list"].AsArray;

            // "data" 객체에서 "point" 키에 해당하는 값을 가져옴          
            for (int i = 0; i < listArray.Count; i++)
            {
                int id = listArray[i]["id"].AsInt;
                string content = listArray[i]["content"];
                string dateSend = listArray[i]["dateSend"];
                string goodsName = listArray[i]["goodsName"];
                string quantity = listArray[i]["quantity"];
                string sender = listArray[i]["sender"];
                string title = listArray[i]["title"];
                string isReadstring = listArray[i]["isRead"];

                bool isRead;
                if (isReadstring == "O")
                    isRead = true;
                else
                    isRead = false;
                DataBase.instance.MailManager.MailSet(id, content, dateSend, goodsName, isRead, quantity, sender, title);
            }
        }
    }
    #endregion
    public void StartZoom()
    {
        StartCoroutine(ZoomCreate());
    }
    public IEnumerator ZoomCreate()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(zoomUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            Application.OpenURL(request.downloadHandler.text);
        }
    }
    public void ZoomJoin()
    {
        StartCoroutine(Zoom());
    }
    private IEnumerator Zoom()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(zoomJoinUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

           /* Debug.Log(request.downloadHandler.text);*/
            Application.OpenURL(request.downloadHandler.text);
        }
    }
    public void CardRequest(string cardName, string cardType, int diamond, string memo, int point, int sid, int star, int healthPoint, int power)
    {
        Card data = new Card
        {
            cardName = cardName,
            cardType = cardType,
            diamond = diamond,
            memo = memo,
            point = point,
            sid = sid,
            star = star,
            healthPoint = healthPoint,
            power = power
        };
        Debug.Log($"{cardName}, {cardType}, {diamond}, {point}, {memo}, {sid}, {star}");
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(CardCreate(jsonData));
    }
    private IEnumerator CardCreate(string jsonData)
    {
        // UnityWebRequest를 사용하여 서버에 POST 요청을 보냄
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(cardCreateUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log(request.downloadHandler.text);
            else
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void CardAll()
    {
        StartCoroutine(CardAllCoroutine());
    }
    public IEnumerator CardAllCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(cardAllUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            JSONNode json = JSONClass.Parse(request.downloadHandler.text);

            Debug.LogWarning(request.downloadHandler.text);

            JSONArray listArray = json["list"].AsArray;

            // "data" 객체에서 "point" 키에 해당하는 값을 가져옴          
            for (int i = 0; i < listArray.Count; i++)
            {
                int id = listArray[i]["id"].AsInt;
                string cardName = listArray[i]["cardName"];
                string cardType = listArray[i]["cardType"];
                int diamond = listArray[i]["diamond"].AsInt;
                string isUse = listArray[i]["isUse"];
                string memo = listArray[i]["description"];
                int point = listArray[i]["point"].AsInt;
                int sid = listArray[i]["sid"].AsInt;
                int star = listArray[i]["star"].AsInt;
                int healthPoint = listArray[i]["healthPoint"].AsInt;
                int power = listArray[i]["power"].AsInt;
                string property = listArray[i]["property"];
                string skill = listArray[i]["skillEffect"];
                string skill2 = listArray[i]["skillEffect2"];
                string skill3 = listArray[i]["skillEffect3"];
                DataBase.instance.ShopItemManager.ItemSet(id, cardName, cardType, diamond, memo, point, sid, star, isUse, healthPoint, power, property, skill, skill2, skill3);
            }
        }
    }
    public void CardBuyRequest(int id, string type)
    {
        StartCoroutine(CardBuy(id, type));
    }
    private IEnumerator CardBuy(int id, string type)
    {
        string urlWithParams = $"{cardBuyUrl}/{id}?goodsName={type}";
        // UnityWebRequest 생성
        UnityWebRequest request = UnityWebRequest.PostWwwForm(urlWithParams, "");

        request.SetRequestHeader("Authorization", "Bearer " + token);
        // 요청 보내기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            DataBase.instance.ShopItemManager.MyCardList();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
    }
    public void UpgradeUserSet(string contact,string name)
    {
        UpgradeUser data = new UpgradeUser
        {
            contact = contact,
            name = name
        };
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(Upgrade(jsonData));
    }
    private IEnumerator Upgrade(string jsonData)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(upgradeUrl, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    public void CardGame_MyStation()
    {
        StartCoroutine(CardGame_MyStation_Coroutine());
    }

    private IEnumerator CardGame_MyStation_Coroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(myCardGame))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text);

            JSONNode json = JSONClass.Parse(request.downloadHandler.text);

            string round = json["data"]["round"];
            string stage = json["data"]["stage"];

            PlayerPrefs.SetInt("round_Index", int.Parse(round)+1);
            PlayerPrefs.SetInt("stage_Index", int.Parse(stage));

            DataBase.instance.CardGameRoundManager.myRound_Index = PlayerPrefs.GetInt("round_Index");
            DataBase.instance.CardGameRoundManager.myStation_Index = PlayerPrefs.GetInt("stage_Index");
        }
    }
    public void CardGameRoundSet()
    {
        StartCoroutine(RoundIndexSet());
    }
    private IEnumerator RoundIndexSet()
    {
        using (UnityWebRequest request = UnityWebRequest.Put(cardGaemRound, ""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("PUT request successful : UPRoundIndex");
                yield return StartCoroutine(CardGame_MyStation_Coroutine());
                yield return new WaitForSeconds(2f);
                DataBase.instance.CardGameRoundManager.StageStart();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    public void CardGameRoundUP()
    {
        StartCoroutine(RoundIndexUP());
    }
    private IEnumerator RoundIndexUP()
    {
        using (UnityWebRequest request = UnityWebRequest.Put(cardGaemRound,""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("PUT request successful : UPRoundIndex");
                yield return StartCoroutine(CardGame_MyStation_Coroutine());

                DataBase.instance.CardGameRoundManager.RoundStart();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    public void CardGameStageUP()
    {
        StartCoroutine(StageIndexUP());
    }
    private IEnumerator StageIndexUP()
    {
        using (UnityWebRequest request = UnityWebRequest.Put(cardGaemStage, ""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("PUT request successful : StageIndexUP");
                yield return StartCoroutine(CardGame_MyStation_Coroutine());
                yield return new WaitForSeconds(2f);
                DataBase.instance.CardGameRoundManager.StageStart();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    public void Token(string originalText)
    {
        // 시작과 끝 단어 설정
        string startWord = "token";
        string endWord = "name";

        // 시작 단어 다음의 부분 찾기
        int startIndex = originalText.IndexOf(startWord);
        if (startIndex != -1)
        {
            // 시작 단어 이후의 문자열
            string afterStart = originalText.Substring(startIndex + startWord.Length);

            // 끝 단어 이전까지의 문자열 찾기
            int endIndex = afterStart.IndexOf(endWord);
            if (endIndex != -1)
            {
                // 최종 결과 출력
                string result = afterStart.Substring(0, endIndex);
                string pattern = "[,\":]";
                token = Regex.Replace(result, pattern, "");
            }
        }
    }
}
