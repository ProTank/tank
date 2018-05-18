using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimstickModule : EventDispatcher
{

    float _startStageX;
    float _startStageY;
    float _offsetX;
    float _offsetY;

    GObject _touchArea;

    int _touchId;

    public AimstickModule(GComponent mainView)
    {
        _touchId = -1;

        _touchArea = mainView.GetChild("touchArea");

        _offsetX = mainView.x;
        _offsetY = mainView.y;

        _touchArea.onTouchBegin.Add(this.OnTouchBegin);
        _touchArea.onTouchMove.Add(this.OnTouchMove);
        _touchArea.onTouchEnd.Add(this.OnTouchEnd);
    }

    private void OnTouchBegin(EventContext context)
    {
        if (_touchId == -1)
        {
            InputEvent evt = (InputEvent)context.data;
            _touchId = evt.touchId;

            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x - _offsetX, evt.y - _offsetY));
            float bx = pt.x;
            float by = pt.y;
            _startStageX = bx;
            _startStageY = by;

            context.CaptureTouch();
        }
    }

    private void OnTouchMove(EventContext context)
    {
        InputEvent evt = (InputEvent)context.data;
        if (_touchId != -1 && evt.touchId == _touchId)
        {
            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x - _offsetX, evt.y - _offsetY));
            float bx = pt.x;
            float by = pt.y;
            float offsetX = bx - _startStageX;
            float offsetY = by - _startStageY;
            _startStageX = bx;
            _startStageY = by;
            Vector2 offsetTo = new Vector2(offsetX, offsetY);

            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_DIRECTION, offsetTo);
        }
    }

    private void OnTouchEnd(EventContext context)
    {
        InputEvent inputEvt = (InputEvent)context.data;
        if (_touchId != -1 && inputEvt.touchId == _touchId)
        {
            _touchId = -1;
            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_END);
        }
    }
}
