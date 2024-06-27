using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour
{
    #region 摄像机移动的第二种方法
    private bool stopFollow;
    [SerializeField] private Transform rocketHeadTrans;
    public void MoveController(bool stopFollow=false)
    {
        Debug.Log("stopFollow的值是 "+stopFollow);
        if (!stopFollow)
        {
            Debug.Log("内部stopFollow的值是 " + stopFollow);
            //每次火箭脱落，摄像机，就往上偏移一些
            transform.DOLocalMove(transform.localPosition+new Vector3(0.02f,0.1f,0.1f),1);
            //每次火箭脱落的时候，摄像机向右倾斜，制造火箭拐弯的效果
            transform.DORotate(transform.localEulerAngles+new Vector3(0,0,5),1);
        }
        else
        {
            this.stopFollow = stopFollow;
            transform.SetParent(null) ;
            DOTween.KillAll();
        }
    }
    private void LateUpdate()
    {
        if (stopFollow)
        {
            //Debug.Log("最后摄像机不跟随，看向火箭");
            transform.LookAt(rocketHeadTrans);
        }
    } 
    /// <summary>
    /// 火箭未发射之前，摄像机的位置
    /// </summary>
    public void ChangeCameraPos()
    {
        if (GlobalValue.rocketLength<8)
        {
            transform.localPosition = new Vector3(-0.24f,0.94f,-1.91f);
        }
        else
        {
            transform.localPosition = new Vector3(-0.28f,1.42f,-2.87f);
        }
    }
    #endregion
    #region 摄像机移动的第一种方法
    //[SerializeField] Transform followTarget;
    //private Vector3 offset;
    //private bool stopFollow;
    //void Start()
    //{
    //    offset = followTarget.position - transform.position;
    //    Debug.Log("stopFollow的默认值是" + stopFollow);
    //    Debug.Log("stopFollow的取反值是" + !stopFollow);
    //}

    //// Update is called once per frame
    //private void LateUpdate()
    //{
    //    if (!stopFollow)
    //    {
    //        transform.position = followTarget.position - offset;
    //    }
    //    transform.LookAt(followTarget);
    //}

    //public void GetNewOffsetAndFollowTarget(Transform newFollowTarget, bool stopFollow = false)
    //{
    //    followTarget = newFollowTarget;
    //    this.stopFollow = stopFollow;
    //    offset = followTarget.position - transform.position;
    //} 
    #endregion
}
