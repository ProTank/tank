using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class Role : MonoBehaviour
{
    protected Animator mAC;
    private CharacterController mCC;
    protected Camera mMainCamera;

    protected bool mMoving = false;
    protected float mMoveDegree;

    protected bool mRotating = false;
    private float mRotateSpeed = 10;
    protected Vector2 mRotateDistance;
    protected float mCameraDegree = 30;

    private float mMoveSpeed = 5;
    private float mGravity = 20;    

    public virtual void RegisterEvent() { }
    public virtual void RemoveEvent() { }
    
    public virtual void OnFixedUpdate() { }

    public virtual void OnInitRole() { }

    void Awake()
    {
        mAC = GetComponent<Animator>();
        mCC = GetComponent<CharacterController>();
        mMainCamera = Camera.main;
    }

    // Use this for initialization
    void Start()
    {
        RegisterEvent();
        OnInitRole();
    }  

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (mMoving)
        {
            if (mCC.isGrounded)
            {
                float rotDegree = mMoveDegree + 90;
                float offsetX = Mathf.Sin((rotDegree) * Mathf.PI / 180);
                float offsetZ = Mathf.Cos((rotDegree) * Mathf.PI / 180);
                direction = new Vector3(offsetX, 0, offsetZ) * mMoveSpeed * Time.deltaTime;
            }
        }
        direction.y -= mGravity * Time.deltaTime;
        direction = transform.TransformDirection(direction);
        mCC.Move(direction);

        if (mRotating)
        {
            Vector3 rot = transform.localEulerAngles;
            rot += new Vector3(0, mRotateSpeed * mRotateDistance.x * Time.deltaTime, 0);
            transform.localEulerAngles = rot;
            mCameraDegree += mRotateSpeed * mRotateDistance.y * Time.deltaTime;
            mRotateDistance = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    void OnDestroy()
    {
        RemoveEvent();        
    }

}
