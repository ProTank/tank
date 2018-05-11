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
	float _lastStageX;
	float _lastStageY;
	float _offsetX;
	float _offsetY;

	GButton _thumb;
	GButton _base;
	GObject _touchArea;

	int _touchId;
	Tweener _tweener;

	public EventListener onMove { get; private set; }
	public EventListener onEnd { get; private set; }

	public int radius { get; set; }

	public JoystickModule(GComponent mainView)
	{
		onMove = new EventListener(this, "onMove");
		onEnd = new EventListener(this, "onEnd");

		_thumb = mainView.GetChild ("thumb").asButton;
		_base = mainView.GetChild ("base").asButton;
		_touchArea = mainView.GetChild ("touchArea");

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

    public void Trigger(EventContext context)
	{
		OnTouchBegin(context);
	}

    private void OnTouchBegin(EventContext context)
	{
		if (_touchId == -1) {//First touch
			InputEvent evt = (InputEvent)context.data;
			_touchId = evt.touchId;

			if (_tweener != null) {
				_tweener.Kill ();
				_tweener = null;
			}

			Vector2 pt = GRoot.inst.GlobalToLocal (new Vector2 (evt.x - _offsetX, evt.y - _offsetY));
			float bx = pt.x;
			float by = pt.y;

			//Debug.Log ("x= " + bx + " y= " + by);

			if (bx < 0)
				bx = 0;
			else if (bx > _touchArea.width)
				bx = _touchArea.width;

			if (by > GRoot.inst.height)
				by = GRoot.inst.height;
			else if (by < _touchArea.y)
				by = _touchArea.y;

			_lastStageX = bx;
			_lastStageY = by;
			_startStageX = bx;
			_startStageY = by;

			_thumb.selected = true;
			_base.visible = true;
			_thumb.SetXY(bx, by);
			_base.SetXY(bx, by);

			context.CaptureTouch();
		}
	}

	private void OnTouchMove(EventContext context)
	{
		InputEvent evt = (InputEvent)context.data;
		if (_touchId != -1 && evt.touchId == _touchId) {

			Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x - _offsetX, evt.y - _offsetY));
			float bx = pt.x;
			float by = pt.y;
			float moveX = bx - _lastStageX;
			float moveY = by - _lastStageY;
			_lastStageX = bx;
			_lastStageY = by;
			float thumbX = _thumb.x + moveX;
			float thumbY = _thumb.y + moveY;

			float offsetX = thumbX - _startStageX;
			float offsetY = thumbY - _startStageY;

			float rad = Mathf.Atan2(offsetY, offsetX);
			float degree = rad * 180 / Mathf.PI;

			_base.selected = true;
			_base.rotation = degree + 90;

			float maxX = radius * Mathf.Cos(rad);
			float maxY = radius * Mathf.Sin(rad);
			if (Mathf.Abs(offsetX) > Mathf.Abs(maxX))
				offsetX = maxX;
			if (Mathf.Abs(offsetY) > Mathf.Abs(maxY))
				offsetY = maxY;

			thumbX = _startStageX + offsetX;
			thumbY = _startStageY + offsetY;
			if (thumbX < 0)
				thumbX = 0;
			if (thumbY > GRoot.inst.height)
				thumbY = GRoot.inst.height;

			_thumb.SetXY(thumbX, thumbY);

			this.onMove.Call(degree);
            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Role.ROLE_MOVE_DIRECTION, rad);
        }
	}

	private void OnTouchEnd(EventContext context)
	{
		InputEvent inputEvt = (InputEvent)context.data;
		if (_touchId != -1 && inputEvt.touchId == _touchId) {
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

			this.onEnd.Call();            
            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Role.ROLE_MOVE_END);
        }
	}


}
