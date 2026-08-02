using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vimeo.SimpleJSON;

public class ShopItemManager : MonoBehaviour
{
    private List<Item> items_All;
    private List<Item> items_People;
    private List<Item> items_Robots;
    private List<Item> items_Plants;
    private List<Item> items_Animals;
    public GameObject itemprefab;
    public Uichage uichage;
    private GameObject itemObject;
    private List<GameObject> itemList;
    private const string myCardUrl = "https://your-server-domain.com/v1/card-purchase/my-cards";
    [SerializeField] private List<string> myCardName;
    private List<Item> myCardList;
    [SerializeField] private GameObject myChoice;
    [SerializeField] private GameObject buyUi;
    [SerializeField] private List<Item> myitemType_People;
    [SerializeField] private List<Item> myitemType_Robots;
    [SerializeField] private List<Item> myitemType_Plants;
    [SerializeField] private List<Item> myitemType_Animals;
    [SerializeField] private List<Item> myitemType_All;
    [HideInInspector] public List<Item> myitemType_All_Sharing;
    [SerializeField] private Item myitem;
    private int myitemIndex;
    public List<Item> myChoiceList;
    public List<int> myCardID;
    public List<string> myCardStatus;
    [SerializeField] private GameObject[] cardIcons;
    [SerializeField] private Sprite[] originIcons;
    [SerializeField] private Sprite[] changeIcons;
    [SerializeField] private Button unLockBtn;
    [SerializeField] private Sprite[] unLockBtnImgs;
    [SerializeField] private Button myCard_NextButton;
    [SerializeField] private Button myCard_PreviousButton;
    [SerializeField] private GameObject[] inventoryChild;
    [SerializeField] private GameObject card;

    [SerializeField] private GameObject exclamation_mark;

    [SerializeField] private Sprite[] cardImgs;
    [SerializeField] private Sprite[] cardIconImgs;
    [SerializeField] private Sprite[] cardLine;
    [SerializeField] private Sprite[] choiceCardLine;
    [SerializeField] private Sprite[] buttonImgs;
    [SerializeField] private Sprite[] buttonOriginImgs;
    private int unilockCount;
    private int count;
    private int myCardCount;
    [SerializeField] private Text unLockCountTxt;

    //[SerializeField] private GameObject character2DObject;
    //[SerializeField] private Sprite[] characterNookiImg;
    [SerializeField] private Sprite[] unLockCardLine;

    [SerializeField] private Image fadeOutUIImage;
    [SerializeField] private float fadeDuration = 1f;

