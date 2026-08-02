using System;
using UnityEngine;
using UnityEngine.UI;

public class Commander : MonoBehaviour
{
    public InputField chatUi;
    private bool enteringCommands;
    private string commands;
    //공지사항
    private string content;
    private string title;

    //카드 등록
    private string cardname;
    private string cardType;
    private string diamond;
    private string point;
    private string memo;
    private string sid;
    private string star;
    private string healthPoint;
    private string power;

    private int myPointUp;

    private string pointType;
    private int quantity;
    private string recipientName;
    private string reciplentContact;
    public void Start()
    {
        chatUi.onEndEdit.AddListener(OnNoticeInputEndEdit);
    }
    public void CommandSet()
    {
        if (DataBase.instance.WebRequestManager.type == UserType.admin)
        {
            chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
            chatUi.gameObject.SetActive(true);
        }
        else
        {
            chatUi.gameObject.SetActive(false);
        }
    }
    public void OnNoticeInputEndEdit(string inputText)
    {
        if (!enteringCommands)
        {
            // "공지사항"으로 시작하는 입력을 확인
            if (inputText.Trim().Equals("공지사항"))
            {
                enteringCommands = true;
                chatUi.ActivateInputField();
                chatUi.text = "";
                chatUi.placeholder.GetComponent<Text>().text = "제목 (2~30자)";
                commands = "공지사항";
            }
            else if (inputText.Trim().Equals("카드등록"))
            {
                enteringCommands = true;
                chatUi.ActivateInputField();
                chatUi.text = "";
                chatUi.placeholder.GetComponent<Text>().text = "카드이름 (2~20자)";
                commands = "카드등록";
            }
            else if (inputText.Trim().Equals("내 포인트 올리기"))
            {
                enteringCommands = true;
                chatUi.ActivateInputField();
                chatUi.text = "";
                chatUi.placeholder.GetComponent<Text>().text = "올릴 포인트";
                commands = "내 포인트 올리기";
            }
            else if (inputText.Trim().Equals("포인트 보내기"))
            {
                enteringCommands = true;
                chatUi.ActivateInputField();
                chatUi.text = "";
                chatUi.placeholder.GetComponent<Text>().text = "보낼 종류 (대문자로 기입)";
                commands = "포인트 보내기";
            }
            else if (inputText.Trim().Equals("유료회원 등록"))
            {
                enteringCommands = true;
                chatUi.ActivateInputField();
                chatUi.text = "";
                chatUi.placeholder.GetComponent<Text>().text = "이름";
                commands = "유료회원 등록";
            }
        }
        else
        {
            switch (commands)
            {
                case "공지사항":
                    if (chatUi.placeholder.GetComponent<Text>().text == "제목 (2~30자)")
                    {
                        title = inputText.Trim();
                        Debug.Log(title);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "내용 (10자이상 입력)";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "내용 (10자이상 입력)")
                    {
                        content = inputText.Trim();
                        Debug.Log(content);
                        chatUi.text = "";
                        DataBase.instance.WebRequestManager.NoticeRequest(title, content);
                        chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
                        enteringCommands = false;
                    }
                    break;
                case "카드등록":
                    if (chatUi.placeholder.GetComponent<Text>().text == "카드이름 (2~20자)")
                    {
                        cardname = inputText.Trim();
                        Debug.Log(cardname);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "카드타입 (대문자로 기입)";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "카드타입 (대문자로 기입)")
                    {
                        cardType = inputText.Trim();
                        Debug.Log(cardType);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "다이아몬드 수량";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "다이아몬드 수량")
                    {
                        diamond = inputText.Trim();
                        Debug.Log(diamond);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "포인트 수량";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "포인트 수량")
                    {
                        point = inputText.Trim();
                        Debug.Log(point);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "메모";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "메모")
                    {
                        memo = inputText.Trim();
                        Debug.Log(memo);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "순서";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "순서")
                    {
                        sid = inputText.Trim();
                        Debug.Log(sid);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "체력";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "체력")
                    {
                        healthPoint = inputText.Trim();
                        Debug.Log(sid);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "공격력";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "공격력")
                    {
                        power = inputText.Trim();
                        Debug.Log(sid);
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "별 갯수";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "별 갯수")
                    {
                        star = inputText.Trim();
                        chatUi.text = "";
                        DataBase.instance.WebRequestManager.CardRequest
                            (
                            cardname,
                            cardType,
                            int.Parse(diamond),
                            memo,
                            int.Parse(point),
                            int.Parse(sid),
                            int.Parse(star),
                            int.Parse(healthPoint),
                            int.Parse(power)
                            );
                        chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
                        enteringCommands = false;
                    }
                    break;
                case "내 포인트 올리기":
                    if (chatUi.placeholder.GetComponent<Text>().text == "올릴 포인트")
                    {
                        myPointUp = int.Parse(inputText.Trim());
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
                        DataBase.instance.PointManager.PointUp(myPointUp);
                        enteringCommands = false;
                    }
                    break;
                case "포인트 보내기":
                    if (chatUi.placeholder.GetComponent<Text>().text == "보낼 종류 (대문자로 기입)")
                    {
                        pointType = inputText.Trim();
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "수량";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "수량")
                    {
                        quantity = int.Parse(inputText.Trim());
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "받을 사람이름";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "받을 사람이름")
                    {
                        recipientName = inputText.Trim();
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "전화번호(-포함 입력)";
                    }
                    else if(chatUi.placeholder.GetComponent<Text>().text == "전화번호(-포함 입력)")
                    {
                        reciplentContact = inputText.Trim();
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        DataBase.instance.LoginManager.UserInfo(recipientName, reciplentContact, pointType, quantity);
                        chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
                        enteringCommands = false;
                    }
                    break;
                case "유료회원 등록":
                    if (chatUi.placeholder.GetComponent<Text>().text == "이름")
                    {
                        recipientName = inputText.Trim();
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        chatUi.placeholder.GetComponent<Text>().text = "전화번호(-포함 입력)";
                    }
                    else if (chatUi.placeholder.GetComponent<Text>().text == "전화번호(-포함 입력)")
                    {
                        reciplentContact = inputText.Trim();
                        chatUi.ActivateInputField();
                        chatUi.text = "";
                        DataBase.instance.WebRequestManager.UpgradeUserSet(reciplentContact, recipientName);
                        chatUi.placeholder.GetComponent<Text>().text = "관리자 명령창 명령어를 입력 후 순서대로 입력해 주세요";
                        enteringCommands = false;
                    }
                    break;
            }
        }
    }
}
