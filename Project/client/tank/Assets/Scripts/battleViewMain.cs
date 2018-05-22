using UnityEngine;
using FairyGUI;

public class battleViewMain : WindowInfo
{
    GComponent _moveStickView;
    GComponent _aimStickView;

    JoystickModule _movestick;
    AimstickModule _aimstick;

    RoleManager mRoleManager;

    public override void InitComponnet()
    {
        _moveStickView = mView.GetChild("moveStick").asCom;
        _movestick = new JoystickModule(_moveStickView);

        _aimStickView = mView.GetChild("aimStick").asCom;
        _aimstick = new AimstickModule(_aimStickView);
    }

    public override void OnStart()
    {
        mRoleManager = new RoleManager();
        mRoleManager.EnterScene();
    }

    public override void OnReshow()
    {
        mRoleManager.ResetScene();
    }

}
