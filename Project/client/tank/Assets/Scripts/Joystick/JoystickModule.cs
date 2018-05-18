using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JoystickModule : BaseEvent
{
    float _InitX;
    float _InitY;
    float _startStageX;
    float _startStageY;
    float _offsetX;
    float _offsetY;

    GButton _thumb;
    GButton _base;
    GObject _touchArea;

    int _touchId;
    Tweener _tweener;

    public int radius { get; set; }

    public JoystickModule(GComponent mainView)
    {
        _thumb = mainView.GetChild("thumb").asButton;
        _base = mainView.GetChild("base").asButton;
        _touchArea = mainView.GetChild("touchArea");

        _touchId = -1;
        radius = 65;

        _InitX = _base.x;
        _InitY = _base.y;
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

            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }

            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x - _offsetX, evt.y - _offsetY));
            float bx = pt.x;
            float by = pt.y;
            _startStageX = bx;
            _startStageY = by;

            _thumb.selected = true;            
            _thumb.SetXY(bx, by);
            _base.visible = true;
            _base.SetXY(bx, by);

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

            float rad = Mathf.Atan2(offsetY, offsetX);
            float degree = rad * 180 / Mathf.PI;
            float maxX = radius * Mathf.Cos((degree) * Mathf.PI / 180);
            float maxY = radius * Mathf.Sin((degree) * Mathf.PI / 180);
            if (Mathf.Abs(offsetX) > Mathf.Abs(maxX))
                offsetX = maxX;
            if (Mathf.Abs(offsetY) > Mathf.Abs(maxY))
                offsetY = maxY;
            _thumb.SetXY(offsetX + _startStageX, offsetY + _startStageY);

            _base.selected = true;
            _base.rotation = degree + 90;

            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_MOVE_DIRECTION, degree);
        }
    }

    private void OnTouchEnd(EventContext context)
    {
        InputEvent inputEvt = (InputEvent)context.data;
        if (_touchId != -1 && inputEvt.touchId == _touchId)
        {
            _touchId = -1;

            _base.selected = false;
            _base.visible = false;

            _tweener = _thumb.TweenMove(new Vector2(_InitX, _InitY), 0.3f).OnComplete(() =>
                {
                    _tweener = null;
                    _thumb.selected = false;
                    _base.visible = true;
                    _base.SetXY(_InitX, _InitY);
                }
            );

            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_MOVE_END);
        }
    }


}
