using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class CardGameRoundManager : MonoBehaviour
{
    [System.Serializable]
    public class VideoInfo
    {
        public string videoUrl;
        public string videoFilePath;
        public float downloadProgress; // 다운로드 진행률을 저장하는 변수 추가
        public bool isDownloaded; // 다운로드 완료 여부를 저장하는 변수 추가
    }

    [HideInInspector] public int myRound_Index;
    [HideInInspector] public int myStation_Index;
    [HideInInspector] public int roundScore;
    public GameObject[] maps;
    public GameObject roundUi;

    public GameObject[] icons;

    public Sprite[] round_UnLock;
    public Sprite round_Lock;

    public List<VideoInfo> videosToDownload = new List<VideoInfo>();
    public Slider downloadProgressSlider; // 다운로드 진행률을 표시하는 Slider
    public Text downloadProgressText; // 다운로드 진행률을 텍스트로 표시하는 Text
    public GameObject downloadUI;
    private Text downloadtotal;
    private Text downloadindex;

    public Sprite[] downloadBarimgs;

    public VideoPlayer video;
    public GameObject fade;
    private int number;
    public AudioSource bgm;

    [SerializeField] private CardGameManager manager;
    public void StartSetting()
    {
        bgm.volume = 0;
        myRound_Index = PlayerPrefs.GetInt("round_Index");
        myStation_Index = PlayerPrefs.GetInt("stage_Index");
        Cilck(myStation_Index - 1);
    }
    public void RoundStart()
    {
        MapSet();
    }
    public void StageStart()
    {
        Cilck(myStation_Index - 1);
    }
    public void MapSet()
    {
        manager.gameObject.SetActive(false);
        roundUi.SetActive(true);

        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(false);
        }

        maps[myStation_Index - 1].SetActive(true);
        RoundSet();
    }
    private void RoundSet()
    {
        for (int i = 0; i < maps[myStation_Index - 1].transform.childCount; i++)
        {
            if (maps[myStation_Index - 1].transform.GetChild(i).GetComponent<Button>() != null)
            {
                maps[myStation_Index - 1].transform.GetChild(i).GetComponent<Image>().color = Color.gray;
                maps[myStation_Index - 1].transform.GetChild(i).GetComponent<Button>().enabled = false;

                maps[myStation_Index - 1].transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = round_Lock;
            }
        }

        for (int i = 0; i < myRound_Index; i++)
        {
            if (i < 5)
            {
                maps[myStation_Index - 1].transform.Find($"Round {i + 1}").gameObject.GetComponent<Image>().color = Color.white;
                maps[myStation_Index - 1].transform.Find($"Round {i + 1}").gameObject.GetComponent<Button>().enabled = true;

                maps[myStation_Index - 1].transform.Find($"Round {i + 1}").gameObject.transform.GetChild(0).GetComponent<Image>().sprite = round_UnLock[i];
            }
            else
            {
                maps[myStation_Index - 1].transform.Find("Boss").gameObject.GetComponent<Image>().color = Color.white;
                maps[myStation_Index - 1].transform.Find("Boss").gameObject.GetComponent<Button>().enabled = true;

                maps[myStation_Index - 1].transform.Find("Boss").gameObject.transform.GetChild(0).GetComponent<Image>().sprite = round_UnLock[i];
            }
        }
    }
    public void Exit()
    {
        manager.gameObject.SetActive(false);

        DeleteAllDownloadedVideos();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActive(true);
        }
        roundUi.SetActive(false);
        bgm.volume = 1;
    }
    IEnumerator DownloadAndPlayVideosCoroutine(int num)
    {
        if (!File.Exists(videosToDownload[num].videoFilePath))
        {
            UnityWebRequest www = UnityWebRequest.Get(videosToDownload[num].videoUrl);
            Debug.Log("시작");
            var downloadOperation = www.SendWebRequest(); // 다운로드 시작
            while (!downloadOperation.isDone) // 다운로드 진행 중인 동안 반복
            {
                videosToDownload[num].downloadProgress = www.downloadProgress; // 다운로드 진행률 업데이트
                downloadProgressSlider.value = videosToDownload[num].downloadProgress;
                downloadProgressText.text = (videosToDownload[num].downloadProgress * 100f).ToString("F1") + "%";
                float sliderValue = downloadProgressSlider.value;
                downloadindex.text = (sliderValue * float.Parse(downloadtotal.text)).ToString("F1");
                yield return null; // 한 프레임 대기
            }
            Debug.Log("끝");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download video: " + www.error);
            }
            else
            {
                // 파일 이름 추출
                string[] urlSegments = videosToDownload[num].videoUrl.Split('/');
                string fileName = urlSegments[urlSegments.Length - 1];

                // 저장 경로 설정
                videosToDownload[num].videoFilePath = Path.Combine(Application.persistentDataPath, $"Appleberry English/videos/CardGame", fileName);
                // 폴더가 존재하지 않으면 생성
                string directoryPath = Path.GetDirectoryName(videosToDownload[num].videoFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    Debug.Log("생성");
                }
                video.url = videosToDownload[num].videoFilePath;
                File.WriteAllBytes(videosToDownload[num].videoFilePath, www.downloadHandler.data);
                Debug.Log("다운로드 완료: " + videosToDownload[num].videoFilePath);
         
                MapSet();
            }
        }
        else
        {
            Debug.Log("파일이 이미 존재합니다: " + videosToDownload[num].videoFilePath);

            video.url = videosToDownload[num].videoFilePath;
            MapSet();
        }
        yield return null;

        video.Prepare();

        while (video.isPrepared)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        video.Play();

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].SetActive(false);
        }
        fade.SetActive(false);

        downloadUI.SetActive(false);
    }
    void DeleteAllDownloadedVideos()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "Appleberry English", "videos");
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true); // 'true'는 하위 폴더 및 파일도 함께 삭제하는 옵션입니다.
            Debug.Log("All downloaded videos deleted.");
        }
        else
        {
            Debug.Log("No downloaded videos found to delete.");
        }
    }
    public IEnumerator VideoStartCoroutine(int num)
    {
        fade.SetActive(true);

        yield return StartCoroutine(DownloadAndPlayVideosCoroutine(num));
    }
    public void Downloadon()
    {
        int randomIndex = UnityEngine.Random.Range(0, downloadBarimgs.Length);
        downloadUI.transform.Find("DownloadText").gameObject.SetActive(false);
        downloadUI.transform.Find("DownloadButtons").gameObject.SetActive(false);
        downloadUI.transform.Find("Bar").GetComponent<Image>().sprite = downloadBarimgs[randomIndex];
        downloadUI.transform.Find("Bar").gameObject.SetActive(true);
        StartCoroutine(VideoStartCoroutine(number));

    }
    public void CancelButton()
    {
        downloadUI.SetActive(false);
    }
    public void Cilck(int num)
    {
        string url = videosToDownload[num].videoUrl;
        number = num;
        StartCoroutine(GetFileSize(url,
        (size) =>
        {
            downloadtotal = downloadUI.transform.Find("Bar").transform.Find("DownloadBar").transform.Find("total").gameObject.GetComponent<Text>();
            downloadindex = downloadUI.transform.Find("Bar").transform.Find("DownloadBar").transform.Find("downloadindex").gameObject.GetComponent<Text>();
            float fileSizeInMB = size / (1024f * 1024f);
            downloadUI.transform.Find("DownloadText").GetChild(0).GetComponent<Text>().text = fileSizeInMB.ToString("F2") + " MB";
            downloadtotal.text = fileSizeInMB.ToString("F2");
        }));
        downloadUI.transform.Find("DownloadText").gameObject.SetActive(true);
        downloadUI.transform.Find("DownloadButtons").gameObject.SetActive(true);
        downloadUI.transform.Find("Bar").gameObject.SetActive(false);
        downloadUI.transform.Find("DownloadButtons").transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        downloadUI.transform.Find("DownloadButtons").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Downloadon());
        downloadUI.transform.Find("DownloadButtons").transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        downloadUI.transform.Find("DownloadButtons").transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => CancelButton());
        downloadUI.SetActive(true);
    }
    IEnumerator GetFileSize(string url, Action<long> resut)
    {
        UnityWebRequest uwr = UnityWebRequest.Head(url);
        yield return uwr.SendWebRequest();
        string size = uwr.GetResponseHeader("Content-Length");

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("Error While Getting Length: " + uwr.error);
            if (resut != null)
                resut(-1);
        }
        else
        {
            if (resut != null)
                resut(Convert.ToInt64(size));
        }
    }
}
