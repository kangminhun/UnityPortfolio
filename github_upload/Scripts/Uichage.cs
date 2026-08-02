using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class Uichage : MonoBehaviour
{
    public GameObject uiParent;
    public GameObject[] uis;
    public GameObject fade;

    public VideoPlayer video;
    public GameObject uicontroller;
    public ListScroll scroll;
    public VideoToListBackButton listBackButton;
    public GameObject unitSelectUi;
    public static int unit;
    public string week;

    public GameObject uIViewContainer;

    [HideInInspector]
    public bool select;
    [HideInInspector]
    public bool ready;
    public void UIViewControllerOpen(string child)
    {
        if (child != "Mailbox" && child != "Ranking" && child != "Attendance")
            uicontroller.GetComponent<AudioSource>().Stop();
        uIViewContainer.SetActive(true);
        uIViewContainer.transform.Find(child).gameObject.SetActive(true);
    }
    public void UIViewControllerClose(string child)
    {
        if (child != "Mailbox" && child != "Ranking" && child != "Attendance")
            uicontroller.GetComponent<AudioSource>().Play();
        uIViewContainer.SetActive(false);
        uIViewContainer.transform.Find(child).gameObject.SetActive(false);
    }
    public void Cilck(int num)
    {
        if (select)
        {
            scroll.number = num;
            string url = videosToDownload[num].videoUrl;
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
            uicontroller.GetComponent<AudioSource>().Stop();
            select = false; // -> 유닛 선택을 완료 후 플레이 버튼을 누르면 완료
        }
        else
            unitSelectUi.SetActive(true);
    }
    public void Back()
    {
        video.gameObject.SetActive(false);
        StartCoroutine(VideoEndCoroutine());
        uicontroller.GetComponent<AudioSource>().Play();
        DeleteAllDownloadedVideos();
    }
    public void UnitSelect()
    {
        if (!unitSelectUi.activeSelf)
            unitSelectUi.SetActive(true);
        else
            unitSelectUi.SetActive(false);
        // Select 버튼 누르면 호출
    }
    public void Unit(int num)
    {
        select = true; // -> 유닛 선택전에 플레이를 눌러서 버그가 나오는걸 방지
        unitSelectUi.SetActive(false);
        // Select 버튼 눌러서 나온 UI에 있는 버튼 누르면 호출
        unit = num;
    }
    public IEnumerator VideoStartCoroutine(int num)
    {
        fade.SetActive(true);
        uicontroller.gameObject.SetActive(false);
        uiParent.SetActive(true);
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].SetActive(false);
        }
        uis[num].SetActive(true);
        week = uis[num].name;
        //uis[num].gameObject.transform.Find("Unit " + unit+1);

        scroll.Setting();
        video.gameObject.SetActive(true);
        video.prepareCompleted -= OnVideoLoaded;
        video.prepareCompleted += OnVideoLoaded;
        listBackButton.listUrl = videosToDownload[num].videoUrl;
        yield return StartCoroutine(DownloadAndPlayVideosCoroutine(num));
        video.prepareCompleted -= OnVideoLoaded;
    }
    public void OnVideoLoaded(VideoPlayer vp)
    {
        ready = true;
        Debug.Log("시작");
    }
    public IEnumerator VideoEndCoroutine()
    {
        fade.SetActive(true);
        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].SetActive(false);
        }
        uiParent.SetActive(false);
        video.gameObject.SetActive(false);
        uicontroller.gameObject.SetActive(true);
        yield return StartCoroutine(CountReady());
    }

    public IEnumerator CountReady()
    {
        float num = 0;
        while (10 < num)
        {
            num += Time.deltaTime;
            yield return null;
        }
        fade.SetActive(false);
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
    public GameObject downloadUI;
    private Text downloadtotal;
    private Text downloadindex;

    public Sprite[] downloadBarimgs;
    public void Downloadon()
    {
        int randomIndex = UnityEngine.Random.Range(0, downloadBarimgs.Length);
        downloadUI.transform.Find("DownloadText").gameObject.SetActive(false);
        downloadUI.transform.Find("DownloadButtons").gameObject.SetActive(false);
        downloadUI.transform.Find("Bar").GetComponent<Image>().sprite = downloadBarimgs[randomIndex];
        downloadUI.transform.Find("Bar").gameObject.SetActive(true);
        StartCoroutine(VideoStartCoroutine(scroll.number));
    }
    public void CancelButton()
    {
        downloadUI.SetActive(false);
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
                videosToDownload[num].videoFilePath = Path.Combine(Application.persistentDataPath, $"Appleberry English/videos/{week}/Unit{unit + 1}", fileName);
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
            }
        }
        else
        {
            Debug.Log("파일이 이미 존재합니다: " + videosToDownload[num].videoFilePath);
        }
        yield return null;
        downloadUI.SetActive(false);
        StartCoroutine(VideoReady());
    }
    IEnumerator VideoReady()
    {
        video.Play();
        yield return new WaitForSeconds(1);
        ready = false;
        fade.SetActive(false);
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

