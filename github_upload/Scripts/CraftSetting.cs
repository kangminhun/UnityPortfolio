using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Vimeo;

public class CraftSetting : MonoBehaviour
{
    public VideoPlayer backPlayer;
    public VideoPlayer player;
    public Uichage craft;

    public GameObject youtubeAdvanced;
    public GameObject fade;
    [HideInInspector]
    public int number;

    public Button controller_PlayButton;
    public Slider controller_PlaySlider;

    public GameObject downloadUi;
    private int videoNumber;
    private Button selectBtn;
    public int maxButtonIndex;

    public List<float> stoptimes;
    public int btnParentsIndex;
    public GameObject btnParents;
    int videoCount;
    public Button[] stopBtns;
    private bool stop;
    private void OnEnable()
    {
        for (int i = 1; i < maxButtonIndex; i++)
        {
            int sum = i;
/*            if (gameObject.transform.GetChild(sum).GetComponent<Image>() != null)
                gameObject.transform.GetChild(sum).GetComponent<Image>().color = Color.white;*/
            if (gameObject.transform.GetChild(sum).GetComponent<Button>() != null)
                gameObject.transform.GetChild(sum).GetComponent<Button>().onClick.AddListener(() => Click(gameObject.transform.GetChild(sum).gameObject));
        }
        if (stopBtns != null)
        {
            for (int i = 0; i < stopBtns.Length; i++)
            {
                stopBtns[i].onClick.RemoveAllListeners();
            }
            for (int i = 0; i < stopBtns.Length; i++)
            {
                stopBtns[i].onClick.AddListener(() => PlayButton());
            }
        }
    }
    public void Click(bool bol)
    {
        stop = bol;
    }

    public void Click(string str)
    {
        youtubeAdvanced.GetComponent<VideoToListBackButton>().videotype = str;
    }
    public void Click(GameObject myButton)
    {
        selectBtn = myButton.GetComponent<Button>();
    }
    public IEnumerator ClickCorotine()
    {
        youtubeAdvanced.gameObject.SetActive(true);
        player.prepareCompleted -= craft.OnVideoLoaded;
        player.prepareCompleted += craft.OnVideoLoaded;
        player.loopPointReached -= youtubeAdvanced.GetComponent<VideoToListBackButton>().BackButton;
        player.loopPointReached += youtubeAdvanced.GetComponent<VideoToListBackButton>().BackButton;
        yield return StartCoroutine(VideoReady());
    }
    public IEnumerator VideoReady()
    {
        controller_PlayButton.gameObject.SetActive(true);

        player.Play();

        if (stop)
        {
            controller_PlayButton.gameObject.SetActive(false);
            btnParents.gameObject.SetActive(true);
            btnParents.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < btnParents.transform.GetChild(0).transform.childCount; i++)
            {
                if (i != btnParentsIndex)
                {
                    btnParents.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    btnParents.transform.GetChild(0).transform.GetChild(btnParentsIndex).gameObject.SetActive(true);
                }
            }
            videoCount = 0;
            StartCoroutine(VideoStop());
        }
        else
            btnParents.gameObject.SetActive(false);
        while (!craft.ready)
        {
            yield return null;
        }
        craft.ready = false;

