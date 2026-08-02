using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Vimeo.Player;
public class ListScroll : MonoBehaviour
{
    public Button reftBtn;
    public Button lightBtn;
    public GameObject[] uis;
    [HideInInspector]
    public int number;
    private int count;
    public GameObject scroll;
    private ScrollRect scrollRect;
    private RectTransform viewportTransform;
    public GameObject[] header;
    public GameObject[] openUis;
    public GameObject openUisParents;
    //public VimeoPlayer vp;
    public VideoPlayer player;
    public GameObject pointMotion;

    private Text[] unitTexts;
    private Text[] weekTexts;
    private int unit;
    private int week;
    public void OnEnable()
    {
        lightBtn.gameObject.SetActive(false);
        reftBtn.gameObject.SetActive(true);
        reftBtn.onClick.RemoveAllListeners();
        lightBtn.onClick.RemoveAllListeners();
        scrollRect = header[number].transform.GetChild(Uichage.unit).GetComponent<ScrollRect>();
        uis = new GameObject[header[number].transform.GetChild(Uichage.unit).GetComponent<ScrollRect>().content.transform.childCount];
        for (int i = 0; i < header[number].transform.childCount; i++)
        {
            if (i != Uichage.unit)
                header[number].transform.GetChild(i).gameObject.SetActive(false);
            else
                header[number].transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int j = 0; j < header[number].transform.GetChild(Uichage.unit).GetComponent<ScrollRect>().content.transform.childCount; j++)
        {
            int num = j;
            uis[j] = header[number].transform.GetChild(Uichage.unit).GetComponent<ScrollRect>().content.transform.GetChild(j).gameObject;
        }
        reftBtn.onClick.AddListener(() => ReftClick());
        lightBtn.onClick.AddListener(() => LightClick());
        viewportTransform = scrollRect.viewport;
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
        scroll = scrollRect.content.gameObject;
    }

    public void Setting()
    {
        count = 0;
        lightBtn.gameObject.SetActive(false);
        scroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(count * -1920, 313);
    }
    public void ReftClick()
    {
        if (count < uis.Length - 1)
        {
            count++;
            lightBtn.gameObject.SetActive(true);
            scroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(count * -1920, 313);
            scrollRect.inertia = false;
        }
        else
        {
            reftBtn.gameObject.SetActive(false);
            return;
        }
    }
    public void LightClick()
    {
        if (count > 0)
        {
            count--;
            reftBtn.gameObject.SetActive(true);
            scroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(count * -1920, 313);
            scrollRect.inertia = false;
        }
        else
        {
            lightBtn.gameObject.SetActive(false);
            return;
        }
    }
    private void OnScrollChanged(Vector2 value)
    {
        scrollRect.inertia = true;
        float minDist = float.MaxValue;
        Transform closestChild = null;
        int childIndex = 0;
        // 가장 가운데에 가까운 자식 요소를 찾습니다.
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            Transform child = scrollRect.content.GetChild(i);
            float dist = Mathf.Abs(child.position.x - (viewportTransform.position.x + 10));

            if (dist < minDist)
            {
                minDist = dist;
                closestChild = child;
            }
            if (scrollRect.content.GetChild(i) == closestChild)
            {
                childIndex = i;
            }
        }
        count = childIndex;
        if (count == 0)
        {
            lightBtn.gameObject.SetActive(false);
        }
        else if (count == uis.Length - 1)
        {
            reftBtn.gameObject.SetActive(false);
        }
        else
        {
            reftBtn.gameObject.SetActive(true);
            lightBtn.gameObject.SetActive(true);
        }

    }
    private GameObject obj;

    public void OpenUi(int num)
    {
        player.Pause();
        openUisParents.SetActive(true);
        for (int i = 0; i < openUis.Length; i++)
        {
            openUis[i].gameObject.SetActive(false);
        }

        openUis[num].gameObject.SetActive(true);
        obj = openUis[num];
    }
    public void OpenUi2(int num2)
    {
        obj.transform.GetChild(num2).gameObject.SetActive(true);
    }
    public void CloseUi()
    {
        player.Play();
        for (int i = 0; i < openUis.Length; i++)
        {
            if (openUis[i].transform.childCount != 0)
                for (int k = 0; k < openUis[i].transform.childCount; k++)
                {
                    openUis[i].transform.GetChild(k).gameObject.SetActive(false);
                }
            openUis[i].gameObject.SetActive(false);
        }
        openUisParents.SetActive(false);
    }
    public void PointMotionOn()
    {
        StartCoroutine(PointMotion());
    }
    private IEnumerator PointMotion()
    {
        pointMotion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        pointMotion.SetActive(false);
    }
}
