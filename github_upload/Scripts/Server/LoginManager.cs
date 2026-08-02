using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum Member_group
{
    ROLE_USER,
    ROLE_PREMIUM,
    ROLE_TEST
}

public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField id;
    [SerializeField] private InputField pw;

    [SerializeField] private InputField contact;
    [SerializeField] private InputField dateBirth;
    [SerializeField] private InputField myName;
    [SerializeField] private InputField password_join;
    [SerializeField] private InputField passwordRe_join;
    [SerializeField] private InputField username;

    [SerializeField] private GameObject joinUI;

    [SerializeField] private GameObject find_IDUI;
    [SerializeField] private InputField name_findid;
    [SerializeField] private InputField contact_findid;

    [SerializeField] private GameObject changePasswordUI;

    [SerializeField] private InputField changePassword;
    [SerializeField] private InputField changePasswordRe;
    [SerializeField] private InputField currentPassword;

    [HideInInspector] public bool id_Not_Duplicated;
    [HideInInspector] public int playerID;
    [HideInInspector] public string username_;
    [HideInInspector] public string cardID;
    public GameObject clickH;

    [HideInInspector] public bool reStartNotice;
    [SerializeField] private GameObject screen;

    [SerializeField] private float shakeDistance = 10f; // 흔들림 거리
    [SerializeField] private float shakeDuration = 0.5f; // 흔들림 지속 시간

    private Vector3 originalPosition;
    public Member_group memberType;
    public DateTime date_premium;
    public Toggle automaticLogin;
    // public Text test;
    private void Start()
    {
        StartCoroutine(ScreenUp());
    }
    private IEnumerator ScreenUp()
    {
        while (screen.GetComponent<RectTransform>().localPosition.y < 1228f)
        {
            float newY = Mathf.MoveTowards(screen.GetComponent<RectTransform>().localPosition.y, 1228f, Time.deltaTime * 2000);
            screen.GetComponent<RectTransform>().localPosition = new Vector3(screen.GetComponent<RectTransform>().localPosition.x, newY, screen.GetComponent<RectTransform>().localPosition.z);
            yield return null;
        }
        if (PlayerPrefs.HasKey("ID"))
        {
            id.text = PlayerPrefs.GetString("ID");
            pw.text = PlayerPrefs.GetString("PW");
            Login();
        }
    }
    public void Click()
    {
        clickH.SetActive(false);
        DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(true);
    }
    public void ExitButton()
    {
        if (joinUI.activeSelf)
        {
            joinUI.SetActive(false);
            contact.text = "";
            dateBirth.text = "";
            myName.text = "";
            password_join.text = "";
            passwordRe_join.text = "";
            username.text = "";
        }
        if (find_IDUI.activeSelf)
        {
            find_IDUI.SetActive(false);
            name_findid.text = "";
            contact_findid.text = "";
        }
        if (changePasswordUI.activeSelf)
        {
            changePasswordUI.SetActive(false);
            changePassword.text = "";
            changePasswordRe.text = "";
            currentPassword.text = "";
        }
        if (DataBase.instance.uis[0].transform.GetChild(1).gameObject.activeSelf)
        {
            DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(false);
            clickH.SetActive(true);

        }
    }
    public void Login()
    {
        if (automaticLogin)
        {
            PlayerPrefs.SetString("ID", id.text);
            PlayerPrefs.SetString("PW", pw.text);
        }
        LoginCheck(id.text, pw.text);
        id.text = "";
        pw.text = "";

    }
    public void Logout()
    {
        id.text = "";
        pw.text = "";
        DataBase.instance.uis[0].transform.GetChild(3).gameObject.SetActive(false);
        DataBase.instance.uis[0].transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Join_Ui_Toggle()
    {
        if (joinUI.activeSelf)
        {
            joinUI.SetActive(false);
            contact.text = "";
            dateBirth.text = "";
            myName.text = "";
            password_join.text = "";
            passwordRe_join.text = "";
            username.text = "";
        }
        else
        {
            joinUI.SetActive(true);
        }
    }
    public void Find_Ui_Toggle()
    {
        if (find_IDUI.activeSelf)
        {
            find_IDUI.SetActive(false);
            name_findid.text = "";
            contact_findid.text = "";
        }
        else
        {
            find_IDUI.SetActive(true);
        }
    }
    public void Change_UI_Toggle()
    {
        if (changePasswordUI.activeSelf)
        {
            changePasswordUI.SetActive(false);
            changePassword.text = "";
            changePasswordRe.text = "";
            currentPassword.text = "";
        }
        else
        {
            changePasswordUI.SetActive(true);
        }
    }
    public void ID_Duplication_CheckButtton()
    {
        DataBase.instance.WebRequestManager.ID_Duplication_Check(username.text);
    }
    public void JoinButton()
    {
        // contact.text = FormatPhoneNumber(contact.text);
        Join(contact.text, dateBirth.text, myName.text, password_join.text, passwordRe_join.text, username.text);
        contact.text = ""; dateBirth.text = ""; myName.text = ""; password_join.text = ""; passwordRe_join.text = ""; username.text = "";
    }
    public void NoticeSetting()
    {
        DataBase.instance.MailManager.mails = new System.Collections.Generic.List<Mail>();
        DataBase.instance.WebRequestManager.Notice();
    }
    public void FindButton()
    {
        DataBase.instance.WebRequestManager.Find(contact_findid.text, name_findid.text);
    }
    public void ChangeButton()
    {
        changePasswordUI.SetActive(false);
        ChangePassword(changePassword.text, changePasswordRe.text, currentPassword.text);
        changePassword.text = ""; changePasswordRe.text = ""; currentPassword.text = "";
    }
    public void ChangePassword(string userInputChangePassword, string userInputChangePasswordRe, string userInputCurrentPassword)
    {
        DataBase.instance.WebRequestManager.Change(userInputChangePassword, userInputChangePasswordRe, userInputCurrentPassword);
    }
    public void Join(string userInputContact, string userInputDateBirth, string userInputName, string userInputPassword, string userInputPasswordRe, string userInputUsername)
    {
        if (id_Not_Duplicated)
        {
            if (userInputPassword.Length >= 8)
            {
                if(userInputPassword == userInputPasswordRe)
                {
                    if (userInputDateBirth.Length == 10)
                    {
                        if (userInputContact.Length == 13)
                        {
                            DataBase.instance.WebRequestManager.Join(userInputContact, userInputDateBirth, userInputName, userInputPassword, userInputPasswordRe, userInputUsername);
                            joinUI.SetActive(false);
                        }
                        else
                        {
                            contact.placeholder.GetComponent<Text>().text = "<color=red>Please match 000-0000-0000 format</color>";
                        }
                    }
                    else
                    {
                        dateBirth.placeholder.GetComponent<Text>().text = "<color=red>Please match 0000-00-00 format</color>";
                    }
                }
                else
                {
                    passwordRe_join.placeholder.GetComponent<Text>().text = "<color=red> The password is not the same </color>";
                }
                // contact.text = ""; dateBirth.text = ""; myName.text = ""; password_join.text = ""; passwordRe_join.text = ""; username.text = "";
            }
            else
            {
                password_join.placeholder.GetComponent<Text>().text = "<color=red> Please enter at least 8 characters </color>";
            }
        }
        else if(!id_Not_Duplicated)
        {
            username.placeholder.GetComponent<Text>().text = "<color=red>ID cannot be generated</color>";
        }
        else
        {
            DataBase.instance.WebRequestManager.resultUI.SetActive(true);
            DataBase.instance.WebRequestManager.resultUI.GetComponent<Image>().sprite = DataBase.instance.WebRequestManager.resultUI_Imgs[1];
            DataBase.instance.WebRequestManager.resultUI.GetComponentInChildren<Text>().text = "Please re-enter the information";
        }
    }
    public void LoginCheck(string inputUserId, string password)
    {
        DataBase.instance.WebRequestManager.PasswordRequest(inputUserId, password);
    }

    public void IDFail()
    {
        DataBase.instance.WebRequestManager.resultUI.SetActive(true);
        DataBase.instance.WebRequestManager.resultUI.GetComponent<Image>().sprite = DataBase.instance.WebRequestManager.resultUI_Imgs[1];
        DataBase.instance.WebRequestManager.resultUI.GetComponentInChildren<Text>().text = "ID does not match";
        clickH.SetActive(true);
    }
    public void UserInfo(string inputUserId, string inputContact, string pointType, int quantity)
    {
        DataBase.instance.WebRequestManager.SearchMember(inputUserId, inputContact, pointType, quantity);
    }


    #region 회원가입 연락처 양식 자동 생성
    public void ContactStringFormat(Text data)
    {
        int num = 0;
        if (int.TryParse(data.text, out num))
        {
            contact.text = FormatPhoneNumber(data.text);
        }
        else
        {
            if (data.text != "")
            {
                contact.text = "";
                Debug.Log("숫자만 입력하세요");
            }
        }
    }
    // 특정 형식에 맞게 "-" 추가
    private string FormatPhoneNumber(string input)
    {
        return string.Format("{0:0##-####-####}", int.Parse(input));
    }
    #endregion
    #region 아이디 찾기
    public void ContactFindStringFormat(Text data)
    {
        int num = 0;
        if (int.TryParse(data.text, out num))
        {
            contact_findid.text = FormatPhoneNumberFind(data.text);
        }
        else
        {
            if (data.text != "")
            {
                contact.text = "";
                Debug.Log("숫자만 입력하세요");
            }
            else
                return;
        }
    }
    // 특정 형식에 맞게 "-" 추가
    private string FormatPhoneNumberFind(string input)
    {
        return string.Format("{0:0##-####-####}", int.Parse(input));
    }
    #endregion
    #region 생년월일
    public void DateBirthStringFormat(Text data)
    {
        int num = 0;
        if (int.TryParse(data.text, out num))
        {
            dateBirth.text = FormatDateBirth(data.text);
        }
        else
        {
            if (data.text != "")
            {
                dateBirth.text = "";
                Debug.Log("숫자만 입력하세요");
            }
            else
                return;
        }
    }
    // 특정 형식에 맞게 "-" 추가
    private string FormatDateBirth(string input)
    {
        return string.Format("{0:####-##-##}", int.Parse(input));
    }
    #endregion
    private IEnumerator ShakeUIElement(RectTransform uiElement)
    {
        float elapsedTime = 0f;
        originalPosition = uiElement.anchoredPosition;
        while (elapsedTime < shakeDuration)
        {
            // 좌우로 흔들림 효과를 주기 위해 삼각함수(sin)를 사용합니다.
            float xOffset = Mathf.Sin(Time.time * Mathf.PI * 2f * 5f) * shakeDistance;
            Vector3 newPosition = originalPosition + new Vector3(xOffset, 0f, 0f);
            uiElement.anchoredPosition = newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 돌아갑니다.
        uiElement.anchoredPosition = originalPosition;
    }
}
