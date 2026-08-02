using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoToListBackButton : MonoBehaviour
{
    public string listUrl;
    public Uichage bg;
    public VideoPlayer vp;
    public VideoPlayer backVp;
    public GameObject fade;
    public GameObject startui;
    public GameObject BtnParents;


    public ListScroll scroll;
    public GameObject endUi;
    public AudioSource resultAudio;
    public GameObject result;
    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;
    public string videotype;
    private bool back;
    public void BackButton(VideoPlayer vpr)
    {
        result.SetActive(true);
        vp.loopPointReached -= BackButton;
        back = true;
        scene_FX_Confetti1.Play();
        StartCoroutine(GoldParticleOn());
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Success");
        if (videotype == "side")
        {
            DataBase.instance.PointManager.PointUp(100);
            result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = $"100";
        }
        else if(videotype=="main")
        {
            DataBase.instance.PointManager.PointUp(400);
            result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = $"400";
        }
    }
    public IEnumerator GoldParticleOn()
    {
        goldParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        goldParticle.gameObject.SetActive(true);
    }
    public void Back()
    {
        result.SetActive(false);
        for (int i = 0; i < BtnParents.transform.childCount; i++)
        {
            if(BtnParents.transform.GetChild(i).gameObject.activeSelf)
            {
                for (int j = 0; j < BtnParents.transform.GetChild(i).gameObject.transform.childCount; j++)
                {
                    BtnParents.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject.SetActive(false);
                }
                BtnParents.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        fade.SetActive(true);
        vp.url = "";
        backVp.url = listUrl;
        backVp.Play();
        backVp.prepareCompleted -= bg.OnVideoLoaded;
        backVp.prepareCompleted += bg.OnVideoLoaded;
        StartCoroutine(VideoReady());
    }
    public IEnumerator VideoReady()
    {
        yield return new WaitForSeconds(1);
        while (!bg.ready)
        {
            yield return null;
        }
        bg.ready = false;
        fade.SetActive(false);
        gameObject.SetActive(false);
        if (back)
            scroll.PointMotionOn();
        back = false;
    }
    public void Play()
    {
        vp.Play();
        startui.SetActive(false);
    }
}