    private bool delay;
    private GameObject cardinfo;
    private void Start()
    {
        items_All = new List<Item>();
        items_People = new List<Item>();
        items_Robots = new List<Item>();
        items_Plants = new List<Item>();
        items_Animals = new List<Item>();
        myCardList = new List<Item>();
        myCard_NextButton.onClick.AddListener(() => MyCardNextButton());
        myCard_PreviousButton.onClick.AddListener(() => MyCardPreviousButton());
    }
    public void Exclamation_markInvek()
    {
        InvokeRepeating("Exclamation_markSet", 0f, 5f);
    }
    private void Exclamation_markSet()
    {
        count = 0;
        unilockCount = 0;
        myCardCount = myitemType_All.Count;
        if (myitemType_All.Count > 0)
        {
            for (int i = 0; i < myitemType_All.Count; i++)
            {
                if (myitemType_All[i].cardStatus != "언락완료")
                {
                    count++;
                }
                else
                {
                    unilockCount++;
                }
            }
            if (count > 0)
            {
                exclamation_mark.SetActive(true);
            }
            else
            {
                exclamation_mark.SetActive(false);
            }
        }
        unLockCountTxt.text = $"{unilockCount} / {myCardCount}";
        Debug.Log($"{unilockCount} / {myCardCount}");
    }
    public void ItemSet(int id, string cardName, string cardType, int diamond, string memo, int point, int sid, int star, string isUse, int healthPoint, int power,string property,string skillEffect, string skillEffect2, string skillEffect3)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.cardId = id;
        item.cardName = cardName;
        item.cardType = cardType;
        item.diamond = diamond;
        item.memo = memo;
        item.point = point;
        item.sid = sid;
        item.star = star;
        if (isUse == "true")
            item.isUse = true;
        else
            item.isUse = false;
        item.healthPoint = healthPoint;
        item.power = power;
        item.property = property;
        item.skillEffect = skillEffect;
        item.skillEffect2 = skillEffect2;
        item.skillEffect3 = skillEffect3;
        items_All.Add(item);
        switch (cardType)
        {
            case "PEOPLE":
                items_People.Add(item);
                break;
            case "ROBOTS":
                items_Robots.Add(item);
                break;
            case "PLANTS":
                items_Plants.Add(item);
                break;
            case "ANIMALS":
                items_Animals.Add(item);
                break;
        }
    }
    public void ShopList(Item itemInfo, int num, string type)
    {
        if (!myCardList.Contains(itemInfo) && !delay)
        {
            if (type == "POINT")
            {
                if (DataBase.instance.PointManager.myPoint - itemInfo.point >= 0)
                {
                    buyUi.SetActive(false);
                    DataBase.instance.WebRequestManager.CardBuyRequest(itemInfo.cardId, type);
                    myCardName.Add(itemInfo.cardName);
                    itemList[num].gameObject.GetComponent<Button>().enabled = false;
                    itemList[num].gameObject.transform.Find("SoldOut").gameObject.SetActive(true);
                    itemList[num].gameObject.transform.Find("SoldOutBack").gameObject.SetActive(true);
                    DataBase.instance.AudioManager.AudioPlay(1);
                    delay = true;
                }
                else
                {
                    Debug.Log($"{type}이 부족합니다");
                    Purchase_failed();
                }
            }
            else if (type == "DIAMOND")
            {
                if (DataBase.instance.PointManager.myDiamond - itemInfo.diamond >= 0)
                {
                    buyUi.SetActive(false);
                    DataBase.instance.WebRequestManager.CardBuyRequest(itemInfo.cardId, type);
                    myCardName.Add(itemInfo.cardName);
                    itemList[num].gameObject.GetComponent<Button>().enabled = false;
                    itemList[num].gameObject.transform.Find("SoldOut").gameObject.SetActive(true);
                    itemList[num].gameObject.transform.Find("SoldOutBack").gameObject.SetActive(true);
                    DataBase.instance.AudioManager.AudioPlay(1);
                    delay = true;
                }
                else
                {
                    Debug.Log($"{type}이 부족합니다");
                    Purchase_failed();

                }
            }
        }
        else
        {
            Debug.Log("이미 있는 제품입니다");
            Purchase_failed();
        }
    }
    public void Purchase_failed()
    {
        buyUi.SetActive(true);
        buyUi.transform.Find("Buy").gameObject.SetActive(false);
        buyUi.transform.Find("Fail").gameObject.SetActive(true);
        DataBase.instance.AudioManager.AudioPlay(0);
    }
    public void BuyUiOpen(Item itemInfo, int num)
    {
        buyUi.SetActive(true);
        buyUi.transform.Find("Buy").gameObject.SetActive(true);
        buyUi.transform.Find("Fail").gameObject.SetActive(false);
        buyUi.transform.Find("Buy").transform.Find("Buy_by_diamond").GetComponent<Button>().onClick.RemoveAllListeners();
        buyUi.transform.Find("Buy").transform.Find("Buy_by_point").GetComponent<Button>().onClick.RemoveAllListeners();

        buyUi.transform.Find("Buy").transform.Find("Buy_by_diamond").GetComponent<Button>().onClick.AddListener(() => ShopList(itemInfo, num, "DIAMOND"));
        buyUi.transform.Find("Buy").transform.Find("Buy_by_point").GetComponent<Button>().onClick.AddListener(() => ShopList(itemInfo, num, "POINT"));
    }
    public void BuyUiClose()
    {
        buyUi.SetActive(false);
    }
    public void MyChoiceCard(Item itemInfo, int num)
    {
        DataBase.instance.AudioManager.AudioPlay(2);
        myChoice.transform.Find("CardSet").gameObject.SetActive(true);
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.Find("Effact").gameObject.SetActive(false);
            itemList[i].transform.Find("StarEff").gameObject.SetActive(false);
        }
        itemList[num].transform.Find("Effact").gameObject.SetActive(true);
        itemList[num].transform.Find("StarEff").gameObject.SetActive(true);
        myChoice.transform.Find("CardSet").GetComponent<Animator>().SetTrigger("Open");
        myChoice.transform.Find("CardSet").transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = itemInfo.cardName;
        myChoice.transform.Find("CardSet").transform.Find("diamond").GetComponentInChildren<Text>().text = itemInfo.diamond.ToString();
        myChoice.transform.Find("CardSet").transform.Find("gold").GetComponentInChildren<Text>().text = itemInfo.point.ToString();
        if (itemInfo.star > 0 && itemInfo.star < 4)
        {
            for (int i = 0; i < myChoice.transform.Find("CardSet").transform.Find("Stars").gameObject.transform.childCount; i++)
            {
                myChoice.transform.Find("CardSet").transform.Find("Stars").gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < itemInfo.star; i++)
            {
                myChoice.transform.Find("CardSet").transform.Find("Stars").gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        myChoice.transform.Find("CardSet").transform.Find("Memo").GetComponent<TextMeshProUGUI>().text = itemInfo.memo;
        myChoice.transform.Find("CardSet").transform.Find("CardIcon").GetComponent<Image>().sprite = itemList[num].transform.Find("Icon").GetComponent<Image>().sprite;

        myChoice.transform.Find("CardSet").transform.Find("BuyButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        myChoice.transform.Find("CardSet").transform.Find("BuyButton").gameObject.GetComponent<Button>().onClick.AddListener(() => BuyUiOpen(itemInfo, num));

        switch(itemInfo.cardType)
        {
            case "PEOPLE":
                myChoice.transform.Find("CardSet").transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                myChoice.transform.Find("CardSet").transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[0];
                break;
            case "ROBOTS":
                myChoice.transform.Find("CardSet").transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                myChoice.transform.Find("CardSet").transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[1];
                break;
            case "PLANTS":
                myChoice.transform.Find("CardSet").transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                myChoice.transform.Find("CardSet").transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[2];
                break;
            case "ANIMALS":
                myChoice.transform.Find("CardSet").transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                myChoice.transform.Find("CardSet").transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[3];
                break;
        }
        for (int i = 0; i < cardImgs.Length; i++)
        {
            if (itemInfo.cardName == cardImgs[i].name)
            {
                myChoice.transform.Find("CardSet").transform.Find("Character").gameObject.GetComponent<Image>().sprite = cardImgs[i];
            }
        }
    }
    public void MyCardList()
    {
        StartCoroutine(MyCard());
    }
    public IEnumerator MyCard()
    {
        myCardName = new List<string>();

        // 리스트들을 루프 밖에서 초기화
        myitemType_People = new List<Item>(); // YourItemType에 맞게 타입을 변경해주세요.
        myitemType_Robots = new List<Item>();
        myitemType_Plants = new List<Item>();
        myitemType_Animals = new List<Item>();
        myitemType_All_Sharing=new List<Item>();

        using (UnityWebRequest request = UnityWebRequest.Get(myCardUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            // JSON 파싱
            JSONNode json = JSONClass.Parse(request.downloadHandler.text);

            // "data" 객체에서 "point" 키에 해당하는 값을 가져옴
            JSONArray listArray = json["list"].AsArray;

            // 각 배열 요소에 접근하여 id와 card 값을 가져오기
            for (int i = 0; i < listArray.Count; i++)
            {
                int id = listArray[i]["id"].AsInt;
                string card = listArray[i]["card"].Value;
                string status = listArray[i]["cardStatus"].Value;

                // 여기서 id와 card 값을 사용하거나 출력하면 됩니다.
                myCardName.Add(card.Trim());

                Debug.Log($"{id} : {card} : {status}");

                myCardList = items_All.Where(item => item.cardName == card).ToList();

                myitemType_People.AddRange(myCardList.Where(item => item.cardType == "PEOPLE")
                                                    .Select(item => { item.cardPurchaseId = id; item.cardStatus = status; return item; }));
                myitemType_Robots.AddRange(myCardList.Where(item => item.cardType == "ROBOTS")
                                                    .Select(item => { item.cardPurchaseId = id; item.cardStatus = status; return item; }));
                myitemType_Plants.AddRange(myCardList.Where(item => item.cardType == "PLANTS")
                                                    .Select(item => { item.cardPurchaseId = id; item.cardStatus = status; return item; }));
                myitemType_Animals.AddRange(myCardList.Where(item => item.cardType == "ANIMALS")
                                                    .Select(item => { item.cardPurchaseId = id; item.cardStatus = status; return item; }));
            }
            myitemType_All = new List<Item>();
            for (int i = 0; i < myitemType_People.Count; i++)
            {
                myitemType_All.Add(myitemType_People[i]);
            }
            for (int i = 0; i < myitemType_Robots.Count; i++)
            {
                myitemType_All.Add(myitemType_Robots[i]);
            }
            for (int i = 0; i < myitemType_Plants.Count; i++)
            {
                myitemType_All.Add(myitemType_Plants[i]);
            }
            for (int i = 0; i < myitemType_Animals.Count; i++)
            {
                myitemType_All.Add(myitemType_Animals[i]);
            }
            myitemType_All_Sharing = myitemType_All;
            Exclamation_markSet();
            delay = false;
        }
    }

    public void ShopAllOpen()
    {
        BuyUiClose();
        itemList = new List<GameObject>();
        ShopType("ALL");
    }
    public void ShopSetting(List<Item> listType)
    {
        if (itemList.Count != 0 && itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                int sum = i;
                Destroy(itemList[sum]);
            }
        }
        itemList = new List<GameObject>();
        myCardList = new List<Item>();
        uichage.UIViewControllerOpen("ShopBox");
        List<Item> sortedItems = listType.OrderBy(item => item.sid).ToList();
        ScrollRect scrollView = uichage.uIViewContainer.transform.Find("ShopBox").gameObject.GetComponentInChildren<ScrollRect>();
        for (int i = 0; i < sortedItems.Count; i++)
        {
            int sum = i;
            itemObject = Instantiate(itemprefab, scrollView.content.transform);
            itemObject.transform.Find("Effact").gameObject.SetActive(false);
            itemObject.transform.Find("StarEff").gameObject.SetActive(false);
            itemObject.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = sortedItems[sum].cardName;
            for (int k = 0; k < cardImgs.Length; k++)
            {
                int data = k;
                //if (sortedItems[sum].cardName == cardImgs[k].name)
                if (string.Equals(sortedItems[sum].cardName, cardImgs[k].name, StringComparison.OrdinalIgnoreCase))
                {
                    itemObject.transform.Find("Masking").transform.Find("CharacterImage").gameObject.GetComponent<Image>().sprite = cardImgs[data];
                }
            }
            switch (sortedItems[sum].cardType)
            {
                case "PEOPLE":
                    itemObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                    itemObject.transform.Find("CardLine").gameObject.GetComponent<Image>().sprite = cardLine[0];
                    break;
                case "ROBOTS":
                    itemObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                    itemObject.transform.Find("CardLine").gameObject.GetComponent<Image>().sprite = cardLine[1];
                    break;
                case "PLANTS":
                    itemObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                    itemObject.transform.Find("CardLine").gameObject.GetComponent<Image>().sprite = cardLine[2];
                    break;
                case "ANIMALS":
                    itemObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                    itemObject.transform.Find("CardLine").gameObject.GetComponent<Image>().sprite = cardLine[3];
                    break;
            }
            if (!myCardName.Contains(sortedItems[sum].cardName))
            {
                if (sortedItems[sum].star > 3)
                {
                    sortedItems[sum].star = 3;
                }
                else if (sortedItems[sum].star <= 0)
                {
                    sortedItems[sum].star = 1;
                }

                for (int j = 0; j < sortedItems[sum].star; j++)
                {
                    itemObject.transform.Find("Star").GetChild(j).gameObject.SetActive(true);
                }
                itemObject.GetComponent<Button>().onClick.AddListener(() => MyChoiceCard(sortedItems[sum], sum));

            }
            else
            {
                itemObject.transform.Find("SoldOut").gameObject.SetActive(true);
                itemObject.transform.Find("SoldOutBack").gameObject.SetActive(true);
                itemObject.GetComponent<Button>().enabled = false;
                myCardList.Add(sortedItems[sum]);
            }
            itemList.Add(itemObject);
        }
    }
    public void ShopClose()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            int sum = i;
            Destroy(itemList[sum]);
        }
        uichage.UIViewControllerClose("ShopBox");
    }
    public void MyCardOpen()
    {
        uichage.UIViewControllerOpen("Inventory");
        Debug.Log(myCardList.Count);
        if (myCardList.Count == 0)
        {
            uichage.uIViewContainer.transform.Find("Inventory").transform.Find("MyCard").transform.Find("CardSet").transform.gameObject.SetActive(false);
        }
        else
        {
            uichage.uIViewContainer.transform.Find("Inventory").transform.Find("MyCard").transform.Find("CardSet").transform.gameObject.SetActive(true);
        }
        if (myitemType_People.Count != 0)
        {
            cardIcons[0].GetComponent<Image>().sprite = changeIcons[0];
            cardIcons[0].GetComponent<Button>().enabled = true;
        }
        else
        {
            cardIcons[0].GetComponent<Image>().sprite = originIcons[0];
            cardIcons[0].GetComponent<Button>().enabled = false;
        }
        if (myitemType_Robots.Count != 0)
        {
            cardIcons[2].GetComponent<Image>().sprite = changeIcons[2];
            cardIcons[2].GetComponent<Button>().enabled = true;
        }
        else
        {
            cardIcons[2].GetComponent<Image>().sprite = originIcons[2];
            cardIcons[2].GetComponent<Button>().enabled = false;
        }
        if (myitemType_Plants.Count != 0)
        {
            cardIcons[1].GetComponent<Image>().sprite = changeIcons[1];
            cardIcons[1].GetComponent<Button>().enabled = true;
        }
        else
        {
            cardIcons[1].GetComponent<Image>().sprite = originIcons[1];
            cardIcons[1].GetComponent<Button>().enabled = false;
        }
        if (myitemType_Animals.Count != 0)
        {
            cardIcons[3].GetComponent<Image>().sprite = changeIcons[3];
            cardIcons[3].GetComponent<Button>().enabled = true;
        }
        else
        {
            cardIcons[3].GetComponent<Image>().sprite = originIcons[3];
            cardIcons[3].GetComponent<Button>().enabled = false;
        }
        if (myitemType_People.Count != 0)
            MyCardType("PEOPLE");
        else if(myitemType_Robots.Count != 0)
            MyCardType("ROBOTS");
        else if(myitemType_Plants.Count != 0)
            MyCardType("PLANTS");
        else if(myitemType_Animals.Count != 0)
            MyCardType("ANIMALS");

    }
    public void MyCardSet(List<Item> items)
    {
        myitemIndex = 0;
        myChoiceList = items.OrderBy(item => item.sid).ToList();

        myitem = myChoiceList[myitemIndex];

        cardinfo = inventoryChild[0].transform.Find("CardSet").gameObject;

        //cardinfo.transform.Find("Memo").GetComponent<Text>().text = myitem.memo;
        for (int i = 0; i < cardinfo.transform.Find("Stars").childCount; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < myitem.star; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(true);
        }
        cardinfo.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = myitem.cardName;
        switch (myitem.cardType)
        {
            case "PEOPLE":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[0];
                break;
            case "ROBOTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[1];
                break;
            case "PLANTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[2];
                break;
            case "ANIMALS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[3];
                break;
        }
        for (int i = 0; i < cardImgs.Length; i++)
        {
            if (myitem.cardName == cardImgs[i].name)
            {
                cardinfo.transform.Find("Character").gameObject.GetComponent<Image>().sprite = cardImgs[i];
            }
        }

        if (myitem.cardStatus == "언락완료")
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(false);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[1];
            unLockBtn.GetComponent<Button>().enabled = false;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();

            card.transform.GetChild(0).gameObject.SetActive(false);
            card.transform.GetChild(1).gameObject.SetActive(true);

            card.transform.GetChild(1).Find("Memo").GetComponent<TextMeshProUGUI>().text = myitem.memo;
            card.transform.GetChild(1).Find("Hptxt").GetComponent<TextMeshProUGUI>().text = myitem.healthPoint.ToString();
            card.transform.GetChild(1).Find("Powertxt").GetComponent<TextMeshProUGUI>().text = myitem.power.ToString();
            float hp = myitem.healthPoint;
            float power = myitem.power;
            card.transform.GetChild(1).Find("Hp").GetComponent<Slider>().value = hp / 10000;
            card.transform.GetChild(1).Find("Power").GetComponent<Slider>().value = power / 10000;
            card.transform.GetChild(1).Find("TrophyTxt").GetComponent<TextMeshProUGUI>().text = ((myitem.healthPoint + myitem.power) / 2).ToString();
            card.transform.GetChild(1).Find("Name").GetComponent<Text>().text = myitem.cardName;
            for (int i = 0; i < card.transform.GetChild(1).Find("Stars").childCount; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < myitem.star; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(true);
            }
            switch (myitem.cardType)
            {
                case "PEOPLE":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[0];
                    break;
                case "ROBOTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[1];
                    break;
                case "PLANTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[2];
                    break;
                case "ANIMALS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[3];
                    break;
            }
        }
        else
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(true);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[0];
            unLockBtn.GetComponent<Button>().enabled = true;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            unLockBtn.GetComponent<Button>().onClick.AddListener(() => UnLockButton(myitem.cardPurchaseId));
            card.transform.GetChild(0).gameObject.SetActive(true);
            card.transform.GetChild(1).gameObject.SetActive(false);
        }
