using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="mailInfo",menuName ="mail")]
public class Mail : ScriptableObject
{
    public int id;
    public string title;
    public string content;
    public string dateSend;
    public string goodsName;
    public bool isRead;
    public string quantity;
    public string recipient;
    public string sender;
}
