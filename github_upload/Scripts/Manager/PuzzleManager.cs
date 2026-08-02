using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject tilePrefab;                               // 숫자 타일 프리팹
    public Transform tilesParent;                            // 타일이 배치되는 "Board" 오브젝트의 Transform

    [HideInInspector]
    public List<Tile> tileList;                               // 생성한 타일 정보 저장
    [HideInInspector]
    public int roundData;
    private Vector2Int puzzleSize;

    public Vector2Int PuzzleSizeget
    {
        get
        {
            return puzzleSize;
        }
        set { puzzleSize = value; }
    }

    public Vector3 EmptyTilePosition { set; get; }          // 빈 타일의 위치
    public int MoveCount { private set; get; } = 0;    // 이동 횟수

    public Image delayImg;

    [Header("1라운드")]
    public Sprite[] tileSprites_3x3;
    public Sprite originImg;
    public Sprite originEngImg;
    [Header("2라운드")]
    public Sprite[] twoRound_tileSprites_3x3;
    public Sprite twoRound_originImg;
    public Sprite twoRound_originEngImg;
    [Header("3라운드")]
    public Sprite[] threeRound_tileSprites_3x3;
    public Sprite threeRound_originImg;
    public Sprite threeRound_originEngImg;
    [Header("4라운드")]
    public Sprite[] fourRound_tileSprites_3x3;
    public Sprite fourRound_originImg;
    public Sprite fourRound_originEngImg;


    //public Image backGround;

    public List<Tile> tiless = new List<Tile>();

    private List<Tile> t = new List<Tile>();

    public Image origin_Img;
    public Gamemanager manager;

    public float timeLimit = 60f; // 1분(60초)
    [HideInInspector]
    public float currentTime;
    [HideInInspector]
    public bool isTimerRunning;
    public Text playTimeTxt;
    public void SpawnTiles()
    {
        currentTime = timeLimit;
        isTimerRunning = true;
        PuzzleSizeget = new Vector2Int(3, 3);
        tilesParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2Int(335, 183);
        for (int y = 0; y < puzzleSize.y; ++y)
        {
            for (int x = 0; x < puzzleSize.x; ++x)
            {
                GameObject clone = Instantiate(tilePrefab, tilesParent);
                Tile tile = clone.GetComponent<Tile>();

                tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

                tileList.Add(tile);
                tiless = tileList;
            }
        }
    }
    public void FolderPathName(int data)
    {
        roundData = data;

        switch (data)
        {
            case 0:
                origin_Img.transform.GetChild(0).GetComponent<Image>().sprite = originImg;
                origin_Img.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = originEngImg;
                break;
            case 1:
                origin_Img.transform.GetChild(0).GetComponent<Image>().sprite = twoRound_originImg;
                origin_Img.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = twoRound_originEngImg;
                break;
            case 2:
                origin_Img.transform.GetChild(0).GetComponent<Image>().sprite = threeRound_originImg;
                origin_Img.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = threeRound_originEngImg;
                break;
            case 3:
                origin_Img.transform.GetChild(0).GetComponent<Image>().sprite = fourRound_originImg;
                origin_Img.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = fourRound_originEngImg;
                break;
        }

    }

    private IEnumerator OnSuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
            tileList[index].transform.SetAsLastSibling();

            yield return null;
        }

        EmptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
        for (int i = 0; i < manager.puzzleBtns.Length; i++)
        {
            manager.puzzleBtns[i].SetActive(true);
        }
    }
    public IEnumerator InitializeTile()
    {
        delayImg.gameObject.SetActive(false);
        manager.InitializePuzzleHint();

        yield return StartCoroutine(GameStart());

        if (tileList.Count != 0)
        {
            for (int i = 0; i < tilesParent.childCount; i++)
            {
                GameObject child = tilesParent.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        tileList = new List<Tile>();

        SpawnTiles();

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tilesParent.GetComponent<RectTransform>());

        // 현재 프레임이 종료될 때까지 대기
        yield return new WaitForEndOfFrame();

        // tileList에 있는 모든 요소의 SetCorrectPosition() 메소드 호출
        tileList.ForEach(x => x.SetCorrectPosition());

        yield return StartCoroutine(OnSuffle());

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].startSetting();
        }
        currentTime = timeLimit;
        isTimerRunning = true;
        playTimeTxt.text = "60";
        StartCoroutine(TimerCoroutine());
        Debug.Log("InitializeTile");
        //timeCoroutine = StartCoroutine(CalculatePlaytime());
    }

    public void IsMoveTile(Tile tile)
    {
        t.Add(tile);
        for (int i = 0; i < t.Count; i++)
        {
            t[i].GetComponent<Image>().color = Color.grey;
        }

        if (t.Count == 2)
        {
            StartCoroutine(TileMoveDelay());
        }
        manager.GetComponent<AudioSource>().Play();
    }

    public bool IsGameOver()
    {
        List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);
        if (tiles.Count == puzzleSize.x * puzzleSize.y)
        {
            Debug.Log("GameClear");
            StartCoroutine(BeforeSuccess());
            return true;
        }
        return false;
    }

    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            playTimeTxt.text = ((int)(currentTime)).ToString();
            // 필요한 경우 UI를 currentTime으로 업데이트합니다
            if ((int)currentTime <= 10)
            {
                playTimeTxt.color = Color.red;
            }
            else
            {
                playTimeTxt.color = Color.black;
            }
            yield return null;
        }

        if (currentTime <= 0f)
        {
            // 타이머가 만료되면 실패 애니메이션 함수를 호출합니다
            manager.FailureAnimation();
        }
    }
    public IEnumerator BeforeSuccess()
    {
        yield return StartCoroutine(GameSuccessMotion());
        yield return new WaitForEndOfFrame();
        t.Clear();
    }

    private IEnumerator TileMoveDelay()
    {
        Vector3 v0 = t[0].GetComponent<RectTransform>().localPosition;
        Vector3 v1 = t[1].GetComponent<RectTransform>().localPosition;

        t[0].GetComponent<Image>().color = Color.white;
        t[1].GetComponent<Image>().color = Color.white;

        t[0].OnMoveTo(v1);
        yield return (t[0].GetComponent<RectTransform>().localPosition == v1);
        t[1].OnMoveTo(v0);
        t.Clear();
    }
    public IEnumerator GameStart()
    {
        origin_Img.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        origin_Img.gameObject.SetActive(false);
    }
    public IEnumerator GameSuccessMotion()
    {
        isTimerRunning = false;
        origin_Img.gameObject.SetActive(true);
        Animator animator = origin_Img.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.SetTrigger("Success");

        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("Stop");

        origin_Img.gameObject.SetActive(false);
        manager.SuccessAnimation();
    }
}
