using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class UIManager : MonoBehaviour
{
    Dictionary<DefineWindow.WindowID, WindowInfo> mShownWindows = new Dictionary<DefineWindow.WindowID, WindowInfo>();
    DefineWindow.WindowID mCurWindow = DefineWindow.WindowID.Idle;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        mCurWindow = DefineWindow.WindowID.Battle;
        mShownWindows.Add(DefineWindow.WindowID.Battle, new battleViewMain());
        GComponent view = GetComponent<UIPanel>().ui;
        mShownWindows[mCurWindow].SetView(view);
        mShownWindows[mCurWindow].BeforeShown();
        mShownWindows[mCurWindow].Show();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowWindow<T>(DefineWindow.WindowID id) where T : WindowInfo
    {
        if (mCurWindow == id)
            return;
        if (mCurWindow != DefineWindow.WindowID.Idle)
        {
            if (!mShownWindows.ContainsKey(mCurWindow))
                MyLog.LogError("Window[" + id.ToString() + "] not in shown list, can't hide.");
            else
                mShownWindows[mCurWindow].Hide();
        }
        if (mShownWindows.ContainsKey(id))
        {
            mShownWindows[id].Reshow();
        }
        else
        {
            string packName = DefineWindow.WindowPackage(id);
            string comName = DefineWindow.WindowCom(id);
            GComponent view = UIPackage.CreateObject(packName, comName).asCom;
            mShownWindows.Add(id, System.Activator.CreateInstance<T>());
            mShownWindows[id].SetView(view);
            mShownWindows[id].BeforeShown();
            mShownWindows[id].Show();
        }
        mCurWindow = id;
    }

}
