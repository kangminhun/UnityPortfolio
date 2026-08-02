using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    [DllImport("I18N")]
    private static extern float Foopluginmethod();

    public static DataBase instance;
    //public PluginManager pluginManager;
    private LoginManager _loginManager;
    private WebRequestManager _webRequestManager;
    private PointManager _pointManager;
    private MailManager _mailManager;
    private ShopItemManager _shopItemManager;
    private Commander _commander;
    private RankManager _rankManager;
    private AttendanceManager _attendanceManager;
    private AudioSource _audioSource;
    private AudioClip _myAudioClip;
    private AudioManager _audioManager;
    private CopyTxt _copyTxt;
    private CardGameRoundManager _cardGameRoundManager;
    public AudioManager AudioManager
    {
        get
        {
            if (_audioManager == null)
            {
                _audioManager = GetComponent<AudioManager>();
            }
            return _audioManager;
        }
    }
    // AudioClip을 설정하고 가져오는 프로퍼티
    public AudioClip MyAudioClip
    {
        get { return _myAudioClip; }
        set
        {
            _myAudioClip = value;
            if (_audioSource != null)
            {
                _audioSource.clip = _myAudioClip;
                _audioSource.Play();
            }
        }
    }

    public LoginManager LoginManager
    {
        get
        {
            if (_loginManager == null)
            {
                _loginManager = GetComponent<LoginManager>();
            }
            return _loginManager;
        }
    }

    public WebRequestManager WebRequestManager
    {
        get
        {
            if (_webRequestManager == null)
            {
                _webRequestManager = GetComponent<WebRequestManager>();
            }
            return _webRequestManager;
        }
    }

    public PointManager PointManager
    {
        get
        {
            if (_pointManager == null)
            {
                _pointManager = GetComponent<PointManager>();
            }
            return _pointManager;
        }
    }

    public MailManager MailManager
    {
        get
        {
            if (_mailManager == null)
            {
                _mailManager = GetComponent<MailManager>();
            }
            return _mailManager;
        }
    }

    public ShopItemManager ShopItemManager
    {
        get
        {
            if (_shopItemManager == null)
            {
                _shopItemManager = GetComponent<ShopItemManager>();
            }
            return _shopItemManager;
        }
    }

    public Commander Commander
    {
        get
        {
            if (_commander == null)
            {
                _commander = GetComponent<Commander>();
            }
            return _commander;
        }
    }

    public RankManager RankManager
    {
        get
        {
            if (_rankManager == null)
            {
                _rankManager = GetComponent<RankManager>();
            }
            return _rankManager;
        }
    }
    public AttendanceManager AttendanceManager
    {
        get
        {
            if (_attendanceManager == null)
            {
                _attendanceManager = GetComponent<AttendanceManager>();
            }
            return _attendanceManager;
        }
    }
    public CardGameRoundManager CardGameRoundManager
    {
        get
        {
            if(_cardGameRoundManager == null)
            {
                _cardGameRoundManager=GetComponent<CardGameRoundManager>();
            }
            return _cardGameRoundManager;
        }
    }
    public GameObject[] uis;
    public CopyTxt CopyTxt
    {
        get
        {
            if (_copyTxt == null)
            {
                _copyTxt = GetComponent<CopyTxt>();
            }
            return _copyTxt;
        }
    }
    //public Text nullChack;
    public GameObject exitUi;
    [SerializeField] private Uicontroller uicontroller;
    [SerializeField] private GameObject fade;

    public void Awake()
    {
        //nullChack.text = $"{_audioManager}, {_loginManager}, {_webRequestManager}, {_pointManager}";
        if (instance == null)
        {
            instance = this;
        }
        _audioSource = GetComponent<AudioSource>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void Go()
    {
        StartCoroutine(StartDelay());
    }
    private IEnumerator StartDelay()
    {
        fade.SetActive(true);
        WebRequestManager.LoginSuccess();
        yield return new WaitForSeconds(.5f);
        LoginManager.NoticeSetting();
        yield return new WaitForSeconds(.5f);
        WebRequestManager.CardAll();
        yield return new WaitForSeconds(.5f);
        ShopItemManager.MyCardList();
        yield return new WaitForSeconds(.5f);
        Commander.CommandSet();
        yield return new WaitForSeconds(.5f);
        uicontroller.Set();
        yield return new WaitForSeconds(.5f);
        WebRequestManager.CardGame_MyStation();
        uis[0].transform.GetChild(0).gameObject.SetActive(true);
        uis[0].transform.GetChild(1).gameObject.SetActive(false);
        uis[0].SetActive(false);
        uis[1].SetActive(true);
        if (DataBase.instance.WebRequestManager.type != UserType.admin)
        {
            AttendanceManager.Attendance();
            MailManager.Exclamation_markInvek();
            // ShopItemManager.Exclamation_markInvek();
        }
        fade.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exitUi.activeSelf)
                exitUi.SetActive(true);
            else
                exitUi.SetActive(false);
        }
    }
    public void Exit()
    {
        Application.Quit();
        if (System.IO.File.Exists(Application.persistentDataPath + $"Appleberry English/videos"))
        {
            System.IO.File.Delete(Application.persistentDataPath + $"Appleberry English/videos");
        }
        Debug.Log("All downloaded videos deleted.");
    }
    public void No()
    {
        exitUi.SetActive(false);
    }
    public void ButtonClick_SoundPlay()
    {
        AudioManager.AudioPlay(4);
    }
}