        fade.SetActive(false);
    }
    public void PlayVideo(int videoIndex)
    {
        if (videoIndex >= 0 && videoIndex < videosToDownload.Count)
        {
            videoNumber = videoIndex;
            if (videosToDownload[videoIndex].videoFilePath == "")
            {
                string url = videosToDownload[videoIndex].videoUrl;
                StartCoroutine(GetFileSize(url,
                (size) =>
                {
                    downloadtotal = downloadUi.transform.Find("Bar").transform.Find("DownloadBar").transform.Find("total").gameObject.GetComponent<Text>();
                    downloadindex = downloadUi.transform.Find("Bar").transform.Find("DownloadBar").transform.Find("downloadindex").gameObject.GetComponent<Text>();
                    float fileSizeInMB = size / (1024f * 1024f);
                    downloadUi.transform.Find("DownloadText").GetChild(0).GetComponent<Text>().text = fileSizeInMB.ToString("F2") + " MB";
                    downloadtotal.text = fileSizeInMB.ToString("F2");
                    //Debug.Log("File Size: " + fileSizeInMB.ToString("F2") + " MB");
                }));
                downloadUi.transform.Find("DownloadText").gameObject.SetActive(true);
                downloadUi.transform.Find("DownloadButtons").gameObject.SetActive(true);
                downloadUi.transform.Find("Bar").gameObject.SetActive(false);
                downloadUi.transform.Find("DownloadButtons").transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                downloadUi.transform.Find("DownloadButtons").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DownloadOn());
                downloadUi.transform.Find("DownloadButtons").transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                downloadUi.transform.Find("DownloadButtons").transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => CancelDownload());
                downloadUi.SetActive(true);
            }
            else
            {
                DownloadOn();
            }
        }
        else
        {
            Debug.LogError("Invalid video index: " + videoIndex);
        }
    }
    public void DownloadOn()
    {
        int randomIndex = UnityEngine.Random.Range(0, downloadBarimgs.Length);
        downloadUi.transform.Find("DownloadText").gameObject.SetActive(false);
        downloadUi.transform.Find("DownloadButtons").gameObject.SetActive(false);
        downloadUi.transform.Find("Bar").GetComponent<Image>().sprite = downloadBarimgs[randomIndex];
        downloadUi.transform.Find("Bar").gameObject.SetActive(true);

        StartCoroutine(DownloadAndPlayVideosCoroutine(videoNumber));
        if (selectBtn.GetComponent<Image>() != null)
        {
            Color newColor = selectBtn.GetComponent<Image>().color;
            newColor.a = 0;
            selectBtn.GetComponent<Image>().color = newColor;
        }
    }
    public void CancelDownload()
    {
        downloadUi.SetActive(false);
    }

    [System.Serializable]
    public class VideoInfo
    {
        public string videoUrl;
        public string videoFilePath;
        public float downloadProgress; // 다운로드 진행률을 저장하는 변수 추가
        public bool isDownloaded; // 다운로드 완료 여부를 저장하는 변수 추가
    }

    public List<VideoInfo> videosToDownload = new List<VideoInfo>();
    public Slider downloadProgressSlider; // 다운로드 진행률을 표시하는 Slider
    public Text downloadProgressText; // 다운로드 진행률을 텍스트로 표시하는 Text
    private Text downloadtotal;
    private string directoryPath;
    private Text downloadindex;

    public Sprite[] downloadBarimgs;
    private IEnumerator DownloadAndPlayVideosCoroutine(int num)
    {
        player.url = "";
        fade.SetActive(true);
        backPlayer.Stop();
        // 이미 파일이 존재하는지 확인
        if (!File.Exists(videosToDownload[num].videoFilePath))
        {
            UnityWebRequest www = UnityWebRequest.Get(videosToDownload[num].videoUrl);
            Debug.Log("시작");
            var downloadOperation = www.SendWebRequest(); // 다운로드 시작
            while (!downloadOperation.isDone) // 다운로드 진행 중인 동안 반복
            {
                videosToDownload[num].downloadProgress = www.downloadProgress; // 다운로드 진행률 업데이트
                float progressRatio = videosToDownload[num].downloadProgress;
                downloadProgressSlider.value = progressRatio;
                downloadProgressText.text = (progressRatio * 100f).ToString("F1") + "%";
                float sliderValue = downloadProgressSlider.value;
                downloadindex.text = (sliderValue * float.Parse(downloadtotal.text)).ToString("F1");
                yield return null; // 한 프레임 대기
            }

            directoryPath = Path.Combine(Application.persistentDataPath, $"Appleberry_English/videos/{craft.week}/Unit{Uichage.unit + 1}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log("생성");
            }
            yield return DownloadAndPlayVideosAsync(num, www);
        }
        else
        {
            Debug.Log("파일이 이미 존재합니다: " + videosToDownload[num].videoFilePath);
            player.url = videosToDownload[num].videoFilePath.TrimEnd();
            StartCoroutine(ClickCorotine());
        }
        downloadUi.SetActive(false);
    }
    private async Task DownloadAndPlayVideosAsync(int num, UnityWebRequest www)
    {
        try
        {
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
                videosToDownload[num].videoFilePath = Path.Combine(directoryPath, fileName);

                Debug.Log(videosToDownload[num].videoFilePath);

                // 비동기 파일 쓰기 메서드 호출
                await WriteFileAsync(videosToDownload[num].videoFilePath.TrimEnd(), www.downloadHandler.data);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("파일 저장 중 오류 발생: " + ex.Message);
        }
    }
    async Task WriteFileAsync(string filePath, byte[] data)
    {
        try
        {
            // 파일 쓰기 작업을 비동기적으로 실행하고 완료될 때까지 기다립니다.
            await Task.Run(() => File.WriteAllBytes(filePath, data));
            Debug.Log("파일 저장 완료: " + filePath);

            // 파일 쓰기 작업이 완료된 후에 파일이 존재하는지 확인합니다.
            if (File.Exists(filePath))
            {
                Debug.Log("파일이 성공적으로 저장되었습니다.");
                string str = filePath;
                player.url = str;

                StartCoroutine(ClickCorotine());

            }
            else
            {
                Debug.LogError("파일이 저장되지 않았습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("파일 저장 중 오류 발생: " + ex.Message);
        }
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
    IEnumerator VideoStop()
    {
        stop = false;
        while (player.isPlaying)
        {
            if (player.time >= stoptimes[videoCount])
            {
                player.Pause();
                stopBtns[videoCount].gameObject.SetActive(true);
            }
            yield return null;
        }
    }
    public void PlayButton()
    {
        stopBtns[videoCount].gameObject.SetActive(false);
        player.Play();
        videoCount++;
        if (stoptimes.Count > videoCount)
            StartCoroutine(VideoStop());
    }
}