/*        for (int i = 0; i < characterNookiImg.Length; i++)
        {
            if (myitem.cardName == characterNookiImg[i].name)
            {
                character2DObject.gameObject.GetComponent<SpriteRenderer>().sprite = characterNookiImg[i];
            }
        }*/
        myCard_PreviousButton.gameObject.SetActive(myitemIndex > 0);
        myCard_NextButton.gameObject.SetActive(myitemIndex < myChoiceList.Count - 1);
    }
    public void MyCardNextButton()
    {
        myitemIndex++;
        myCard_PreviousButton.gameObject.SetActive(true);
        if (myChoiceList.Count-1 <= myitemIndex)
        {
            myCard_NextButton.gameObject.SetActive(false);
        }

        myitem = myChoiceList[myitemIndex];

        cardinfo = inventoryChild[0].transform.Find("CardSet").gameObject;

        //cardinfo.transform.Find("Memo").GetComponent<Text>().text = myitem.memo;
        for (int i = 0; i < cardinfo.transform.Find("Stars").childCount; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < myitem.star; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(true);
        }
        cardinfo.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = myitem.cardName;
        switch (myitem.cardType)
        {
            case "PEOPLE":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[0];
                break;
            case "ROBOTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[1];
                break;
            case "PLANTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[2];
                break;
            case "ANIMALS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[3];
                break;
        }
        for (int i = 0; i < cardImgs.Length; i++)
        {
            if (myitem.cardName == cardImgs[i].name)
            {
                cardinfo.transform.Find("Character").gameObject.GetComponent<Image>().sprite = cardImgs[i];
            }
        }

        if (myitem.cardStatus == "언락완료")
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(false);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[1];
            unLockBtn.GetComponent<Button>().enabled = false;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();

            card.transform.GetChild(0).gameObject.SetActive(false);
            card.transform.GetChild(1).gameObject.SetActive(true);
            card.transform.GetChild(1).Find("Memo").GetComponent<TextMeshProUGUI>().text = myitem.memo;
            card.transform.GetChild(1).Find("Hptxt").GetComponent<TextMeshProUGUI>().text = myitem.healthPoint.ToString();
            card.transform.GetChild(1).Find("Powertxt").GetComponent<TextMeshProUGUI>().text = myitem.power.ToString();
            float hp = myitem.healthPoint;
            float power = myitem.power;
            card.transform.GetChild(1).Find("Hp").GetComponent<Slider>().value = hp / 10000;
            card.transform.GetChild(1).Find("Power").GetComponent<Slider>().value = power / 10000;
            card.transform.GetChild(1).Find("TrophyTxt").GetComponent<TextMeshProUGUI>().text = ((myitem.healthPoint + myitem.power) / 2).ToString();
            card.transform.GetChild(1).Find("Name").GetComponent<Text>().text = myitem.cardName;
            for (int i = 0; i < card.transform.GetChild(1).Find("Stars").childCount; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < myitem.star; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(true);
            }
            switch (myitem.cardType)
            {
                case "PEOPLE":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[0];
                    break;
                case "ROBOTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[1];
                    break;
                case "PLANTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[2];
                    break;
                case "ANIMALS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[3];
                    break;
            }
        }
        else
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(true);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[0];
            unLockBtn.GetComponent<Button>().enabled = true;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            unLockBtn.GetComponent<Button>().onClick.AddListener(() => UnLockButton(myitem.cardPurchaseId));

            card.transform.GetChild(0).gameObject.SetActive(true);
            card.transform.GetChild(1).gameObject.SetActive(false);
        }

