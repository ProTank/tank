using System.Collections.Generic;
using FairyGUI;
using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Dictionary<WindowID, Window> windowDict;

    private Window curWindow;

    void Awake()
    {
        MyLog.LogError("------Start------");
        Instance = this;
        DontDestroyOnLoad(this);        
    }

    void Start()
    {
        RegisterWindow();
    }

    private void RegisterWindow()
    {

    }

    public void ShowWindow(WindowID id)
    {
        if (!windowDict.ContainsKey(id))
        {
            MyLog.LogError("Window[" + id.ToString() + "] not register.");
            return;
        }
        if (curWindow != null)
        {
            curWindow.Hide();
        }
        windowDict[id].Show();
        curWindow = windowDict[id];
    }

    public void HideWindow(WindowID id)
    {
        if (!windowDict.ContainsKey(id))
        {
            MyLog.LogError("Window[" + id.ToString() + "] not register.");
            return;
        }
        windowDict[id].Hide();
    }


}
