using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour
{
    #region ������ƶ��ĵڶ��ַ���
    private bool stopFollow;
    [SerializeField] private Transform rocketHeadTrans;
    public void MoveController(bool stopFollow=false)
    {
        Debug.Log("stopFollow��ֵ�� "+stopFollow);
        if (!stopFollow)
        {
            Debug.Log("�ڲ�stopFollow��ֵ�� " + stopFollow);
            //ÿ�λ�����䣬�������������ƫ��һЩ
            transform.DOLocalMove(transform.localPosition+new Vector3(0.02f,0.1f,0.1f),1);
            //ÿ�λ�������ʱ�������������б�������������Ч��
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
            //Debug.Log("�������������棬������");
            transform.LookAt(rocketHeadTrans);
        }
    } 
    /// <summary>
    /// ���δ����֮ǰ���������λ��
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
    #region ������ƶ��ĵ�һ�ַ���
    //[SerializeField] Transform followTarget;
    //private Vector3 offset;
    //private bool stopFollow;
    //void Start()
    //{
    //    offset = followTarget.position - transform.position;
    //    Debug.Log("stopFollow��Ĭ��ֵ��" + stopFollow);
    //    Debug.Log("stopFollow��ȡ��ֵ��" + !stopFollow);
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
