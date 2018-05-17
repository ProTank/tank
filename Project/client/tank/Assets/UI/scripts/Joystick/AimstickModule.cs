using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimstickModule : EventDispatcher {

	float _startStageX;
	float _startStageY;
	float _offsetX;
	float _offsetY;

	float _areaWidth;
	float _areaHeight;

	GObject _touchArea;

	int _touchId;

	public EventListener onMove { get; private set; }
	public EventListener onEnd { get; private set; }

	public int len { get; set; }

	public AimstickModule(GComponent mainView)
	{
		onMove = new EventListener(this, "onMove");
		onEnd = new EventListener(this, "onEnd");

		_touchId = -1;

		_touchArea = mainView.GetChild ("touchArea");
		_areaWidth = _touchArea.width;
		_areaHeight = _touchArea.height;

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

			Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x - _offsetX, evt.y - _offsetY));
			float bx = pt.x;
			float by = pt.y;

			if (bx < 0)
				bx = 0;
			else if (bx > _touchArea.width)
				bx = _touchArea.width;

			if (by > GRoot.inst.height)
				by = GRoot.inst.height;
			else if (by < _touchArea.y)
				by = _touchArea.y;

			_startStageX = bx;
			_startStageY = by;

			//Debug.Log ("bsx= " + bx + " bsy= " + by);

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

			float offsetX = bx - _startStageX;
			float offsetY = by - _startStageY;
            _startStageX = bx;
            _startStageY = by;

            //Debug.Log ("sx= " + _startStageX + " sy= " + _startStageY);
            //Debug.Log ("mbx= " + bx + " mby= " + by);
            //Debug.Log("offsetX= " + offsetX + " offsetY= " + offsetY);

            Vector2 offsetTo = new Vector2 (offsetX/_areaWidth*30, offsetY/_areaHeight*30);

			this.onMove.Call(offsetTo);
            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_DIRECTION, offsetTo);
		}
	}

	private void OnTouchEnd(EventContext context)
	{
		InputEvent inputEvt = (InputEvent)context.data;
		if (_touchId != -1 && inputEvt.touchId == _touchId)
		{
			_touchId = -1;
			this.onEnd.Call();
            BaseEvent.Instance.TriggerEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_END);
        }
	}
}
