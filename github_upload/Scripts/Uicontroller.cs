using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Uicontroller : MonoBehaviour
{
    public Uichage sc;
    public Button buttons;
    public ListButtonInformation[] information;
    public Button nextButton;
    public Button previousButton;
    public GameObject bg;
    public GameObject[] bgList;
    public AudioSource audioPlayer;

    [HideInInspector]
    public List<int> OpenUnit;

    public Button[] unitButtons;
    int num = 0;
    public void Set()
    {
        num = 0;
        Uichage.unit = 0;
        sc.select = false;
        if (num < information.Length - 1)
        {
            OpenUnit = new List<int>();
            previousButton.gameObject.SetActive(false);
            buttons.enabled = true;
            buttons.onClick.RemoveAllListeners();
            buttons.onClick.AddListener(() => sc.Cilck(num));
            bg.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = information[num].mainUi;

            for (int i = 0; i < unitButtons.Length; i++)
            {
                unitButtons[i].GetComponent<Image>().color = Color.gray;
                unitButtons[i].GetComponent<Image>().sprite = information[num].unitButtonImgs[i];
                unitButtons[i].onClick.RemoveAllListeners();
            }

            OpenUnit.Add(0);
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_USER)
            {
                unitButtons[0].GetComponent<Image>().color = Color.white;
                unitButtons[0].GetComponent<Image>().sprite = information[num].unitButtonImgs[0];
                unitButtons[0].onClick.AddListener(() => sc.Unit(0));
            }
            else if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_PREMIUM)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_TEST)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }

            for (int i = 0; i < bgList.Length; i++)
            {
                bgList[i].gameObject.SetActive(false);
            }
            bgList[num].SetActive(true);
            audioPlayer.clip = information[num].clip;
            audioPlayer.Play();
            previousButton.gameObject.SetActive(false);
        }
        else if (num == information.Length)
        {
            nextButton.gameObject.SetActive(false);
        }

    }
    public void Next()
    {
        if (num < information.Length - 1)
        {
            Uichage.unit = 0;
            sc.select = false;
            OpenUnit = new List<int>();
            num++;
            previousButton.gameObject.SetActive(true);

            buttons.enabled = true;
            buttons.onClick.RemoveAllListeners();
            buttons.onClick.AddListener(() => sc.Cilck(num));
            bg.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = information[num].mainUi;

            for (int i = 0; i < unitButtons.Length; i++)
            {
                unitButtons[i].GetComponent<Image>().color = Color.gray;
                unitButtons[i].GetComponent<Image>().sprite = information[num].unitButtonImgs[i];
                unitButtons[i].onClick.RemoveAllListeners();
            }
            OpenUnit.Add(0);
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_USER)
            {
                unitButtons[0].GetComponent<Image>().color = Color.white;
                unitButtons[0].GetComponent<Image>().sprite = information[num].unitButtonImgs[0];
                unitButtons[0].onClick.AddListener(() => sc.Unit(0));
            }
            else if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_PREMIUM)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_TEST)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }
            for (int i = 0; i < bgList.Length; i++)
            {
                bgList[i].gameObject.SetActive(false);
            }
            bgList[num].SetActive(true);
            audioPlayer.clip = information[num].clip;
            audioPlayer.Play();
        }
        else if (num == information.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
        }
    }
    public void Previous()
    {
        if (num > 0)
        {
            Uichage.unit = 0;
            sc.select = false;
            OpenUnit = new List<int>();
            num--;
            nextButton.gameObject.SetActive(true);

            buttons.enabled = true;
            buttons.onClick.RemoveAllListeners();
            buttons.onClick.AddListener(() => sc.Cilck(num));
            bg.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = information[num].mainUi;

            for (int i = 0; i < unitButtons.Length; i++)
            {
                unitButtons[i].GetComponent<Image>().color = Color.gray;
                unitButtons[i].GetComponent<Image>().sprite = information[num].unitButtonImgs[i];
                unitButtons[i].onClick.RemoveAllListeners();
            }

            OpenUnit.Add(0);
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_USER)
            {
                unitButtons[0].GetComponent<Image>().color = Color.white;
                unitButtons[0].GetComponent<Image>().sprite = information[num].unitButtonImgs[0];
                unitButtons[0].onClick.AddListener(() => sc.Unit(0));
            }
            else if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_PREMIUM)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }
            if (DataBase.instance.LoginManager.memberType == Member_group.ROLE_TEST)
            {
                for (int i = 0; i < information[num].openUnitNumbers.Length; i++)
                {
                    int sum = i;

                    unitButtons[sum].GetComponent<Image>().color = Color.white;
                    unitButtons[sum].GetComponent<Image>().sprite = information[num].unitButtonImgs[sum];
                    unitButtons[sum].onClick.AddListener(() => sc.Unit(information[num].openUnitNumbers[sum] - 1));
                }
            }

            for (int i = 0; i < bgList.Length; i++)
            {
                bgList[i].gameObject.SetActive(false);
            }
            bgList[num].SetActive(true);
            audioPlayer.clip = information[num].clip;
            audioPlayer.Play();
        }
        else if (num == 0)
        {
            previousButton.gameObject.SetActive(false);
        }
    }
}
