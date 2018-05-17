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
    protected Vector3 mMoveDirection;

    protected bool mRotating = false;
    private float mRotateSpeed = 20;
    protected Vector2 mRotateDegree; 

    private float mMoveSpeed = 10;
    private float mGravity = 20;

    private float mCameraDistance = 10;
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
        InitCamera();
    }

    void InitRole()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    void InitCamera()
    {
        Vector3 rolePos = transform.position;
        float offsetH = mCameraDistance * Mathf.Cos(Mathf.PI * mCameraDegree / 180);
        float offsetV = mCameraDistance * Mathf.Sin(Mathf.PI * mCameraDegree / 180);
        mMainCamera.transform.position = new Vector3(rolePos.x, rolePos.y + offsetV, rolePos.z - offsetH);
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
                direction = mMoveDirection * mMoveSpeed * Time.deltaTime;                
            }            
        }
        direction.y -= mGravity * Time.deltaTime;
        direction = transform.TransformDirection(direction);
        mCC.Move(direction);
    }

    void FixedUpdate()
    {
        if (!mRotating)
        {
            Vector3 rolePos = transform.position;
            float offsetH = mCameraDistance * Mathf.Cos(Mathf.PI * mCameraDegree / 180);
            float offsetV = mCameraDistance * Mathf.Sin(Mathf.PI * mCameraDegree / 180);
            Vector3 targetCampos = new Vector3(rolePos.x, rolePos.y + offsetV, rolePos.z - offsetH);
            mMainCamera.transform.position = Vector3.Lerp(mMainCamera.transform.position, targetCampos, mSmoothing * Time.deltaTime);
            mMainCamera.transform.localEulerAngles = transform.localEulerAngles + new Vector3(mCameraDegree, 0, 0);
        }
        else
        {
            if (mRotateDegree == Vector2.zero)
                return;
            Vector3 vec = mMainCamera.transform.localEulerAngles;
            vec.x = 0;
            mMainCamera.transform.localEulerAngles = vec;
            mMainCamera.transform.Rotate(-mRotateDegree.x * Time.deltaTime * mRotateSpeed, mRotateDegree.y * Time.deltaTime * mRotateSpeed, 0);
            //Debug.LogError("angle:" + mMainCamera.transform.localEulerAngles + ", mRotateDegree:" + mRotateDegree);
            mRotateDegree = Vector2.zero;
            mMainCamera.transform.localEulerAngles += new Vector3(mCameraDegree, 0, 0);
        }
    }

    void OnDestroy()
    {
        RemoveEvent();        
    }

}
