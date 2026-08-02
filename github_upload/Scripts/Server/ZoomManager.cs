using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using Vimeo.SimpleJSON;

public class ZoomManager : MonoBehaviour
{
    public VideoPlayer player;
    public void Create()
    {
        if (DataBase.instance.WebRequestManager.type == UserType.admin)
            DataBase.instance.WebRequestManager.StartZoom();
        else
            DataBase.instance.WebRequestManager.ZoomJoin();
    }
    public void Test()
    {
        player.gameObject.SetActive(true);
    }
}
