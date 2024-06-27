using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class RotatePoint : MonoBehaviour
{
    [Header("***ת��ui��ʾ***")]
    [SerializeField] GameObject panel_GetGold;
    [SerializeField] TextMeshProUGUI txt_time;
    [SerializeField] TextMeshProUGUI txt_instruction;
    [SerializeField] TextMeshProUGUI txt_currentGold;

    [Header("***ת�̰�ť***")]
    [SerializeField] Button btn_rotate;
    [SerializeField] Button btn_getGold;
    [SerializeField] Button btn_close;

    [Header("***��Ч��Դ***")]
    [SerializeField] AudioClip rotateClip;

    
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float continueTime;
    //��õĽ��
    private bool canGet;
    private int currentGold;

    //ת���ǶȺ�ʱ��
    private bool isRotate;
    private float angle;
    private DateTime lastTime;
    private float rotateTime;

    private void Awake()
    {
        //��ťע���¼�
        btn_rotate.onClick.AddListener(SetTime);
        btn_getGold.onClick.AddListener(GetGold);
        btn_getGold.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        transform.eulerAngles = Vector3.zero;

        GlobalValue.GetLastTime();
        lastTime = GlobalValue.lastTime;
        Debug.Log("��һ�γ齱��ʱ���� "+lastTime);

        DateTime nowTime = DateTime.Now;
        TimeSpan timeSpan = nowTime.Subtract(lastTime).Duration();

        canGet = timeSpan.TotalSeconds> 300;
        //canGet = true;

        //��ת��ť�Ƿ���ã���������ʾ
        if (canGet==false)
        {
            txt_instruction.gameObject.SetActive(true);
            txt_time.gameObject.SetActive(true);
            btn_rotate.interactable = false;
            Debug.Log("ָ�벻����");
        }
        else
        {
            Debug.Log("ָ�������");
            txt_instruction.gameObject.SetActive(false);
            txt_time.gameObject.SetActive(false);
            btn_rotate.interactable = true;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        //��ʾ������һ�γ齱��ʱ��
        if (canGet==false)
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan span = nowTime.Subtract(lastTime).Duration();
            double leftTime = 300 - span.TotalSeconds;

            
            if(leftTime > 0)
            {
                txt_time.text = ((int)(leftTime / 60/60)).ToString() + " : "+((int)(leftTime/60)).ToString() 
                    + " : "+ ((int)(leftTime%60)).ToString();
            }
            else if (leftTime <= 0)
            {
                txt_instruction.gameObject.SetActive(false);
                txt_time.gameObject.SetActive(false);
            }
            //else if (leftTime < 1&&leftTime>0)
            //{
            //    txt_time.text = ((int)(leftTime*60)).ToString() + " Seconds";
            //}

        }


        if (isRotate==false)
        {
            return;
        }
        if (Time.time < rotateTime)//ת��ת��
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        else//ת��ʱ�䵽��,����ֹͣΪֹ
        {
            //rotate ��angle+360 �ſ���תһ��֮��ŵ���Ŀ��λ�ö���������С�ĽǶ�ȥת���õ�
            transform.DORotate(new Vector3(0, 0, -angle + 360), 1).OnComplete(
                () =>
                {
                    btn_close.gameObject.SetActive(true);
                    btn_getGold.gameObject.SetActive(true);
                    CancelInvoke();

                    lastTime = DateTime.Now;
                    PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
                });
            isRotate = false;
        }

    }
    /// <summary>
    /// ����ָ��ת��ʱ��
    /// </summary>
    private void SetTime()
    {
        UIManager.instance.PlayButtonSound();
        btn_rotate.interactable = false;
        btn_close.gameObject.SetActive(false);
        rotateTime = Time.time + continueTime;

        SetAngle();
        isRotate = true;
        InvokeRepeating("PlaySound",0.2f,0.2f);
    }
    private void PlaySound()
    {
        AudioSourceManager.instance.PlaySound(rotateClip);
    }
    /// <summary>
    /// ����ת���Ƕ�
    /// </summary>
    private void SetAngle()
    {
        //�� UnitySystem֮��Ҳ����һ��Random,���Ե�����System����ʱ�򣬾�Ҫ��UnityEngine.Random
        int randomNum = UnityEngine.Random.Range(0, 8);
        currentGold = UIManager.instance.awardNum[randomNum];
        txt_currentGold.text = currentGold.ToString();
        angle = randomNum * 45;
    }
    /// <summary>
    /// ���ת����Ұ�ť
    /// </summary>
    private void GetGold()
    {
        UIManager.instance.PlayGetGoldSound();
        GlobalValue.coins += currentGold;
        UIManager.instance.ChangePlayerGoldNum();//��ʾ�ܽ��
        //btn_rotate.interactable = true;

        btn_getGold.gameObject.SetActive(false);
        panel_GetGold.SetActive(false);
    }
    /// <summary>
    /// ���³齱֮���ʱ��
    /// </summary>
    private void OnDisable()
    {
        
    }
}
