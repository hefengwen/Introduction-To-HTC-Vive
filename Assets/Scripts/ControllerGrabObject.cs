using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    //一个 GameObject，用于保存当前与之碰撞的触发器（trigger），这样你才能抓住这个对象。
    private GameObject collidingObject;
    //一个 GameObject，用于保存玩家当前抓住的对象。
    private GameObject objectInHand;
    /// <summary>
    /// 这个方法接受一个碰撞体作为参数，并将它的 GameObject 保存到 collidingObject 变量，以便抓住和释放这个对象。
    /// </summary>
    /// <param name="col"></param>
    private void SetCollidingObject(Collider col)
    {
        //如果玩家已经抓着某些东西了，或者这个对象没有一个刚性体，则不要将这个 GameObject 作为可以抓取目标。
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        //将这个对象作为可以抓取的目标。
        collidingObject = col.gameObject;
    }

    //当触发器碰撞体进入另一个碰撞体时，将另一个碰撞体作为可以抓取的目标。
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    //和第一段类似,但不同的是玩家已经将手柄放在一个对象上并持续一段时间。如果没有这段代码，碰撞会失败或者会导致异常
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    //当碰撞体退出一个对象，放弃目标，这段代码会将 collidingObject 设为 null 以删除目标对象。
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
            return;
        collidingObject = null;
    }
    //抓取物体
    private void GrabObject()
    {
        //在玩家手中的 GameObject 转移到 objectInHand 中，将 collidingObject 中保存的 GameObject 移除。
        objectInHand = collidingObject;
        collidingObject = null;
        //添加一个连接对象，调用下面的 FixedJoint 方法将手柄和 GameObject 连接起来。
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }
    //创建一个固定连接并加到手柄中，并设置连接属性，使它坚固，不那么容易断裂。最后返回这个连接。
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        //确定控制器上一定有一个固定连接。
        if (GetComponent<FixedJoint>())
        {
            //删除这个连接上所连的对象，然后销毁这个连接
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            //将玩家放开物体时手柄的速度和角度赋给这个物体，这样会形成了一个完美的抛物线。
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        //将 objectInHand 变量置空。
        objectInHand = null;
    }

    void Update () {
        //当玩家按下扳机，同时手上有一个可以抓取的对象，则将对象抓住。
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
                GrabObject();
        }
        //当玩家松开扳机，同时手柄上连接着一个物体，则放开这个物体。
        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
                ReleaseObject();
        }
	}
}
