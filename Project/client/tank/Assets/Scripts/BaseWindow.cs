using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class WindowInfo
{
    protected GComponent mView;

    public void SetView(GComponent view)
    {
        mView = view;
    }

    public void BeforeShown()
    {
        InitComponnet();
        RegisterEvent();
    }

    public void Show()
    {
        GRoot.inst.AddChild(mView);
        OnStart();
    }

    public void Hide()
    {
        OnHide();
        mView.visible = false;
    }

    public void Reshow()
    {
        mView.visible = true;
        OnReshow();
    }

    public virtual void InitComponnet() { }
    public virtual void RegisterEvent() { }
    public virtual void OnStart() { }
    public virtual void OnHide() { }
    public virtual void OnReshow() { }

}
