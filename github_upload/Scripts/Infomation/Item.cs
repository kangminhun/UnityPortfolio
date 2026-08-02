using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string cardName;
    public string cardType;
    public int cardId;
    public string memo;
    public int sid;
    public int diamond;
    public int point;
    public int star;
    public bool isUse;
    public int healthPoint;
    public int power;
    public string cardStatus;
    public int cardPurchaseId;
    public string property;
    public string skillEffect;
    public string skillEffect2;
    public string skillEffect3;
}
