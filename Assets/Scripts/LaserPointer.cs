using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        //制造出一束新的激光，然后保存一个它的引用。
        laser = Instantiate(laserPrefab);
        //保存激光的 transform 组件。
        laserTransform = laser.transform;
    }

    //这个变量用于引用 Laser 预制件。
    public GameObject laserPrefab;
    //这个变量用于引用一个 Laser 实例。
    private GameObject laser;
    //一个 Transform 组件，方便后面适用。
    private Transform laserTransform;
    //激光击中的位置。
    private Vector3 hitPoint;

    private void ShowLaser(RaycastHit hit)
    {
        //显示激光。
        laser.SetActive(true);
        //激光位于手柄和投射点之间。我们可以用 Lerp 方法，这样我们只需要给它两个端点，以及一个距离百分比即可。如果我们设置这个百分比为 0.5，也就是 50%，这会返回一个中点的位置。
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        //将激光照射到 hitPoint 的位置。
        laserTransform.LookAt(hitPoint);
        //
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance); ;
    }

    
    void Update () {
        //如果 touchpad 被按下…
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;
            //从手柄发射激光。如果激光照射到某样物体，保存射到的位置并显示激光。
            if (Physics.Raycast(trackedObj.transform.position,transform.forward,out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
            }
        }
        else
        {
            //当玩家放开 touchpad，隐藏激光。
            laser.SetActive(false);
        }
	}
}
