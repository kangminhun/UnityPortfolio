using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PluginManager : MonoBehaviour
{
    [DllImport("I18N")]
    private static extern float I18NMethodScript1();

    [DllImport("I18N.CJK")]
    private static extern float I18NCJKMethodScript1();

    [DllImport("I18N.West")]
    private static extern float I18NWestMethodScript1();
    public void CallI18N()
    {
        float result1 = I18NMethodScript1();
    }
    public void CallI18NCJK()
    {
        float resultCJK1 = I18NCJKMethodScript1();
    }
    public void CallI18NWest()
    {
        float resultWest1 = I18NWestMethodScript1();
    }
}
