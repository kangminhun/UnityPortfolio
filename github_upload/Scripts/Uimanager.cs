using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uimanager : MonoBehaviour
{
    public static Uimanager instance;
    public GameObject[] Uis;
    public GameObject changAniManager;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    // 0 -> 로그인창
    // 1 -> 목차
    // 2 -> 게임
    // 3 -> 영상

    public void ListButtonClick(int num)
    {
        LoginUiChange(Uimanager.instance.Uis[1], false, Uimanager.instance.Uis[num], true);
    }
    public void ComeBackList()
    {
        for (int i = 0; i < Uimanager.instance.Uis.Length; i++)
        {
            LoginUiChange(Uimanager.instance.Uis[i], false, Uimanager.instance.Uis[1], true);
        }
    }
    public void LoginUiChange(GameObject obj_1, bool obj_1Value, GameObject obj_2, bool obj_2Value)
    {
        StartCoroutine(Fade(obj_1, obj_1Value, obj_2, obj_2Value));
    }
    public void LoginUiChange(GameObject obj_1, bool obj_1Value, GameObject obj_2, bool obj_2Value,GameObject obj_3)
    {
        StartCoroutine(Fade(obj_1, obj_1Value, obj_2, obj_2Value, obj_3));
    }
    IEnumerator Fade(GameObject obj_1, bool obj_1Value, GameObject obj_2, bool obj_2Value)
    {
        changAniManager.SetActive(true);
        Animator animator = changAniManager.GetComponent<Animator>();
        animator.SetTrigger("Up");
        yield return new WaitForSeconds(.7f);
        obj_1.SetActive(obj_1Value);
        obj_2.SetActive(obj_2Value);
        yield return new WaitForSeconds(.6f);
        changAniManager.SetActive(false);
    }
    IEnumerator Fade(GameObject obj_1, bool obj_1Value, GameObject obj_2, bool obj_2Value, GameObject obj_3)
    {
        changAniManager.SetActive(true);
        Animator animator = changAniManager.GetComponent<Animator>();
        animator.SetTrigger("Up");
        yield return new WaitForSeconds(.7f);
        obj_1.SetActive(obj_1Value);
        obj_3.SetActive(false);
        obj_2.SetActive(obj_2Value);
        yield return new WaitForSeconds(1f);
        changAniManager.SetActive(false);
    }
}
