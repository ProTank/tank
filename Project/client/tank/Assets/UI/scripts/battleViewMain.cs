using UnityEngine;
using FairyGUI;

public class battleViewMain : MonoBehaviour
{

	GComponent _mainView;
	GComponent _moveStickView;
	GComponent _aimStickView;

	GTextField _moveDegreeText;
	GTextField _aimVectorText;

	JoystickModule _movestick;
	AimstickModule _aimstick;

	void Start () {

		Application.targetFrameRate = 60;
		Stage.inst.onKeyDown.Add(OnKeyDown);

		_mainView = this.GetComponent<UIPanel>().ui;

		_moveStickView = _mainView.GetChild ("moveStick").asCom;
		_moveDegreeText = _mainView.GetChild ("moveDegree").asTextField;
		_movestick = new JoystickModule(_moveStickView);
		_movestick.onMove.Add(movestickMove);
		_movestick.onEnd.Add(movestickEnd);

		_aimStickView = _mainView.GetChild ("aimStick").asCom;
		_aimVectorText = _mainView.GetChild ("aimVector").asTextField;
		_aimstick = new AimstickModule(_aimStickView);
		_aimstick.onMove.Add(aimstickMove);
		
	}

	void movestickMove(EventContext context)
	{
		float degree = (float)context.data;
		_moveDegreeText.text = "" + degree;
        
	}

	void movestickEnd()
	{
		_moveDegreeText.text = "degree";
	}

	void aimstickMove(EventContext context)
	{
		Vector2 r = (Vector2)context.data;
		_aimVectorText.text = "offsetX= " + r.x + " offsetY= " + r.y;
	}

	void OnKeyDown(EventContext context)
	{
		if (context.inputEvent.keyCode == KeyCode.Escape)
		{
			Application.Quit();
		}
	}
}