/*        for (int i = 0; i < characterNookiImg.Length; i++)
        {
            if (myitem.cardName == characterNookiImg[i].name)
            {
                character2DObject.gameObject.GetComponent<SpriteRenderer>().sprite = characterNookiImg[i];
            }
        }*/
    }
    public void MyCardPreviousButton()
    {
        myitemIndex--;
        myCard_NextButton.gameObject.SetActive(true);
        if (0 >= myitemIndex)
        {
            myCard_PreviousButton.gameObject.SetActive(false);
        }

        myitem = myChoiceList[myitemIndex];

        cardinfo = inventoryChild[0].transform.Find("CardSet").gameObject;

        //cardinfo.transform.Find("Memo").GetComponent<Text>().text = myitem.memo;
        for (int i = 0; i < cardinfo.transform.Find("Stars").childCount; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < myitem.star; i++)
        {
            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(true);
        }
        cardinfo.transform.Find("CardName").GetComponent<TextMeshProUGUI>().text = myitem.cardName;
        switch (myitem.cardType)
        {
            case "PEOPLE":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[0];
                break;
            case "ROBOTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[1];
                break;
            case "PLANTS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[2];
                break;
            case "ANIMALS":
                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardLine[3];
                break;
        }
        for (int i = 0; i < cardImgs.Length; i++)
        {
            if (myitem.cardName == cardImgs[i].name)
            {
                cardinfo.transform.Find("Character").gameObject.GetComponent<Image>().sprite = cardImgs[i];
            }
        }

/*
        for (int i = 0; i < characterNookiImg.Length; i++)
        {
            if (myitem.cardName == characterNookiImg[i].name)
            {
                character2DObject.gameObject.GetComponent<SpriteRenderer>().sprite = characterNookiImg[i];
            }
        }*/
        if (myitem.cardStatus == "언락완료")
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(false);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[1];
            unLockBtn.GetComponent<Button>().enabled = false;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();

            card.transform.GetChild(0).gameObject.SetActive(false);
            card.transform.GetChild(1).gameObject.SetActive(true);
            card.transform.GetChild(1).Find("Memo").GetComponent<TextMeshProUGUI>().text = myitem.memo;
            card.transform.GetChild(1).Find("Hptxt").GetComponent<TextMeshProUGUI>().text = myitem.healthPoint.ToString();
            card.transform.GetChild(1).Find("Powertxt").GetComponent<TextMeshProUGUI>().text = myitem.power.ToString();
            float hp = myitem.healthPoint;
            float power = myitem.power;
            card.transform.GetChild(1).Find("Hp").GetComponent<Slider>().value = hp / 10000;
            card.transform.GetChild(1).Find("Power").GetComponent<Slider>().value = power / 10000;
            card.transform.GetChild(1).Find("TrophyTxt").GetComponent<TextMeshProUGUI>().text = ((myitem.healthPoint + myitem.power) / 2).ToString();
            card.transform.GetChild(1).Find("Name").GetComponent<Text>().text = myitem.cardName;
            for (int i = 0; i < card.transform.GetChild(1).Find("Stars").childCount; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < myitem.star; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(true);
            }
            switch (myitem.cardType)
            {
                case "PEOPLE":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[0];
                    break;
                case "ROBOTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[1];
                    break;
                case "PLANTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[2];
                    break;
                case "ANIMALS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[3];
                    break;
            }
        }
        else
        {
            cardinfo.transform.Find("Screen").gameObject.SetActive(true);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[0];
            unLockBtn.GetComponent<Button>().enabled = true;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            unLockBtn.GetComponent<Button>().onClick.AddListener(() => UnLockButton(myitem.cardPurchaseId));

            card.transform.GetChild(0).gameObject.SetActive(true);
            card.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void UnLockButton(int data)
    {
        StartCoroutine(UnLock(data));
    }
    private IEnumerator UnLock(int cardPurchaseId)
    {
        string status_Check_MailUrl = $"https://your-server-domain.com/v1/card-purchase/{cardPurchaseId}";

        UnityWebRequest request = UnityWebRequest.Put(status_Check_MailUrl, "");

        request.SetRequestHeader("Authorization", "Bearer " + DataBase.instance.WebRequestManager.token);

        // 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공");
            yield return StartCoroutine(MyCard());

            /*            yield return StartCoroutine(Fade(0, 1));
                        inventoryChild[0].gameObject.SetActive(false);
                        character2DObject.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                        Animator animator = inventoryChild[1].GetComponent<Animator>();
                        animator.SetTrigger("close");
                        inventoryChild[1].gameObject.SetActive(true);

                        GameObject cardinfo = inventoryChild[1].transform.Find("CardSet").gameObject;

                        cardinfo.transform.Find("Memo").GetComponent<Text>().text = myitem.memo;
                        for (int i = 0; i < cardinfo.transform.Find("Stars").childCount; i++)
                        {
                            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(false);
                        }
                        for (int i = 0; i < myitem.star; i++)
                        {
                            cardinfo.transform.Find("Stars").GetChild(i).gameObject.SetActive(true);
                        }
                        cardinfo.transform.Find("CardName").GetComponent<Text>().text = myitem.cardName;
                        switch (myitem.cardType)
                        {
                            case "PEOPLE":
                                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[0];
                                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[0];
                                break;
                            case "ROBOTS":
                                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[1];
                                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[1];
                                break;
                            case "PLANTS":
                                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[2];
                                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[2];
                                break;
                            case "ANIMALS":
                                cardinfo.transform.Find("CardIcon").gameObject.GetComponent<Image>().sprite = cardIconImgs[3];
                                cardinfo.transform.Find("CardImg").gameObject.GetComponent<Image>().sprite = choiceCardLine[3];
                                break;
                        }
                        for (int i = 0; i < cardImgs.Length; i++)
                        {
                            if (myitem.cardName == cardImgs[i].name)
                            {
                                cardinfo.transform.Find("Character").gameObject.GetComponent<Image>().sprite = cardImgs[i];
                            }
                        }
                        yield return StartCoroutine(Fade(1, 0));
                        animator = inventoryChild[1].GetComponent<Animator>();
                        animator.SetTrigger("open");

                        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 6f);

                        yield return StartCoroutine(Fade(0, 1));*/

/*            Animator animator = inventoryChild[1].GetComponent<Animator>();

            animator = inventoryChild[1].GetComponent<Animator>();
            animator.SetTrigger("open");*/

           /* inventoryChild[0].gameObject.SetActive(true);
            inventoryChild[1].gameObject.SetActive(false);*/

            
            card.transform.GetChild(1).Find("Memo").GetComponent<TextMeshProUGUI>().text = myitem.memo;
            card.transform.GetChild(1).Find("Hptxt").GetComponent<TextMeshProUGUI>().text = myitem.healthPoint.ToString();
            card.transform.GetChild(1).Find("Powertxt").GetComponent<TextMeshProUGUI>().text = myitem.power.ToString();
            float hp = myitem.healthPoint;
            float power = myitem.power;
            card.transform.GetChild(1).Find("Hp").GetComponent<Slider>().value = hp/10000;
            card.transform.GetChild(1).Find("Power").GetComponent<Slider>().value = power/10000;
            card.transform.GetChild(1).Find("TrophyTxt").GetComponent<TextMeshProUGUI>().text = ((myitem.healthPoint + myitem.power) / 2).ToString();
            card.transform.GetChild(1).Find("Name").GetComponent<Text>().text = myitem.cardName;
            switch (myitem.cardType)
            {
                case "PEOPLE":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[0];
                    break;
                case "ROBOTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[1];
                    break;
                case "PLANTS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[2];
                    break;
                case "ANIMALS":
                    card.transform.GetChild(1).GetComponent<Image>().sprite = unLockCardLine[3];
                    break;
            }
            for (int i = 0; i < card.transform.GetChild(1).Find("Stars").childCount; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < myitem.star; i++)
            {
                card.transform.GetChild(1).Find("Stars").GetChild(i).gameObject.SetActive(true);
            }
            //yield return StartCoroutine(Fade(1, 0));

            Animator animator = card.GetComponent<Animator>();
            animator.SetTrigger("Open");

            DataBase.instance.AudioManager.AudioPlay(5);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 2f);

            cardinfo.transform.Find("Screen").gameObject.SetActive(false);

            card.transform.GetChild(0).gameObject.SetActive(false);
            card.transform.GetChild(1).gameObject.SetActive(true);
            DataBase.instance.AudioManager.AudioPlay(3);
            unLockBtn.GetComponent<Image>().sprite = unLockBtnImgs[1];
            unLockBtn.GetComponent<Button>().enabled = false;
            unLockBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            DataBase.instance.RankManager.MY_Ranking();
        }
        else
        {
            Debug.Log("실패");
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
    public void MyCardType(string str)
    {
        switch (str)
        {
            case "PEOPLE":
                MyCardSet(myitemType_People);
                break;
            case "ROBOTS":
                MyCardSet(myitemType_Robots);
                break;
            case "PLANTS":
                MyCardSet(myitemType_Plants);
                break;
            case "ANIMALS":
                MyCardSet(myitemType_Animals);
                break;
        }
    }
    public void ShopType(string str)
    {
        switch (str)
        {
            case "ALL":
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PeopleButton").GetComponent<Image>().sprite = buttonOriginImgs[1];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("RobotButton").GetComponent<Image>().sprite = buttonOriginImgs[2];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PlantButton").GetComponent<Image>().sprite = buttonOriginImgs[3];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AnimalButton").GetComponent<Image>().sprite = buttonOriginImgs[4];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AllButton").GetComponent<Image>().sprite = buttonImgs[0];
                ShopSetting(items_All);
                break;
            case "PEOPLE":
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AllButton").GetComponent<Image>().sprite = buttonOriginImgs[0];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("RobotButton").GetComponent<Image>().sprite = buttonOriginImgs[2];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PlantButton").GetComponent<Image>().sprite = buttonOriginImgs[3];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AnimalButton").GetComponent<Image>().sprite = buttonOriginImgs[4];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PeopleButton").GetComponent<Image>().sprite = buttonImgs[1];
                ShopSetting(items_People);
                break;
            case "ROBOTS":
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AllButton").GetComponent<Image>().sprite = buttonOriginImgs[0];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PeopleButton").GetComponent<Image>().sprite = buttonOriginImgs[1];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PlantButton").GetComponent<Image>().sprite = buttonOriginImgs[3];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AnimalButton").GetComponent<Image>().sprite = buttonOriginImgs[4];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("RobotButton").GetComponent<Image>().sprite = buttonImgs[2];
                ShopSetting(items_Robots);
                break;
            case "PLANTS":
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AllButton").GetComponent<Image>().sprite = buttonOriginImgs[0];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PeopleButton").GetComponent<Image>().sprite = buttonOriginImgs[1];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("RobotButton").GetComponent<Image>().sprite = buttonOriginImgs[2];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AnimalButton").GetComponent<Image>().sprite = buttonOriginImgs[4];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PlantButton").GetComponent<Image>().sprite = buttonImgs[3];
                ShopSetting(items_Plants);
                break;
            case "ANIMALS":
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AllButton").GetComponent<Image>().sprite = buttonOriginImgs[0];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PeopleButton").GetComponent<Image>().sprite = buttonOriginImgs[1];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("RobotButton").GetComponent<Image>().sprite = buttonOriginImgs[2];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("PlantButton").GetComponent<Image>().sprite = buttonOriginImgs[3];
                uichage.uIViewContainer.transform.Find("ShopBox").transform.Find("AnimalButton").GetComponent<Image>().sprite = buttonImgs[4];
                ShopSetting(items_Animals);
                break;
        }
    }
    public void InventoryClose()
    {
        uichage.UIViewControllerClose("Inventory");
    }
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        Image spriteRenderer = fadeOutUIImage;

        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            spriteRenderer.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        spriteRenderer.color = color;
    }
}
