using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class MyCardGame : MonoBehaviour
{
    [SerializeField] private ShopItemManager shopItemManager;
    [SerializeField] private List<Item> myItem;

    public GameObject myCardPrefab;
    public RectTransform deckPoint;

    private List<GameObject> myCardList;
    private GameObject card;

    public GameObject myCard;
    public Button attactButton;
    public Button shieldButton;

    public GameObject enemyCard;
    public List<Item> enemyCardList;
    private int enemyIndex;
    public Slider enemyHpbar;
    public GameObject[] enemyCardDack;
    public Image[] enemyDackCardImgs;
    private int enemyMaxHp;
    private float enemyHp;
    private float enemyPower;

    public Sprite[] enemycharacterImages;
    public Sprite[] characterImages;
    public Sprite[] iconImage;
    public Sprite[] cardImage;

    public Slider hpbar;
    private float hp;
    private int maxHp;
    private float power;

    private bool myTurn;
    private bool choiceOn;

    private int myCardDieCount;
    public GameObject myAttackEffact;
    public GameObject enemyAttackEffact;

    public GameObject result;
    public Button resultButton;
    public AudioSource resultAudio;
    public AudioClip[] resultAudioClips;
    public Sprite[] resultButtonImg;

    public ParticleSystem goldParticle;
    public ParticleSystem scene_FX_Confetti1;
    public Text nameTxt_Copy;
    public Text nameTxt;

    private string enemyAttackType;
    private string myAttackType;
    private string mySkilType;
    private string enemySkillType;
    private string[] myeffact;

    //임시
    public CardGameManager manager;

    public float shakeDistance = 10f; // 흔들림 거리
    public float shakeDuration = 0.5f; // 흔들림 지속 시간

    private Vector3 originalPosition;

    private int myattackCount;
    private int enemyattackCount;

    public GameObject clearImg;
    public Sprite[] clearSprites;
    private void Start()
    {
        attactButton.onClick.AddListener(() => MyAttack());
        shieldButton.onClick.AddListener(() => MyShield());
    }
    public void GameStart()
    {
        nameTxt_Copy.text = nameTxt.text;
        enemyAttackEffact.gameObject.SetActive(false);
        myAttackEffact.gameObject.SetActive(false);
        myCardDieCount = 0;
        enemyIndex = 0;
        if (myCardList != null)
        {
            for (int i = 0; i < myCardList.Count; i++)
            {
                Destroy(myCardList[i]);
            }
        }
        attactButton.gameObject.SetActive(false);
        shieldButton.gameObject.SetActive(false);
        myCard.transform.transform.Find("CardSet").gameObject.SetActive(false);
        enemyCard.transform.transform.Find("CardSet").gameObject.SetActive(false);
        myCardList = new List<GameObject>();
        myItem = new List<Item>();
        Transform spawnPoint;
        List<Item> items = new List<Item>();
        items = shopItemManager.myitemType_All_Sharing.OrderByDescending(item => (item.power + item.healthPoint) / 2).ToList();
        myItem = items.Take(4).ToList();
        for (int i = 0; i < myItem.Count; i++)
        {
            int sum = i;
            if (myItem[sum].cardStatus == "언락완료")
            {
                card = Instantiate(myCardPrefab, deckPoint);
                spawnPoint = card.transform.Find("CardSet").gameObject.transform;
                for (int s = 0; s < spawnPoint.Find("Stars").transform.childCount; s++)
                {
                    spawnPoint.Find("Stars").transform.GetChild(s).gameObject.SetActive(false);
                }
                for (int j = 0; j < characterImages.Length; j++)
                {
                    if (string.Equals(myItem[sum].cardName, characterImages[j].name, StringComparison.OrdinalIgnoreCase))
                    {
                        spawnPoint.Find("Character").gameObject.GetComponent<Image>().sprite = characterImages[j];
                    }
                    spawnPoint.Find("CardName").gameObject.GetComponent<TextMeshProUGUI>().text = myItem[sum].cardName;

                    if (myItem[sum].cardType == "PEOPLE")
                    {
                        spawnPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardImage[0];
                        spawnPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite = iconImage[0];
                    }
                    else if (myItem[sum].cardType == "ROBOTS")
                    {
                        spawnPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardImage[1];
                        spawnPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite = iconImage[1];
                    }
                    else if (myItem[sum].cardType == "PLANTS")
                    {
                        spawnPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardImage[2];
                        spawnPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite = iconImage[2];
                    }
                    else if (myItem[sum].cardType == "ANIMALS")
                    {
                        spawnPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite = cardImage[3];
                        spawnPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite = iconImage[3];
                    }
                    for (int k = 0; k < myItem[sum].star; k++)
                    {
                        spawnPoint.Find("Stars").transform.GetChild(k).gameObject.SetActive(true);
                    }
                }
                card.GetComponent<Button>().enabled = true;
                spawnPoint.Find("Screen").gameObject.SetActive(false);
                card.GetComponent<Button>().onClick.AddListener(() => Click(sum));
                myCardList.Add(card);
            }
        }
        hpbar.value = 100 / 100;
        for (int i = 0; i < enemyCardDack.Length; i++)
        {
            enemyCardDack[i].transform.Find("CardSet").gameObject.transform.Find("Screen").gameObject.SetActive(false);
            for (int j = 0; j < enemyCardDack[i].transform.Find("CardSet").gameObject.transform.Find("Stars").transform.childCount; j++)
            {
                enemyCardDack[i].transform.Find("CardSet").gameObject.transform.Find("Stars").transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        GameSet();
        choiceOn = true;
    }
    private void GameSet()
    {
        enemyCard.transform.Find("CardSet").gameObject.SetActive(true);
        enemyCard.transform.Find("CardSet").GetComponent<Animator>().SetTrigger("Open");
        Transform enemyCardSetPoint = enemyCard.transform.Find("CardSet").gameObject.transform;
        for (int i = 0; i < enemyCardSetPoint.transform.Find("Stars").childCount; i++)
        {
            enemyCardSetPoint.transform.Find("Stars").GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < enemycharacterImages.Length; i++)
        {
            if (string.Equals(enemyCardList[enemyIndex].cardName, enemycharacterImages[i].name, StringComparison.OrdinalIgnoreCase))
            {
                enemyCardSetPoint.Find("Character").gameObject.GetComponent<Image>().sprite = enemycharacterImages[i];
                enemyCardSetPoint.Find("CardName").gameObject.GetComponent<TextMeshProUGUI>().text = enemyCardList[enemyIndex].cardName;
                for (int j  = 0; j < enemyCardList[enemyIndex].star; j++)
                {
                    enemyCardSetPoint.transform.Find("Stars").GetChild(j).gameObject.SetActive(true);
                }
                enemyHpbar.gameObject.transform.Find("Bar").gameObject.transform.Find("Character").gameObject.transform.Find("CharacterImage").gameObject.GetComponent<Image>().sprite = enemycharacterImages[i];
            }
            for (int j = 0; j < enemyCardList.Count; j++)
            {
                if (string.Equals(enemyCardList[j].cardName, enemycharacterImages[i].name, StringComparison.OrdinalIgnoreCase))
                {
                    enemyDackCardImgs[j].sprite = enemycharacterImages[i];
                }
                for (int k = 0; k < enemyCardList[j].star; k++)
                {
                    enemyCardDack[j].transform.Find("CardSet").transform.Find("Stars").GetChild(k).gameObject.SetActive(true);
                }
                enemyCardDack[j].transform.Find("CardSet").Find("CardName").gameObject.GetComponent<TextMeshProUGUI>().text = enemyCardList[j].cardName;
            }
        }
        enemyMaxHp = enemyCardList[enemyIndex].healthPoint;
        enemyPower = enemyCardList[enemyIndex].power;

        enemyHp = enemyMaxHp;
        enemyHpbar.value = enemyHp / enemyMaxHp;
    }
    private void EnemyAttakType()
    {
        int randomType = UnityEngine.Random.Range(0, 10);
        if (randomType <= 6)
        {
            enemyAttackType = "Attack";
        }
        else
        {
            enemyAttackType = "Shield";
        }
    }
    public void MyShield()
    {
        if (myTurn)
        {
            EnemyAttakType();
            attactButton.gameObject.SetActive(false);
            shieldButton.gameObject.SetActive(false);
            myTurn = false;
            myAttackType = "Shield";
            StartCoroutine(MyAttackDelay());
        }
    }
    public void MyAttack()
    {
        if (myTurn)
        {
            EnemyAttakType();
            myAttackType = "Attack";
            attactButton.gameObject.SetActive(false);
            shieldButton.gameObject.SetActive(false);
            if (myattackCount < 2)
                myattackCount++;
            else
                myattackCount = 0;

            if (myattackCount == 0)
                mySkilType = myeffact[0];
            else if (myattackCount == 1)
                mySkilType = myeffact[1];
            else if (myattackCount == 2)
                mySkilType = myeffact[2];

            myTurn = false;
            float damage = 0;
            if(enemyAttackType == "Attack")
            {
                damage = power;
            }
            else
            {
                damage = power / 2;
            }
            enemyHp -= damage;
            if (enemyHp > 0)
            {
                StartCoroutine(MyAttackDelay());
            }
            else
            {
                StartCoroutine(EnemyDie());
            }
        }
    }
    private IEnumerator EnemyDie()
    {
        EffactOn("My");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ShakeUIElement(enemyCard.GetComponent<RectTransform>()));

        yield return new WaitForSeconds(.5f);

        enemyHpbar.value = enemyHp / enemyMaxHp;
        enemyAttackEffact.gameObject.SetActive(false);
        myAttackEffact.gameObject.SetActive(false);

        enemyCard.transform.Find("CardSet").gameObject.SetActive(false);
        enemyIndex++;

        for (int i = 0; i < enemyIndex; i++)
        {
            enemyCardDack[i].transform.Find("CardSet").gameObject.transform.Find("Screen").gameObject.SetActive(true);
        }

        if (enemyCardList.Count - 1 < enemyIndex)
        {

            Debug.Log("승리");
            Debug.Log(DataBase.instance.CardGameRoundManager.myRound_Index - 1);
            Debug.Log(DataBase.instance.CardGameRoundManager.roundScore);
            if (DataBase.instance.CardGameRoundManager.roundScore == DataBase.instance.CardGameRoundManager.myRound_Index -1 || DataBase.instance.CardGameRoundManager.roundScore == 10)
            {
                if (DataBase.instance.CardGameRoundManager.myRound_Index < 6)
                {
                    DataBase.instance.WebRequestManager.CardGameRoundUP();
                    yield return new WaitForSeconds(2f);
                    
                    manager.gameObject.SetActive(false);
                }
                else if(DataBase.instance.CardGameRoundManager.roundScore < 7)
                {
                    clearImg.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = clearSprites[DataBase.instance.CardGameRoundManager.myStation_Index - 1];
                    clearImg.gameObject.SetActive(true);
                    DataBase.instance.WebRequestManager.CardGameStageUP();
                }
                else
                {
                    clearImg.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = clearSprites[DataBase.instance.CardGameRoundManager.myStation_Index - 1];
                    clearImg.gameObject.SetActive(true);
                    DataBase.instance.WebRequestManager.CardGameRoundSet();
                }
            }
            else
            {
                manager.gameObject.SetActive(false);
            }

        }
        else
        {
            GameSet();
            attactButton.gameObject.SetActive(true);
            shieldButton.gameObject.SetActive(true);
            myTurn = true;
        }
    }

    private IEnumerator MyAttackDelay()
    {
        if (myAttackType == "Attack")
        {
            EffactOn("My");
            if (enemyAttackType == "Shield")
                EffactOn("EnemyAttackType_Shield");

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(ShakeUIElement(enemyCard.GetComponent<RectTransform>()));

            yield return new WaitForSeconds(.5f);

            enemyHpbar.value = enemyHp / enemyMaxHp;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }
        enemyAttackEffact.gameObject.SetActive(false);
        myAttackEffact.gameObject.SetActive(false);
        if (enemyAttackType == "Attack")
            EnemyAttact();
        else
            EnemyShield();
    }
    private void EnemyAttact()
    {
        if (!myTurn)
        {
            float damage = 0;

            if (enemyattackCount == 0)
                enemySkillType = enemyCardList[enemyIndex].skillEffect3;
            else if (enemyattackCount == 1)
                enemySkillType = enemyCardList[enemyIndex].skillEffect2;
            else if (enemyattackCount == 2)
                enemySkillType = enemyCardList[enemyIndex].skillEffect3;

            if (enemyattackCount < 2)
                enemyattackCount++;
            else
                enemyattackCount = 0;

            if (myAttackType == "Attack")
            {
                damage = enemyPower;
            }
            else
            {
                damage = enemyPower / 2;
            }
            hp -= damage;
            if (hp <= 0)
            {
                StartCoroutine(MyDie());
            }
            else
            {
                StartCoroutine(EnemyAttackDelay());
            }
        }
    }
    private void EnemyShield()
    {
        StartCoroutine(EnemyAttackDelay());
    }
    private IEnumerator EnemyAttackDelay()
    {
        if (enemyAttackType == "Attack")
        {
            EffactOn("Enemy");
            if (myAttackType == "Shield")
                EffactOn("MyAttackType_Shield");

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(ShakeUIElement(myCard.GetComponent<RectTransform>()));

            yield return new WaitForSeconds(.5f);

            hpbar.value = hp / maxHp;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }
        enemyAttackEffact.gameObject.SetActive(false);
        myAttackEffact.gameObject.SetActive(false);
        myTurn = true;
        attactButton.gameObject.SetActive(true);
        shieldButton.gameObject.SetActive(true);
    }
    private IEnumerator MyDie()
    {
        EffactOn("Enemy");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ShakeUIElement(myCard.GetComponent<RectTransform>()));

        yield return new WaitForSeconds(.5f);
        hpbar.value = hp / maxHp;
        enemyAttackEffact.gameObject.SetActive(false);
        myAttackEffact.gameObject.SetActive(false);
        myCardDieCount++;

        if (myCardDieCount < myItem.Count)
        {
            choiceOn = true;
        }
        else
        {
            Debug.Log("패배");
            FailureAnimation();
        }
        myCard.transform.Find("CardSet").gameObject.SetActive(false);
    }
    private void Click(int num)
    {
        if (choiceOn)
        {
            myTurn = true;
            choiceOn = false;
            attactButton.gameObject.SetActive(true);
            shieldButton.gameObject.SetActive(true);
            myCardList[num].GetComponent<Button>().enabled = false;
            myCard.transform.Find("CardSet").gameObject.SetActive(true);
            myeffact = new string[3];

            Transform myCardPoint = myCard.transform.Find("CardSet").gameObject.transform;
            Transform objPoint = myCardList[num].transform.Find("CardSet").gameObject.transform;
            objPoint.Find("Screen").gameObject.SetActive(true);
            maxHp = myItem[num].healthPoint;
            power = myItem[num].power;
            hp = maxHp;
            hpbar.value = hp / maxHp;

            hpbar.gameObject.transform.Find("Bar").gameObject.transform.Find("Character").gameObject.transform.Find("CharacterImage").gameObject.GetComponent<Image>().sprite = objPoint.Find("Character").gameObject.GetComponent<Image>().sprite;

            myCardPoint.Find("Character").gameObject.GetComponent<Image>().sprite = objPoint.Find("Character").gameObject.GetComponent<Image>().sprite;
            myCardPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite = objPoint.Find("CardImg").gameObject.GetComponent<Image>().sprite;
            myCardPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite = objPoint.Find("CardIcon").gameObject.GetComponent<Image>().sprite;
            myCardPoint.Find("CardName").gameObject.GetComponent<TextMeshProUGUI>().text = myItem[num].cardName;

            for (int i = 0; i < myCardPoint.Find("Stars").gameObject.transform.childCount; i++)
            {
                myCardPoint.Find("Stars").gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int j = 0; j < myItem[num].star; j++)
            {
                myCardPoint.Find("Stars").gameObject.transform.GetChild(j).gameObject.SetActive(true);
            }

            myeffact[0] = myItem[num].skillEffect3;
            myeffact[1] = myItem[num].skillEffect2;
            myeffact[2] = myItem[num].skillEffect3;
        }
    }
    private void EffactOn(string str)
    {
        if (str == "Enemy")
        {
            enemyAttackEffact.gameObject.SetActive(true);
            for (int i = 0; i < enemyAttackEffact.transform.childCount; i++)
            {
                enemyAttackEffact.transform.GetChild(i).gameObject.SetActive(false);
            }
 
            enemyAttackEffact.transform.Find(enemySkillType.ToLower()).gameObject.SetActive(true);

        }
        else if (str == "My")
        {
            myAttackEffact.gameObject.SetActive(true);
            for (int i = 0; i < myAttackEffact.transform.childCount; i++)
            {
                myAttackEffact.transform.GetChild(i).gameObject.SetActive(false);
            }
            myAttackEffact.transform.Find(mySkilType.ToLower()).gameObject.SetActive(true);

        }
        else if (str == "MyAttackType_Shield")
        {
            myAttackEffact.gameObject.SetActive(true);
            for (int i = 0; i < myAttackEffact.transform.childCount; i++)
            {
                myAttackEffact.transform.GetChild(i).gameObject.SetActive(false);
            }
            myAttackEffact.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (str == "EnemyAttackType_Shield")
        {
            enemyAttackEffact.gameObject.SetActive(true);
            for (int i = 0; i < enemyAttackEffact.transform.childCount; i++)
            {
                enemyAttackEffact.transform.GetChild(i).gameObject.SetActive(false);
            }
            enemyAttackEffact.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void SuccessAnimation()
    {
        // 성공 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 성공 이미지나 파티클 효과를 재생할 수 있습니다
        Debug.Log("성공");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        scene_FX_Confetti1.Play();
        StartCoroutine(GoldParticleOn());
        result.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        resultAudio.clip = resultAudioClips[0];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Success");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[0];
        resultButton.onClick.AddListener(() => DataBase.instance.CardGameRoundManager.Exit());
        DataBase.instance.PointManager.PointUp(100);
        result.transform.Find("Success Paticle").transform.Find("Gold").GetComponentInChildren<Text>().text = "100";
    }
    private void FailureAnimation()
    {
        // 실패 애니메이션을 실행하는 로직을 여기에 작성합니다
        // 예를 들어, 실패 이미지나 메시지를 표시할 수 있습니다
        Debug.Log("실패");
        resultButton.onClick.RemoveAllListeners();
        result.gameObject.SetActive(true);
        result.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        result.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        resultAudio.clip = resultAudioClips[1];
        resultAudio.Play();
        result.GetComponent<Animator>().SetTrigger("Fail");
        resultButton.GetComponent<Image>().sprite = resultButtonImg[1];
        resultButton.onClick.AddListener(() => GameStart());
    }
    public IEnumerator GoldParticleOn()
    {
        goldParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        goldParticle.gameObject.SetActive(true);
    }

    private IEnumerator ShakeUIElement(RectTransform uiElement)
    {
        float elapsedTime = 0f;
        originalPosition = uiElement.anchoredPosition;
        while (elapsedTime < shakeDuration)
        {
            // 좌우로 흔들림 효과를 주기 위해 삼각함수(sin)를 사용합니다.
            float xOffset = Mathf.Sin(Time.time * Mathf.PI * 2f * 5f) * shakeDistance;
            Vector3 newPosition = originalPosition + new Vector3(xOffset, 0f, 0f);
            uiElement.anchoredPosition = newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 돌아갑니다.
        uiElement.anchoredPosition = originalPosition;
    }
}
