using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public List<Mail> mails;
    public Uichage uichage;
    public GameObject testMail;
    public Toggle allclickToggle;
    public Sprite[] diamoundIcon;
    public Sprite[] pointIcon;
    public GameObject exclamation_mark;

    [SerializeField ]private Sprite orginImg;
    [SerializeField] private Sprite changImg;
    private int itemsPerPage = 5;
    private int currentPage = 0;
    private List<Mail> myCheckList;
    private const string mailStatusUrl = "https://your-server-domain.com/v1/mail";
    private int endIndex;
    private void Start()
    {
        endIndex = mails.Count;
    }
    public void Exclamation_markInvek()
    {
        InvokeRepeating("Exclamation_markSet", 0f, 5f);
    }
    private void Exclamation_markSet()
    {
        int count = 0;
        if (mails.Count > 0)
        {
            for (int i = 0; i < mails.Count; i++)
            {
                if (!mails[i].isRead)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                exclamation_mark.SetActive(true);
            }
            else
            {
                exclamation_mark.SetActive(false);
            }
        }
    }
    public void MailSet(int id, string content, string dateSend, string goodsName, bool isRead, string quantity, string sender, string title)
    {
        Mail mail = ScriptableObject.CreateInstance<Mail>();
        mail.id = id;
        mail.title = title;
        mail.content = content;
        mail.dateSend = dateSend;
        mail.goodsName = goodsName;
        mail.isRead = isRead;
        mail.quantity = quantity;
        mail.recipient = DataBase.instance.LoginManager.username_;
        mail.sender = sender;
        mails.Add(mail);
    }
    public void MailList(int num, int childNum)
    {
        if (mails.Count != 0)
        {
            Color newColor = uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails").gameObject.transform.GetChild(childNum).GetComponent<Image>().color;
            newColor.a = .6f;
            uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails").gameObject.transform.GetChild(childNum).GetComponent<Image>().color = newColor;
            string content = mails[num].content;
            string sender = mails[num].sender;
            string title = mails[num].title;
            testMail.transform.Find("content").GetComponent<Text>().text = content;
            testMail.transform.Find("sender").GetComponent<Text>().text = sender;
            testMail.transform.Find("title").GetComponent<Text>().text = title;
            if (mails[num].quantity != "DefaultQuantity")
            {
                if (mails[num].goodsName == "DIAMOND")
                {
                    switch(mails[num].quantity)
                    {
                        case "1000":
                            testMail.transform.Find("diamond").GetComponent<Image>().sprite = diamoundIcon[0];
                            break;
                        case "2000":
                            testMail.transform.Find("diamond").GetComponent<Image>().sprite = diamoundIcon[1];
                            break;
                        case "3000":
                            testMail.transform.Find("diamond").GetComponent<Image>().sprite = diamoundIcon[2];
                            break;
                    }
                    testMail.transform.Find("diamond").gameObject.SetActive(true);
                    testMail.transform.Find("point").GetComponent<Image>().color = Color.white;
                    testMail.transform.Find("point").gameObject.SetActive(false);
                    if (!mails[num].isRead)
                    {
                        testMail.transform.Find("diamond").GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        testMail.transform.Find("diamond").GetComponent<Image>().color = Color.gray;
                    }
                }
                else
                {
                    switch (mails[num].quantity)
                    {
                        case "100":
                            testMail.transform.Find("point").GetComponent<Image>().sprite = pointIcon[0];
                            break;
                        case "200":
                            testMail.transform.Find("point").GetComponent<Image>().sprite = pointIcon[1];
                            break;
                        case "300":
                            testMail.transform.Find("point").GetComponent<Image>().sprite = pointIcon[2];
                            break;
                    }
                    testMail.transform.Find("point").gameObject.SetActive(true);
                    testMail.transform.Find("diamond").GetComponent<Image>().color = Color.white;
                    testMail.transform.Find("diamond").gameObject.SetActive(false);
                    if (!mails[num].isRead)
                    {
                        testMail.transform.Find("point").GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        testMail.transform.Find("point").GetComponent<Image>().color = Color.gray;
                    }
                }
                testMail.transform.Find("GetButton").gameObject.SetActive(true);
                if (!mails[num].isRead)
                {
                    testMail.transform.Find("GetButton").GetComponent<Button>().enabled = true;
                    testMail.transform.Find("GetButton").GetComponent<Image>().color = Color.white;
                    testMail.transform.Find("GetButton").GetComponent<Button>().onClick.RemoveAllListeners();
                    testMail.transform.Find("GetButton").GetComponent<Button>().onClick.AddListener(() => GetButton(mails[num]));
                }
                else
                {
                    testMail.transform.Find("GetButton").GetComponent<Button>().enabled = false;
                    testMail.transform.Find("GetButton").GetComponent<Image>().color = Color.gray;
                }
            }
            else
            {
                testMail.transform.Find("GetButton").gameObject.SetActive(false);
                if (!mails[num].isRead)
                {
                    StartCoroutine(Status_Check_Mail(mails[num].id));
                    mails[num].isRead = true;
                }
                testMail.transform.Find("diamond").gameObject.SetActive(false);
                testMail.transform.Find("point").gameObject.SetActive(false);
                LoadMailObjects();
            }
        }
        else
        {
            testMail.transform.Find("content").GetComponent<Text>().text = "받은 메일이 없습니다";
            testMail.transform.Find("sender").GetComponent<Text>().text = "";
            testMail.transform.Find("title").GetComponent<Text>().text = "받은 메일이 없습니다";
        }
    }
    public void GetButton(Mail mail)
    {
        testMail.transform.Find("GetButton").GetComponent<Button>().enabled = false;
        testMail.transform.Find("GetButton").GetComponent<Image>().color = Color.gray;
        if (testMail.transform.Find("point").gameObject.activeSelf)
            testMail.transform.Find("point").GetComponent<Image>().color = Color.gray;
        else
            testMail.transform.Find("diamond").GetComponent<Image>().color = Color.gray;
        if (!mail.isRead)
        {
            StartCoroutine(Status_Check_Mail(mail.id));
            LoadMailObjects();
        }
        mail.isRead = true;
    }
    public IEnumerator Status_Check_Mail(int mailId)
    {
        string status_Check_MailUrl = $"{mailStatusUrl}/{mailId}";

        UnityWebRequest request = UnityWebRequest.Put(status_Check_MailUrl, "");

        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공");
        }
        else
        {
            Debug.Log("실패");
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
    public void AllToggleOn()
    {
        Transform uiParent = uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails");
        int activeChildCount = CountActiveChildren(uiParent);
        for (int i = 0; i < activeChildCount; i++)
        {
            int data = i;
            if (allclickToggle.isOn)
            {
                uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails").GetChild(data).GetComponentInChildren<Toggle>().isOn = true;
            }
            else
            {
                uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails").GetChild(data).GetComponentInChildren<Toggle>().isOn = false;
            }
        }
    }
    private void Toggle(GameObject toggle, Mail mail)
    {
        if (toggle.GetComponentInChildren<Toggle>().isOn)
        {
            Debug.Log("++++++++++++++++++++++++++++++++++");
            myCheckList.Add(mail);
            mail.isRead = true;
            if (mail.quantity != "DefaultQuantity")
            {
                GetButton(mail);
            }
            StartCoroutine(Status_Check_Mail(mail.id));
        }
        else
        {
            Debug.Log("-----------------------------------");
            myCheckList.Remove(mail);
        }
    }
    public IEnumerator DeleteMail(int mailId)
    {
        string statusCheckMailUrl = $"{mailStatusUrl}/{mailId}";

        UnityWebRequest request = UnityWebRequest.Delete(statusCheckMailUrl);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("실패");
            Debug.LogError(request.responseCode);
            Debug.LogError(request.downloadHandler.text);
        }
    }
    public void DeleteButton()
    {
        if (myCheckList.Count != 0)
        {
            StartCoroutine(DeleteSelectedMails());
        }
    }

    public IEnumerator DeleteSelectedMails()
    {
        foreach (var mail in myCheckList)
        {
            yield return StartCoroutine(DeleteMail(mail.id));
        }
        myCheckList.Clear();
        allclickToggle.isOn = false;
        yield return new WaitForSeconds(1);
        DataBase.instance.LoginManager.NoticeSetting();
        yield return new WaitForSeconds(1f);
        MailOpen();
    }
    public void MailOpen()
    {
        itemsPerPage = 5;
        currentPage = 0;
        endIndex = mails.Count;
        uichage.UIViewControllerOpen("Mailbox");
        LoadMailObjects();
        MailList(0, 0);
    }
    public void LoadMailObjects()
    {
        Transform mailsTransform = uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").transform.Find("Mails").gameObject.transform;       
        myCheckList = new List<Mail>();

        Debug.Log($"endIndex : {endIndex}");
        if (endIndex / 5 != 0)
        {
            for (int i = 0; i < mailsTransform.transform.childCount; i++)
            {
                mailsTransform.transform.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject mailObject;
                int sum = i;
                mailObject = mailsTransform.GetChild(sum).gameObject;
                mailObject.GetComponent<Button>().onClick.RemoveAllListeners();
                mailObject.GetComponent<Button>().onClick.AddListener(() => MailList(sum + (5 * currentPage), sum));

                mailObject.GetComponentInChildren<Toggle>().onValueChanged.RemoveAllListeners();
                mailObject.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn => Toggle(mailObject, mails[sum + (5 * currentPage)]));
                Debug.Log(mails.Count);
                if (mails[sum + (5 * currentPage)].isRead)
                {
                    Color newColor = mailObject.GetComponent<Image>().color;
                    newColor.a = .6f;
                    mailObject.GetComponent<Image>().color = newColor;
                    mailObject.GetComponent<Image>().sprite = changImg;
                    mailObject.transform.Find("Sender").GetComponent<Text>().text = "<color=white>" + mails[sum + (5 * currentPage)].sender + "</color>";
                    mailObject.transform.Find("Title").GetComponent<Text>().text = "<color=white>" + mails[sum + (5 * currentPage)].title + "</color>";
                }
                else
                {
                    mailObject.GetComponent<Image>().color=Color.white;
                    mailObject.GetComponent<Image>().sprite = orginImg;
                    mailObject.transform.Find("Sender").GetComponent<Text>().text = "<color=#BF8D5D>" + mails[sum + (5 * currentPage)].sender + "</color>";
                    mailObject.transform.Find("Title").GetComponent<Text>().text = "<color=#BF8D5D>" + mails[sum + (5 * currentPage)].title + "</color>";
                }
            }
        }
        else
        {
            for (int i = 0; i < mailsTransform.transform.childCount; i++)
            {
                mailsTransform.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < endIndex % 5; i++)
            {
                GameObject mailObject;
                int sum = i;
                mailObject = mailsTransform.GetChild(sum).gameObject;
                mailObject.gameObject.SetActive(true);
                mailObject.GetComponent<Button>().onClick.RemoveAllListeners();
                mailObject.GetComponent<Button>().onClick.AddListener(() => MailList(sum + (5 * currentPage), sum));
                mailObject.GetComponentInChildren<Toggle>().onValueChanged.RemoveAllListeners();
                mailObject.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn => Toggle(mailObject, mails[sum + (5 * currentPage)]));
                if (mails[sum + (5 * currentPage)].isRead)
                {
                    Color newColor = mailObject.GetComponent<Image>().color;
                    newColor.a = .6f;
                    mailObject.GetComponent<Image>().color = newColor;
                    mailObject.GetComponent<Image>().sprite = changImg;
                    mailObject.transform.Find("Sender").GetComponent<Text>().text = "<color=white>" + mails[sum+(5* currentPage)].sender + "</color>";
                    mailObject.transform.Find("Title").GetComponent<Text>().text = "<color=white>" + mails[sum + (5 * currentPage)].title + "</color>";
                }
                else
                {
                    mailObject.GetComponent<Image>().color = Color.white;
                    mailObject.GetComponent<Image>().sprite = orginImg;
                    mailObject.transform.Find("Sender").GetComponent<Text>().text = "<color=#BF8D5D>" + mails[sum + (5 * currentPage)].sender + "</color>";
                    mailObject.transform.Find("Title").GetComponent<Text>().text = "<color=#BF8D5D>" + mails[sum + (5 * currentPage)].title + "</color>";
                }
            }
        }

        allclickToggle.isOn = false;
        UpdateButtons();
    }
    private void UpdateButtons()
    {
        Transform mailsTransform = uichage.uIViewContainer.transform.Find("Mailbox").transform.Find("Bg").gameObject.transform;

        // 이전 버튼 활성화 여부 설정
        bool hasPrevious = currentPage > 0;
        mailsTransform.Find("PreviousButton").gameObject.SetActive(hasPrevious);

        // 다음 버튼 활성화 여부 설정
        bool hasNext = (currentPage + 1) * itemsPerPage < mails.Count;
        mailsTransform.Find("NextButton").gameObject.SetActive(hasNext);

        mailsTransform.Find("Number").GetComponent<Text>().text = (currentPage+1).ToString();

    }

    public void NextPage()
    {
        if ((currentPage + 1) * itemsPerPage < mails.Count)
        {
            allclickToggle.isOn = false;
            currentPage++;
            endIndex -= 5;
            LoadMailObjects();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            allclickToggle.isOn = false;
            currentPage--;
            endIndex += 5;
            LoadMailObjects();
        }
    }

    public void MailClose()
    {
        uichage.UIViewControllerClose("Mailbox");
    }

    public void Send_items(string pointType, int quantity, int memberId)
    {
        StartCoroutine(Send_items_Mail(pointType, quantity, memberId));
    }
    public IEnumerator Send_items_Mail(string pointType, int quantity, int memberId)
    {
        string status_Check_MailUrl = $"{mailStatusUrl}/recipient/{memberId}?goodsName={pointType}&quantity={quantity}";
        Debug.Log(status_Check_MailUrl);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(status_Check_MailUrl, "");

        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공");
        }
        else
        {
            Debug.Log("실패");
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
    private int CountActiveChildren(Transform parent)
    {
        int count = 0;

        // 부모의 모든 자식을 반복
        foreach (Transform child in parent)
        {
            // 자식이 켜져 있는 경우 count 증가
            if (child.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
