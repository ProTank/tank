using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager
{
    Dictionary<int, Role> mRoleObjs = new Dictionary<int, Role>();

    public void EnterScene()
    {
        CreateSelf();
    }

    private void CreateSelf()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("models/Tank01")) as GameObject;
        if (obj)
        {
            Role player = obj.AddComponent<Player>();
            mRoleObjs.Add(0, player);
        }
    }

}
