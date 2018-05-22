using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class Player : Role
{
    private float mCameraDistanceV = 12;
    private float mCameraDistanceH = 3;
    private float mSmoothing = 5f;

    public override void RegisterEvent()
    {
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Player.PLAYER_MOVE_DIRECTION, Move);
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Player.PLAYER_MOVE_END, MoveEnd);
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_DIRECTION, Rotate);
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_END, RotateEnd);
    }

    public override void RemoveEvent()
    {
        BaseEvent.Instance.RemoveEvent(GlobleEventDefine.Player.PLAYER_MOVE_DIRECTION, Move);
        BaseEvent.Instance.RemoveEvent(GlobleEventDefine.Player.PLAYER_MOVE_END, MoveEnd);
        BaseEvent.Instance.RemoveEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_DIRECTION, Rotate);
        BaseEvent.Instance.RemoveEvent(GlobleEventDefine.Player.PLAYER_TOWARDS_END, RotateEnd);
    }

    public override void OnInitRole()
    {
        base.OnInitRole();

        transform.localEulerAngles = new Vector3(0, 0, 0);
        UpdateCamera();
    }

    void Move(EventContext context)
    {
        mMoveDegree = (float)context.data;       
        mMoving = true;
        float x = Mathf.Cos((360 - mMoveDegree) * Mathf.PI / 180);
        float y = Mathf.Sin((360 - mMoveDegree) * Mathf.PI / 180);
        mAC.SetFloat("walkX", x);
        mAC.SetFloat("walkY", y);

        mAC.SetInteger("moveState", (int)DefineRole.MoveState.Walk);
        //Debug.LogError("mMoveDegree:" + (90 + mMoveDegree));
    }

    void MoveEnd(EventContext context)
    {
        mMoving = false;
        mAC.SetFloat("moveState", (int)DefineRole.MoveState.Idle);
    }

    void Rotate(EventContext context)
    {
        mRotating = true;
        Vector2 vec = (Vector2)context.data;
        mRotateDistance += new Vector2(vec.x, vec.y);
        //Debug.LogError("mRotateDistance:" + mRotateDistance);
    }

    void RotateEnd(EventContext context)
    {
        mRotating = false;
    }

    void UpdateCamera()
    {
        Vector3 rolePos = transform.position;
        float degree = 90 - transform.localEulerAngles.y;
        float x = rolePos.x - mCameraDistanceH * Mathf.Cos(degree * Mathf.PI / 180);
        float z = rolePos.z - mCameraDistanceH * Mathf.Sin(degree * Mathf.PI / 180);
        Vector3 targetCampos = new Vector3(x, rolePos.y + mCameraDistanceH, z);
        mMainCamera.transform.position = Vector3.Lerp(mMainCamera.transform.position, targetCampos, mSmoothing * Time.deltaTime);
        mMainCamera.transform.localEulerAngles = transform.localEulerAngles + new Vector3(mCameraDegree, 0, 0);
    }

    public override void OnFixedUpdate()
    {
        UpdateCamera();
    }

}
