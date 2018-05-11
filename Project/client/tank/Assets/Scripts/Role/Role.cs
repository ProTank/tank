using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class Role : MonoBehaviour
{
    private CharacterController mCC;

    private bool mMoving;
    private Vector3 mMoveDirection;

    private float mMoveSpeed = 10;
    private float mGravity = 20;

    void Awake()
    {
        mCC = GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start()
    {
        RegisterEvent();
    }

    void RegisterEvent()
    {
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Role.ROLE_MOVE_DIRECTION, Move);
        BaseEvent.Instance.RegisterEvent(GlobleEventDefine.Role.ROLE_MOVE_END, MoveEnd);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (mMoving)
        {            
            if (mCC.isGrounded)
            {
                direction = mMoveDirection * mMoveSpeed * Time.deltaTime;                
            }            
        }
        direction.y -= mGravity * Time.deltaTime;
        direction = transform.TransformDirection(direction);
        mCC.Move(direction);
    }

    void Move(EventContext context)
    {
        float rad = (float)context.data;
        //MyLog.LogError("onMoveDir:" + rad);
        float x = Mathf.Cos(rad);
        float z = -Mathf.Sin(rad);
        mMoveDirection = new Vector3(x, 0, z);
        mMoving = true;
    }

    void MoveEnd(EventContext context)
    {
        mMoving = false;
        mMoveDirection = Vector3.zero;
    }

}
