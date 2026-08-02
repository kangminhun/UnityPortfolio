using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardScript : MonoBehaviour
{
    public static bool DO_NOT = false;

    [SerializeField]
    private int _state;
    [SerializeField]
    private int _cardValue;
    [SerializeField]
    private bool _initialized = false;

    private Sprite _cardBack;
    private Sprite _cardFace;

    public GameObject _manager;
    private int count;
    public void Start()
    {
        _state = 1;
        GetComponent<Button>().onClick.AddListener(() => flipcard());
    }
    public void setupGraphics(int value)
    {
        _cardBack = _manager.GetComponent<Gamemanager>().getCardBack();
        _cardFace = _manager.GetComponent<Gamemanager>().getCardFace(value);

        Resetcard();
    }
    public void Resetcard()
    {
        _state = 1;

        if (_state == 0 && !DO_NOT)
            GetComponent<Image>().sprite = _cardBack;
        else if (_state == 1 && !DO_NOT)
            GetComponent<Image>().sprite = _cardFace;
    }
    public void flipcard()
    {
        if (_state == 0)
            _state = 1;
        else if (_state == 1)
            _state = 0;

        if (_state == 0 && !DO_NOT)
            GetComponent<Image>().sprite = _cardBack;
        else if (_state == 1 && !DO_NOT)
            GetComponent<Image>().sprite = _cardFace;
        _manager.GetComponent<Gamemanager>().checkCards();

    }
    public void HintCard()
    {
        StartCoroutine(Hint());
    }

    public int cardValue
    {
        get { return _cardValue; }
        set { _cardValue = value; }
    }

    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool initialized
    {
        get { return _initialized; }
        set { _initialized = value; }
    }

    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(0.2F);
        if (_state == 0)
            GetComponent<Image>().sprite = _cardBack;
        else if (_state == 1)
            GetComponent<Image>().sprite = _cardFace;
        DO_NOT = false;
    }
    IEnumerator Hint()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();

        // 3번 깜박이는 동안 버튼기능 삭제 

        GetComponent<Image>().sprite = _cardBack;
        yield return new WaitForSeconds(.3f);
        GetComponent<Image>().sprite = _cardFace;
        yield return new WaitForSeconds(.3f);
        GetComponent<Image>().sprite = _cardBack;
        yield return new WaitForSeconds(.3f);
        GetComponent<Image>().sprite = _cardFace;
        yield return new WaitForSeconds(.3f);
        GetComponent<Image>().sprite = _cardBack;
        yield return new WaitForSeconds(.3f);
        GetComponent<Image>().sprite = _cardFace;


        GetComponent<Button>().onClick.AddListener(flipcard);
        //yield return new WaitForSeconds(.3f);
    }
}
