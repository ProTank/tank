using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class Player : Role
{

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

    void Move(EventContext context)
    {
        float rad = (float)context.data;
        //MyLog.LogError("onMoveDir:" + rad);
        float x = Mathf.Cos(rad);
        float z = -Mathf.Sin(rad);
        mMoveDirection = new Vector3(x, 0, z);        
        mMoving = true;
        mAC.SetInteger("State", 1);
    }

    void MoveEnd(EventContext context)
    {
        mMoving = false;
        mMoveDirection = Vector3.zero;
        mAC.SetInteger("State", 0);
    }

    void Rotate(EventContext context)
    {
        mRotating = true;
        Vector2 vec = (Vector2)context.data;
        //Debug.LogError("vec:" + vec);
        mRotateDegree += new Vector2(vec.y, vec.x);
    }

    void RotateEnd(EventContext context)
    {
        mRotating = false;
    }

}
