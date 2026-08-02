using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OneCardVelue : MonoBehaviour,  IBeginDragHandler,IDragHandler, IEndDragHandler, IPointerEnterHandler,IPointerExitHandler
{
    public CardInformation information;
    [HideInInspector]
    public int cardValue;
    public enum stage
    {
        Idle,Draw,Down,Out
    } 
    public stage cardStage;
    public PlayerCard playercard;
    private HorizontalLayoutGroup horizontal;
    void Start()
    {
        gameObject.GetComponent<Image>().sprite= information.sprite;
        cardValue = information.value;
        horizontal = playercard.contentPoint.GetComponent<HorizontalLayoutGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        playercard.draging = true;
        cardStage = stage.Draw;
        GetComponent<RectTransform>().sizeDelta = new Vector2(534, 342);
        horizontal.enabled = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        playercard.OnDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        playercard.draging = false;
        horizontal.enabled = true;
        cardStage = stage.Down;
        playercard.ChooseCard();
        playercard.OffDrag();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playercard.draging)
            GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 530f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!playercard.draging)
            GetComponent<RectTransform>().sizeDelta = new Vector2(534, 342);
    }

}
