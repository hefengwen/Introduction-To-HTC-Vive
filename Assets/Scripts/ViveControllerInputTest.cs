using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInputTest : MonoBehaviour {
    //对正在被跟踪的对象进行一个引用。在这里，也就是一只手柄。
    private SteamVR_TrackedObject trackedObj;
    //Device 属性能够很方便地访问到这个手柄。通过所跟踪的对象的索引来访问控制器的 input，并返回这个 input。
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    //当脚本加载时，trackedObj 会被赋值为 SteamVR_TrackedObject 对象，这个对象和手柄是关联的
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update () {
        //获取手指在 touchpad 上的位置并输出到控制台。
        if (Controller.GetAxis() != Vector2.zero)
        {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }
        //当你按下扳机时，这会打印到控制台。扳机有一个专门的方法用于判断它是否被按下：GetHairTrigger(), GetHairTriggerDown() 和 GetHairTriggerUp()。
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }
        //如果松开扳机，这会打印到控制台。
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }
        //如果按下抓取（grip）键，这会打印到控制台。GetPressDown 方法是用于判断某个按钮已经被按下的标准方法。
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
        }
        //如果释放抓取键，这会打印到控制台。GetPressUp 方法是用于判断某个按钮是否已经被释放的标准方法。
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
	}
}
