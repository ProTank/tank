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

    private float mMoveSpeed = 10;
    private float mGravity = 20;

    private float mCameraDistanceV = 40;
    private float mCameraDistanceH = 10;
    private float mCameraDegree = 40;
    private float mSmoothing = 5f;

    public virtual void RegisterEvent() { }
    public virtual void RemoveEvent() { }

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
        InitRole();
        UpdateCamera();
    }

    void InitRole()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
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
        UpdateCamera();
    }

    void OnDestroy()
    {
        RemoveEvent();        
    }

}
