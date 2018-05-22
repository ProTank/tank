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
        GameObject obj = GameObject.Instantiate(Resources.Load("models/Role01")) as GameObject;
        if (obj)
        {
            Role player = obj.AddComponent<Player>();
            if (!mRoleObjs.ContainsKey(0))
                mRoleObjs.Add(0, player);
            else
                mRoleObjs[0] = player;
        }
    }

    public void ResetScene()
    {
        if (mRoleObjs.ContainsKey(0) && mRoleObjs[0] != null)
        {

        }
        else
        {
            CreateSelf();
        }
    }

}
