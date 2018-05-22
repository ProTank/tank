using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineWindow
{
    public enum WindowID
    {
        Idle,
        Battle,
    }

    public static string WindowPackage(WindowID id)
    {
        switch (id)
        {
            case WindowID.Battle:
                return "battle";
            default:
                break;
        }
        return "";
    }

    public static string WindowCom(WindowID id)
    {
        switch (id)
        {
            case WindowID.Battle:
                return "battleMain";
            default:
                break;
        }
        return "";
    }

}