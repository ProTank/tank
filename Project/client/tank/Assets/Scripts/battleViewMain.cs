using UnityEngine;
using FairyGUI;

public class battleViewMain : MonoBehaviour
{

    GComponent _mainView;
    GComponent _moveStickView;
    GComponent _aimStickView;

    JoystickModule _movestick;
    AimstickModule _aimstick;

    RoleManager mRoleManager;

    void Awake()
    {
        mRoleManager = new RoleManager();
        mRoleManager.EnterScene();
    }


    void Start()
    {
        Application.targetFrameRate = 60;
        Stage.inst.onKeyDown.Add(OnKeyDown);

        _mainView = this.GetComponent<UIPanel>().ui;

        _moveStickView = _mainView.GetChild("moveStick").asCom;
        _movestick = new JoystickModule(_moveStickView);

        _aimStickView = _mainView.GetChild("aimStick").asCom;
        _aimstick = new AimstickModule(_aimStickView);
    }

    void OnKeyDown(EventContext context)
    {
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Application.Quit();
        }
    }
}
